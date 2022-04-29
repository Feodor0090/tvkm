using tvkm.UIEngine;

namespace tvkm;

public class DefaultControlsScheme : IControlsSchemeProvider
{
    public InputAction ToAction(ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.UpArrow:
                return InputAction.MoveUp;
            case ConsoleKey.DownArrow:
                return InputAction.MoveDown;
            case ConsoleKey.LeftArrow:
                return InputAction.MoveLeft;
            case ConsoleKey.RightArrow:
                return InputAction.MoveRight;
            case ConsoleKey.Enter:
                return InputAction.Activate;
            case ConsoleKey.Escape:
                return InputAction.Return;
            default:
                if (char.IsControl(key.KeyChar) || key.Modifiers.HasFlag(ConsoleModifiers.Control)) //TODO rewrite
                    return InputAction.SpecialKey;
                return InputAction.TextType;
        }
    }
}