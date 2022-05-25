using tvkm.UIEngine.Templates;

namespace tvkm.UIEngine.Controls;

public class Label<T> : ILabel, IItem<T> where T : IScreen<T>
{
    public string Text { get; set; }

    public Label(string text)
    {
        Text = text;
    }

    public void Draw(bool selected, ListScreen<T> screen)
    {
        Console.ForegroundColor = selected ? Settings.SelectionColor : Settings.DefaultColor;
        Console.Write("  ");
        Console.Write(Text);
        Console.WriteLine();
    }

    public virtual void HandleKey(InputEvent e, ScreenStack<T> stack)
    {
        // Do nothing
    }

    public int Height => 1;
}