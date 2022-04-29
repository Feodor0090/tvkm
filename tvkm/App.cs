using System.Net;
using Newtonsoft.Json;
using tvkm.Api;
using tvkm.Dialogs;
using tvkm.UIEngine;
using VkNet;
using VkNet.Model;
using Button = tvkm.UIEngine.Button;

namespace tvkm;

public class App : ListScreen
{
    private readonly VkApi _api = new();
    private ScreenHub sh;
    private const int VkmId = 2685278;
    private const string VkmSecret = "lxhD8OD7dMsqtXIm5IUY";

    public LongpollDaemon? Longpoll;
    public static int UserId { get; private set; }

    public App(ScreenHub sh) : base("TVKM")
    {
        this.sh = sh;
        AddRange(new[]
        {
            new Button("Лента", () =>
            {
                sh.Push(new SimpleAlert("Махо пидор", sh));
            }),
            new Button("Сообщения", () =>
            {
                try
                {
                    sh.Push(new DialogsScreen(_api));
                }
                catch (HttpRequestException)
                {
                    sh.Push(new SimpleAlert("Сбой подключения. Проверьте сеть.", sh));
                }
            }),
            new Button("Друзья", () =>
            {
                sh.Push(new FriendsList(_api));
            }),
            new Button("Закрыть сессию", () =>
            {
                sh.Push(new SimpleAlert("Сделаем позже. Удалите session.txt из рабочей папки.", sh));
            }),
            new Button("Выход", sh.Back),
        });
    }
    
    public override void OnEnter(ScreenHub screenHub)
    {
        UserId = (int)(_api.UserId ?? 0);
        Title = $"TVKM - id{_api.UserId ?? 0}";
        Longpoll = new LongpollDaemon(_api);
        Longpoll.Run();
    }

    public override void OnLeave()
    {
        Longpoll?.Dispose();
        Longpoll = null;
    }
    
    #region Authorization
    
    public void Auth(int id, string token) => _api.Authorize(new ApiAuthParams { AccessToken = token, UserId = id });

    public void Auth()
    {
        var s = File.ReadAllText("session.txt").Split(' ');
        Auth(int.Parse(s[0]), s[1]);
    }

    public void Auth(string login, string password)
    {
        using HttpClient http = new();
        http.BaseAddress = new Uri("https://oauth.vk.com");
        var r = http.GetAsync("/token?grant_type=password" +
                              "&client_id=" + VkmId + "&client_secret=" + VkmSecret + "&username=" + login +
                              "&password=" + password +
                              "&scope=notify,friends,photos,audio,video,docs,notes,pages,status,offers,questions,wall,groups,messages,notifications,stats,ads")
            .Result;
        if (r.StatusCode != HttpStatusCode.OK)
            throw new ArgumentException();

        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(r.Content.ReadAsStringAsync().Result);
        Auth(int.Parse(dict["user_id"]), dict["access_token"]);
        using StreamWriter sw = new("session.txt", false);
        sw.Write($"{dict["user_id"]} {dict["access_token"]}");
        sw.Flush();
    }
    
    #endregion
    
}