namespace tvkm.UIEngine.Controls;

public class Label<T> : ILabel, IItem<T> where T : IScreen<T>
{
    public string Text { get; set; }

    public Label(string text)
    {
        Text = text;
    }

    public void Draw(bool selected)
    {
        Console.ForegroundColor = selected ? Settings.SelectionColor : Settings.DefaultColor;
        Console.Write(Text);
        Console.WriteLine();
    }

    public void HandleKey(InputEvent e, ScreenStack<T> stack)
    {
        // Do nothing
    }

    public int Height => 1;
}