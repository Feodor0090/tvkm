namespace tvkm.UIEngine;

public readonly struct InputEvent
{
    public InputEvent(InputAction action, ConsoleModifiers mods, ConsoleKey key, char ch)
    {
        Action = action;
        Mods = mods;
        Key = key;
        Char = ch;
    }

    public readonly InputAction Action;
    public readonly ConsoleModifiers Mods;
    public readonly ConsoleKey Key;
    public readonly char Char;
}