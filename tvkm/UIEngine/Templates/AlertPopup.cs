using tvkm.UIEngine.Controls;

namespace tvkm.UIEngine.Templates;

/// <summary>
/// List screen with title and single "close" button.
/// </summary>
/// <typeparam name="T">"Main" screen of your application.</typeparam>
public sealed class AlertPopup<T> : ListScreen<T> where T : IScreen<T>
{
    public AlertPopup(string text, ScreenStack<T> stack) : base(text)
    {
        Add(new Button<T>("Закрыть", stack.Back));
    }
}