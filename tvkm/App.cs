using System.Net;
using Newtonsoft.Json;
using tvkm.Api;
using tvkm.Dialogs;
using tvkm.UIEngine;
using tvkm.UIEngine.Templates;
using VkNet;
using VkNet.Model;
using Button = tvkm.UIEngine.Controls.Button;

namespace tvkm;

public class App : ListScreen
{
    private readonly VkApi _api = new();
    private ScreenStack _stack;

    public LongpollDaemon? Longpoll;
    public static int UserId { get; private set; }

    public App(ScreenStack stack) : base("TVKM")
    {
        this._stack = stack;
        AddRange(new[]
        {
            new Button("Лента", () => { stack.Push(new AlertPopup("Махо пидор", stack)); }),
            new Button("Сообщения", () =>
            {
                try
                {
                    stack.Push(new DialogsScreen(_api));
                }
                catch (HttpRequestException)
                {
                    stack.Push(new AlertPopup("Сбой подключения. Проверьте сеть.", stack));
                }
            }),
            new Button("Друзья", () => { stack.Push(new FriendsList(_api)); }),
            new Button("Закрыть сессию",
                () => { stack.Push(new AlertPopup("Сделаем позже. Удалите session.txt из рабочей папки.", stack)); }),
            new Button("Выход", stack.Back),
        });
    }

    #region Screen control

    public override void OnEnter(ScreenStack stack)
    {
        ConfigManager.ReadSettings();
        UserId = (int) (_api.UserId ?? 0);
        Title = $"TVKM - id{_api.UserId ?? 0}";
        Longpoll = new LongpollDaemon(_api);
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

    private void Auth(long id, string token) => _api.Authorize(new ApiAuthParams {AccessToken = token, UserId = id});

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
                _stack.Push(new TextboxPopup("2FA авторизация", "Код из SMS", s1 =>
                {
                    try
                    {
                        var error = AuthByPassword(login, password, s1);
                        if (error != null)
                        {
                            _stack.Push(new AlertPopup(error, _stack));
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
                        _stack.Push(new AlertPopup("Не удалось войти.", _stack));
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