namespace tvkm.UIEngine;

public interface IItem
{
    public void Draw(bool selected);
    public void HandleKey(InputEvent e, ScreenHub hub);
    public int Height { get; }
    
}