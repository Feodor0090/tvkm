using System.Diagnostics;
using tvkm.UIEngine;
using tvkm.UIEngine.Controls;
using tvkm.UIEngine.Templates;

namespace tvkm;

public static class Program
{
    private static readonly ScreenHub hub = new ScreenHub(new DefaultControlsScheme());
    public static App App;

    private sealed class StartupScreen : ListScreen
    {
        public StartupScreen(ScreenStack stack) : base("TVKM")
        {
            Add(new Button("Восстановить сессию", () =>
            {
                if (File.Exists("session.txt"))
                {
                    App.RestoreFromFile();
                    stack.Push(App);
                }
                else
                    stack.Push(new AlertPopup("Нет сохранённой сессии.", stack));
            }));
            Add(new Button("Вход по логину/паролю", () => stack.Push(new LoginScreen(stack))));
            Add(new Button("GitHub", () => { Settings.TryOpenBrowser( "github.com/Feodor0090", stack); }));
            Add(new Button("Закрыть", stack.Back));
        }
    }

    private sealed class LoginScreen : ListScreen
    {
        public LoginScreen(ScreenStack stack) : base("Вход в аккаунт VK")
        {
            var login = new TextField("Логин");
            var password = new TextField("Пароль") { ShowChars = false };
            Add(login);
            Add(password);
            Add(new Button("Далее", () =>
            {
                try
                {
                    var error = App.AuthByPassword(login.Text, password.Text);
                    if (error != null)
                    {
                        stack.Push(new AlertPopup(error, stack));
                        return;
                    }

                    stack.BackThenPush(App);
                }
                catch (OperationCanceledException)
                {
                    // do nothing
                }
                catch
                {
                    stack.Push(new AlertPopup("Не удалось войти.", stack));
                }
            }));
        }
    }

    static void Main(string[] args)
    {
        App = new App(hub.CurrentTab);
        ConfigManager.ReadSettings();
        hub.CurrentTab.Push(new StartupScreen(hub.CurrentTab));
        hub.Loop();
    }
}