namespace tvkm.UIEngine;

public interface IControlsSchemeProvider
{
    InputAction ToAction(ConsoleKeyInfo key, object? currentItem);
}