namespace tvkm.UIEngine;

public interface IItem<T> where T : IScreen<T>
{
    public void Draw(bool selected);
    public void HandleKey(InputEvent e, ScreenStack<T> stack);
    public int Height { get; }
}