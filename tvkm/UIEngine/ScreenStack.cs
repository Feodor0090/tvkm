using tvkm.UIEngine.Templates;

namespace tvkm.UIEngine;

public class ScreenStack
{
    private readonly ScreenHub _hub;

    public ScreenStack(ScreenHub hub)
    {
        _hub = hub;
    }

    private readonly Stack<IScreen> _screens = new();

    public bool Empty => _screens.Count == 0;

    public Object PartialDrawLock => _hub;

    public App? App => _screens.OfType<App>().FirstOrDefault();

    public void Push(IScreen s)
    {
        if (_screens.Count > 0)
            _screens.Peek().OnPause();
        _screens.Push(s);
        s.OnEnter(this);
        _hub.Redraw();
    }

    public void Back()
    {
        _screens.Pop().OnLeave();
        if (_screens.TryPeek(out var s))
            s.OnResume();
    }

    public void BackThenPush(IScreen s)
    {
        Back();
        Push(s);
    }

    public IScreen? Peek()
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

    public void ClearThenPush(IScreen s)
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
        Push(new AlertPopup(text, this));
    }
}