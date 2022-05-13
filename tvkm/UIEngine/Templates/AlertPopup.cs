using tvkm.UIEngine.Controls;

namespace tvkm.UIEngine.Templates;

public class AlertPopup : ListScreen
{
    public AlertPopup(string text, ScreenHub sh) : base(text)
    {
        Add(new Button("Закрыть", sh.Back));
    }
}