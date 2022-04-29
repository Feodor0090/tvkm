namespace tvkm.UIEngine;

public class SimpleAlert : ListScreen
{
    public SimpleAlert(string text, ScreenHub sh) : base(text)
    {
        Add(new Button("Закрыть", sh.Back));
    }
}