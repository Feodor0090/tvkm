namespace tvkm.UIEngine;

/// <summary>
/// TVKM screen is an object that can draw itself onto console and handle keyboard input.
/// </summary>
/// <typeparam name="T">"Main" screen of your application.</typeparam>
public interface IScreen<T> where T : IScreen<T>
{
    /// <summary>
    /// Fully draws this screen onto console.
    /// </summary>
    public void Draw();

    /// <summary>
    /// Reacts on keyboard input.
    /// </summary>
    /// <param name="e">Input event.</param>
    /// <param name="stack">Stack, where this screen is placed now.</param>
    public void HandleKey(InputEvent e, ScreenStack<T> stack);

    /// <summary>
    /// Fires when this screen is pushed.
    /// </summary>
    /// <param name="stack"></param>
    public void OnEnter(ScreenStack<T> stack);

    /// <summary>
    /// Fires when a screen above this is pushed.
    /// </summary>
    public void OnPause();

    /// <summary>
    /// Fires when user returns to this screen.
    /// </summary>
    public void OnResume();

    /// <summary>
    /// Fires when this screen is closed.
    /// </summary>
    public void OnLeave();

    /// <summary>
    /// Currently selected item, null if there are no one.
    /// </summary>
    public IItem<T>? Current { get; }
}