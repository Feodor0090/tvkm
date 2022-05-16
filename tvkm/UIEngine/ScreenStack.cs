using tvkm.UIEngine.Templates;

namespace tvkm.UIEngine;

/// <summary>
/// TVKM screen stack is an object that handles a list of screens that user can navigate through.
/// </summary>
/// <typeparam name="T">"Main" screen of your application.</typeparam>
public sealed class ScreenStack<T> where T : IScreen<T>
{
    /// <summary>
    /// Hub instance.
    /// </summary>
    private readonly ScreenHub<T> _hub;

    /// <summary>
    /// Creates a stack.
    /// </summary>
    /// <param name="hub">Hub where it will work.</param>
    public ScreenStack(ScreenHub<T> hub)
    {
        _hub = hub;
    }

    private readonly Stack<IScreen<T>> _screens = new();

    public bool Empty => _screens.Count == 0;

    /// <summary>
    /// Lock on this object if you want to draw something in a thread outside event handlers.
    /// </summary>
    public object PartialDrawLock => _hub;

    public T MainScreen => _screens.OfType<T>().Single();

    /// <summary>
    /// Opens a new screen in this stack, suspending previous.
    /// </summary>
    /// <param name="s">Screen to open.</param>
    public void Push(IScreen<T> s)
    {
        if (_screens.Count > 0)
            _screens.Peek().OnPause();
        _screens.Push(s);
        s.OnEnter(this);
        _hub.Redraw();
    }

    /// <summary>
    /// Closes active screen and resumes a previous one.
    /// </summary>
    public void Back()
    {
        _screens.Pop().OnLeave();
        if (_screens.TryPeek(out var s))
            s.OnResume();
    }

    public void BackThenPush(IScreen<T> s)
    {
        Back();
        Push(s);
    }

    public IScreen<T>? Peek()
    {
        if (_screens.Count == 0) return null;
        return _screens.Peek();
    }

    public void CancelRedraw()
    {
        _hub.CancelRedraw();
    }

    public void Redraw()
    {
        _hub.Redraw();
    }

    public void ClearThenPush(IScreen<T> s)
    {
        while (_screens.Count > 0)
            Back();
        Push(s);
    }

    /// <summary>
    /// Shortcut for pushing an alert popup with text.
    /// </summary>
    /// <param name="text">Text to show to user.</param>
    public void Alert(string text)
    {
        Push(new AlertPopup<T>(text, this));
    }
}