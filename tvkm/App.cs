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
    private ScreenHub sh;

    public LongpollDaemon? Longpoll;
    public static int UserId { get; private set; }

    public App(ScreenHub sh) : base("TVKM")
    {
        this.sh = sh;
        AddRange(new[]
        {
            new Button("Лента", () => { sh.Push(new AlertPopup("Махо пидор", sh)); }),
            new Button("Сообщения", () =>
            {
                try
                {
                    sh.Push(new DialogsScreen(_api));
                }
                catch (HttpRequestException)
                {
                    sh.Push(new AlertPopup("Сбой подключения. Проверьте сеть.", sh));
                }
            }),
            new Button("Друзья", () => { sh.Push(new FriendsList(_api)); }),
            new Button("Закрыть сессию",
                () => { sh.Push(new AlertPopup("Сделаем позже. Удалите session.txt из рабочей папки.", sh)); }),
            new Button("Выход", sh.Back),
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
                sh.Push(new TextboxPopup("2FA авторизация", "Код из SMS", s1 =>
                {
                    try
                    {
                        var error = AuthByPassword(login, password, s1);
                        if (error != null)
                        {
                            sh.Push(new AlertPopup(error, sh));
                            return;
                        }
                        sh.Back();
                        sh.BackThenPush(this);
                    }
                    catch (OperationCanceledException)
                    {
                        // do nothing
                    }
                    catch
                    {
                        sh.Push(new AlertPopup("Не удалось войти.", sh));
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