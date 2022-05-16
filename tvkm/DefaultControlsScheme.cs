using tvkm.UIEngine;

namespace tvkm;

public class DefaultControlsScheme : IControlsSchemeProvider<App>
{
    public InputAction ToAction(ConsoleKeyInfo key, IItem<App>? currentItem)
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
            case ConsoleKey.Tab:
                return key.Modifiers.HasFlag(ConsoleModifiers.Shift) ? InputAction.MoveUp : InputAction.MoveDown;
            default:
                if (char.IsControl(key.KeyChar) || key.Modifiers.HasFlag(ConsoleModifiers.Control)) //TODO rewrite
                    return InputAction.SpecialKey;
                return InputAction.TextType;
        }
    }
}