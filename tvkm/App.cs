using System.Net;
using Newtonsoft.Json;
using tvkm.Api;
using tvkm.Dialogs;
using tvkm.UIEngine;
using tvkm.UIEngine.Controls;
using tvkm.UIEngine.Templates;
using VkNet;
using VkNet.Model;

namespace tvkm;

public class App : LongLoadingListScreen<App>
{
    public readonly VkApi Api = new();
    private ScreenStack<App> _stack;

    public LongpollDaemon? Longpoll;
    public int UserId { get; private set; }

    public App(ScreenStack<App> stack) : base("TVKM")
    {
        this._stack = stack;
        AddRange(new[]
        {
            new Button<App>("Лента", () => { stack.Alert("Махо пидор"); }),
            new Button<App>("Сообщения", () =>
            {
                try
                {
                    stack.Push(new DialogsScreen(stack));
                }
                catch (HttpRequestException)
                {
                    stack.Push(new AlertPopup<App>("Сбой подключения. Проверьте сеть.", stack));
                }
            }),
            new Button<App>("Друзья", () => { stack.Push(new FriendsList()); }),
            new Button<App>("Видео", () => { stack.Push(new VideosScreen()); }),
            new Button<App>("Документы", () => { stack.Push(new DocumentsScreen()); }),
            new Button<App>("Закрыть сессию",
                () =>
                {
                    stack.Push(new ListScreen<App>("Действие удалит session.txt в рабочем каталоге!", new[]
                    {
                        new Button<App>("Да, удалить", () =>
                        {
                            ConfigManager.DeleteSavedToken();
                            stack.Back();
                            stack.Back();
                            Api.LogOut();
                        })
                    }));
                }),
            new Button<App>("Назад", stack.Back),
        });
    }

    #region Screen control

    protected override void Load(ScreenStack<App> stack)
    {
        ConfigManager.ReadConfig();
        UserId = (int) (Api.UserId ?? 0);
        string userName = VkUser.GetName(UserId, Api);
        Title = $"TVKM - id{UserId} ({userName})";
        Longpoll = new LongpollDaemon(this, Api);
        Longpoll.Run();
    }

    public override void OnLeave()
    {
        Longpoll?.Dispose();
        Longpoll = null;
    }

    #endregion

    #region Authorization

    private const int VkmId = 2685278;
    private const string VkmSecret = "lxhD8OD7dMsqtXIm5IUY";

    private void Auth(long id, string token) => Api.Authorize(new ApiAuthParams {AccessToken = token, UserId = id});

    public void RestoreFromFile()
    {
        var data = ConfigManager.ReadSavedToken();
        Auth(data.Item1, data.Item2);
    }

    public string? AuthByPassword(string login, string password, string? code = null)
    {
        using HttpClient http = new();
        http.BaseAddress = new Uri("https://oauth.vk.com");
        var r = http.GetAsync("/token?grant_type=password" +
                              "&client_id=" + VkmId + "&client_secret=" + VkmSecret + "&username=" + login +
                              "&password=" + password +
                              "&2fa_supported=1&scope=notify,friends,photos,audio,video,docs,notes,pages,status,offers,questions,wall,groups,messages,notifications,stats,ads" +
                              (code == null ? "" : $"&code={code}"))
            .Result;


        var s = r.Content.ReadAsStringAsync().Result;

        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(s);
        if (dict.ContainsKey("error_description"))
        {
            if (dict["error"] == "need_validation" && dict["error_description"].Contains("use code param"))
            {
                _stack.Push(new TextboxPopup<App>("2FA авторизация", "Код из SMS", s1 =>
                {
                    try
                    {
                        var error = AuthByPassword(login, password, s1);
                        if (error != null)
                        {
                            _stack.Alert(error);
                            return;
                        }

                        _stack.Back();
                        _stack.BackThenPush(this);
                    }
                    catch (OperationCanceledException)
                    {
                        // do nothing
                    }
                    catch
                    {
                        _stack.Alert("Не удалось войти.");
                    }
                }));
                throw new OperationCanceledException();
            }

            return dict["error_description"];
        }

        Auth(int.Parse(dict["user_id"]), dict["access_token"]);
        ConfigManager.WriteToken(long.Parse(dict["user_id"]), dict["access_token"]);
        return null;
    }

    #endregion
}