using tvkm.UIEngine;
using tvkm.UIEngine.Controls;
using tvkm.UIEngine.Templates;

namespace tvkm;

internal sealed class StartupScreen : ListScreen
{
    public StartupScreen(ScreenStack stack) : base("TVKM")
    {
        Add(new Button("Восстановить сессию", () =>
        {
            if (File.Exists("session.txt"))
            {
                Program.App.RestoreFromFile();
                stack.Push(Program.App);
            }
            else
                stack.Push(new AlertPopup("Нет сохранённой сессии.", stack));
        }));
        Add(new Button("Вход по логину/паролю", () => stack.Push(new LoginScreen(stack))));
        Add(new Button("GitHub", () => { Settings.TryOpenBrowser( "github.com/Feodor0090", stack); }));
        Add(new Button("Закрыть", stack.Back));
    }
}