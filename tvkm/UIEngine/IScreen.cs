namespace tvkm.UIEngine;

public interface IScreen
{
    public void Draw();
    public void HandleKey(ScreenHub sh, InputEvent e);

    public void OnEnter(ScreenHub screenHub);

    public void OnPause();

    public void OnResume();

    public void OnLeave();
}