namespace tvkm.UIEngine;

public interface IScreen
{
    public void Draw();
    public void HandleKey(InputEvent e, ScreenStack stack);

    public void OnEnter(ScreenStack stack);

    public void OnPause();

    public void OnResume();

    public void OnLeave();

    public IItem? Current { get; }
}