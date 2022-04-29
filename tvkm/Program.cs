using System.Diagnostics;
using tvkm.UIEngine;

namespace tvkm;

public static class Program
{
    private static readonly ScreenHub hub = new ScreenHub(new DefaultControlsScheme());
    private static App _app;

    private sealed class StartupScreen : ListScreen
    {
        public StartupScreen() : base("TVKM")
        {
            Add(new Button("Восстановить сессию", () =>
            {
                if (File.Exists("session.txt"))
                {
                    _app.Auth();
                    hub.Push(_app);
                }
                else
                    hub.Push(new SimpleAlert("Нет сохранённой сессии.", hub));
            }));
            Add(new Button("Вход по логину/паролю", () => hub.Push(new LoginScreen())));
            Add(new Button("GitHub", () => { Process.Start(Settings.BrowserPath, "github.com/Feodor0090"); }));
            Add(new Button("Закрыть", hub.Back));
        }
    }

    private sealed class LoginScreen : ListScreen
    {
        public LoginScreen() : base("Вход в аккаунт VK")
        {
            var login = new TextField("Логин");
            var password = new TextField("Пароль") { ShowChars = false };
            Add(login);
            Add(password);
            Add(new Button("Далее", () =>
            {
                try
                {
                    _app.Auth(login.Text, password.Text);
                    hub.BackThenPush(_app);
                }
                catch
                {
                    hub.Push(new SimpleAlert("Не удалось войти.", hub));
                }
            }));
        }
    }

    static void Main(string[] args)
    {
        _app = new App(hub);
        hub.Push(new StartupScreen());
        hub.Loop();
    }
}