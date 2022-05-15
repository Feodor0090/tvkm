using tvkm.UIEngine.Controls;

namespace tvkm.UIEngine.Templates;

/// <summary>
/// List screen with title and single "close" button.
/// </summary>
public sealed class AlertPopup : ListScreen
{
    public AlertPopup(string text, ScreenStack stack) : base(text)
    {
        Add(new Button("Закрыть", stack.Back));
    }
}