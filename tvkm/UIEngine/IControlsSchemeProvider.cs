namespace tvkm.UIEngine;

public interface IControlsSchemeProvider<T> where T : IScreen<T>
{
    InputAction ToAction(ConsoleKeyInfo key, IItem<T>? currentItem);
}