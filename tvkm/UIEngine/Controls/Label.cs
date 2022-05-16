namespace tvkm.UIEngine.Controls;

public class Label<T> : ILabel, IItem<T> where T : IScreen<T>
{
    private string? _leftBorder;
    public string Text { get; set; }

    public string? LeftBorder
    {
        get => _leftBorder;
        set
        {
            if (value != null && value.Length != 2)
                throw new ArgumentException();
            _leftBorder = value;
        }
    }

    public string? RightBorder { get; set; }

    public Label(string text)
    {
        Text = text;
    }

    public void Draw(bool selected)
    {
        Console.ForegroundColor = selected ? Settings.SelectionColor : Settings.DefaultColor;
        if (LeftBorder == null)
            Console.Write("  ");
        else
            Console.Write(LeftBorder);
        
        Console.Write(Text);
        if (RightBorder != null)
        {
            //TODO implement
        }
        Console.WriteLine();
    }

    public void HandleKey(InputEvent e, ScreenStack<T> stack)
    {
        // Do nothing
    }

    public int Height => 1;
}