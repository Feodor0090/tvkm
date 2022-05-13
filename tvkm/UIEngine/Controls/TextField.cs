namespace tvkm.UIEngine.Controls;

public class TextField : IItem
{
    public TextField(string label, string defaultText = "")
    {
        Label = label;
        _text = defaultText.ToList();
    }

    public bool ShowChars = true;
    public string Label;
    private List<char> _text;

    public string Text
    {
        get => new(_text.ToArray());
        set => _text = value.ToList();
    }

    public void Draw(bool selected)
    {
        Console.ForegroundColor = selected ? Settings.SelectionColor : Settings.DefaultColor;
        var w = Console.BufferWidth;
        Console.Write("  ");
        Console.Write((char)0x2554);
        Console.Write((char)0x2550);
        for (var i = 0; i < w - 7; i++)
        {
            Console.Write(i < Label.Length ? Label[i] : (char)0x2550);
        }

        Console.Write((char)0x2557);
        Console.Write("    ");
        Console.Write((char)0x2551);

        for (var i = 0; i < w - 4 && i < _text.Count; i++)
        {
            Console.Write(ShowChars ? _text[i] : '*');
        }

        Console.CursorLeft = Console.BufferWidth - 3;
        Console.Write((char)0x2551);
        Console.Write("    ");
        Console.Write((char)0x255A);
        for (var i = 0; i < w - 6; i++)
        {
            Console.Write((char)0x2550);
        }

        Console.Write((char)0x255D);
    }

    public void HandleKey(InputEvent e)
    {
        if (e.Key == ConsoleKey.Backspace)
            if (_text.Count > 0)
                _text.RemoveAt(_text.Count - 1);
        
        if (e.Action == InputAction.TextType)
            _text.Add(e.Char);
    }


    public int Height => 3;
}