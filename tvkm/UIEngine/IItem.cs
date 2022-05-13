namespace tvkm.UIEngine;

public interface IItem
{
    public void Draw(bool selected);
    public void HandleKey(InputEvent e, ScreenStack stack);
    public int Height { get; }
    
}