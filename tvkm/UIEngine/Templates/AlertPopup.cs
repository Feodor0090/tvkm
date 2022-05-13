using tvkm.UIEngine.Controls;

namespace tvkm.UIEngine.Templates;

public class AlertPopup : ListScreen
{
    public AlertPopup(string text, ScreenStack stack) : base(text)
    {
        Add(new Button("Закрыть", stack.Back));
    }
}