namespace tvkm.UIEngine;

public interface IScreen
{
    public void Draw();
    public void HandleKey(ScreenHub sh, InputEvent e);

    public void OnEnter(ScreenStack stack);

    public void OnPause();

    public void OnResume();

    public void OnLeave();

    public IItem? Current { get; }
}