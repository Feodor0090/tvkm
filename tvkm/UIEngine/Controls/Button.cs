namespace tvkm.UIEngine.Controls;

public class Button<T> : IItem<T> where T : IScreen<T>
{
    public Button(string text, Action onPress)
    {
        action = onPress;
        top = "  " + new string(Enumerable.Range(0, text.Length).Select(x => (char) 0x2500).Prepend((char) 0x250C)
            .Append((char) 0x2510)
            .ToArray());
        main = "  " + ((char) 0x2502) + text + ((char) 0x2502);
        bottom = "  " + new string(Enumerable.Range(0, text.Length).Select(x => (char) 0x2500).Prepend((char) 0x2514)
            .Append((char) 0x2518)
            .ToArray());
    }

    //TODO remove this
    private Action action;
    private string top, main, bottom;


    public void Draw(bool selected)
    {
        Console.ForegroundColor = selected ? ConsoleColor.Yellow : ConsoleColor.White;
        Console.WriteLine(top);
        Console.WriteLine(main);
        Console.WriteLine(bottom);
    }

    public void HandleKey(InputEvent e, ScreenStack<T> stack)
    {
        if (e.Action == InputAction.Activate) action();
    }

    public int Height => 3;
}