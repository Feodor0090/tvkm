using tvkm.UIEngine;
using tvkm.UIEngine.Controls;
using tvkm.UIEngine.Templates;

namespace tvkm;

internal sealed class StartupScreen : ListScreen
{
    private App _app;
    public StartupScreen(ScreenStack stack, App app) : base("TVKM")
    {
        _app = app;
        Add(new Button("Восстановить сессию", () =>
        {
            if (File.Exists("session.txt"))
            {
                _app.RestoreFromFile();
                stack.Push(_app);
            }
            else
                stack.Push(new AlertPopup("Нет сохранённой сессии.", stack));
        }));
        Add(new Button("Вход по логину/паролю", () => stack.Push(new LoginScreen(stack, _app))));
        Add(new Button("GitHub", () => { Settings.TryOpenBrowser( "github.com/Feodor0090", stack); }));
        Add(new Button("Закрыть", stack.Back));
    }
}