using tvkm.UIEngine;
using tvkm.UIEngine.Controls;
using tvkm.UIEngine.Templates;

namespace tvkm;

internal sealed class StartupScreen : ListScreen<App>
{
    private App _app;

    public StartupScreen(ScreenStack<App> stack, App app) : base("TVKM")
    {
        _app = app;
        Add(new Button<App>("Восстановить сессию", () =>
        {
            if (File.Exists("session.txt"))
            {
                _app.RestoreFromFile();
                stack.Push(_app);
            }
            else
                stack.Push(new AlertPopup<App>("Нет сохранённой сессии.", stack));
        }));
        Add(new Button<App>("Вход по логину/паролю", () => stack.Push(new LoginScreen(stack, _app))));
        Add(new Button<App>("GitHub", () => { ExternalUtils.TryOpenBrowser("github.com/Feodor0090/tvkm", stack); }));
        Add(new Button<App>("Закрыть", stack.Back));
    }
}