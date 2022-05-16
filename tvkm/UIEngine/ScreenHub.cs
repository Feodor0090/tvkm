namespace tvkm.UIEngine;

/// <summary>
/// TVKM screen hub is a singleton object that manages screens drawing and event loop.
/// </summary>
/// /// <typeparam name="T">"Main" screen of your application.</typeparam>
public sealed class ScreenHub<T> where T : IScreen<T>
{
    public ScreenHub(IControlsSchemeProvider scheme)
    {
        _render = Thread.CurrentThread;
        _input = new Thread(HandleInput);
        Scheme = scheme;
        _screens = new ScreenStack<T>(this);
    }

    /// <summary>
    /// Currently used input mapper.
    /// </summary>
    public IControlsSchemeProvider Scheme { get; set; }

    //TODO: there should be an array of tabs (each tab is a stack), not a single stack.
    private readonly ScreenStack<T> _screens;
    private readonly Thread _render;
    private readonly Thread _input;
    private bool _redraw = true;

    private int _lastW;
    private int _lastH;


    /// <summary>
    /// Should be called from event handler. Skips pending redraw, allowing handler to partially update screen's content.
    /// </summary>
    public void CancelRedraw()
    {
        _redraw = false;
    }

    /// <summary>
    /// Main program loop.
    /// </summary>
    /// <exception cref="InvalidOperationException">Must be called from the same thread, where was created.</exception>
    public void Loop()
    {
        if (_render != Thread.CurrentThread) throw new InvalidOperationException();
        _input.Start();
        while (!_screens.Empty)
        {
            try
            {
                if (_redraw)
                {
                    var s = _screens.Peek();
                    lock (this)
                    {
                        Console.Clear();
                        s.Draw();
                    }
                }

                _redraw = true;
                Thread.Sleep(Timeout.Infinite);
            }
            catch (ThreadInterruptedException)
            {
            }
        }
    }

    private void HandleInput()
    {
        _lastW = Console.BufferWidth;
        _lastH = Console.BufferHeight;
        while (!_screens.Empty)
        {
            if (!Console.KeyAvailable)
            {
                if (_lastW != Console.BufferWidth || _lastH != Console.BufferHeight)
                {
                    _lastW = Console.BufferWidth;
                    _lastH = Console.BufferHeight;
                    Redraw();
                }

                Thread.Sleep(50);
                continue;
            }

            var k = Console.ReadKey(true);
            var s = _screens.Peek();
            if (s == null) return;

            if (k.Key == ConsoleKey.R && k.Modifiers == ConsoleModifiers.Control)
            {
                Redraw();
                continue;
            }

            if (k.Key == ConsoleKey.F && k.Modifiers == ConsoleModifiers.Control)
            {
                GC.Collect();
                continue;
            }

            _redraw = true; // cancellation is expected to be called inside event handler, not before it.
            s.HandleKey(
                new InputEvent(Scheme.ToAction(k, s.Current), k.Modifiers, k.Key, k.KeyChar), _screens);
            if (_redraw) Redraw();
            _redraw = true;
        }
    }

    /// <summary>
    /// Triggers full redraw.
    /// </summary>
    public void Redraw()
    {
        _render.Interrupt();
    }

    /// <summary>
    /// Gets a tab's stack.
    /// </summary>
    /// <param name="index">Tab index.</param>
    public ScreenStack<T> this[int index]
    {
        //TODO implement tabs
        get => _screens;
    }
}