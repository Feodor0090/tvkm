using tvkm.UIEngine;
using tvkm.UIEngine.Controls;
using tvkm.UIEngine.Templates;

namespace tvkm;

internal sealed class StartupScreen : ListScreen<App>
{
    public StartupScreen(ScreenStack<App> stack, App app) : base("TVKM")
    {
        Add(new Button<App>("Восстановить сессию", () =>
        {
            if (File.Exists("session.txt"))
            {
                app.RestoreFromFile();
                stack.Push(app);
            }
            else
                stack.Push(new AlertPopup<App>("Нет сохранённой сессии.", stack));
        }));
        Add(new Button<App>("Вход по логину/паролю", () => stack.Push(new LoginScreen(stack, app))));
        Add(new Button<App>("Настройки", () => stack.Push(new ConfigView())));
        Add(new Button<App>("GitHub", () => { ExternalUtils.TryOpenBrowser("github.com/Feodor0090/tvkm", stack); }));
        Add(new Button<App>("Закрыть", stack.Back));
    }
}