using tvkm.UIEngine.Templates;

namespace tvkm.UIEngine;

public interface IItem<T> where T : IScreen<T>
{
    public void Draw(bool selected, ListScreen<T> screen);
    public void HandleKey(InputEvent e, ScreenStack<T> stack);
    public int Height { get; }
}