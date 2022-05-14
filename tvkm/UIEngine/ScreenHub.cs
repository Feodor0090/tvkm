namespace tvkm.UIEngine;

public sealed class ScreenHub
{
    public ScreenHub(IControlsSchemeProvider scheme)
    {
        _render = Thread.CurrentThread;
        _input = new Thread(HandleInput);
        Scheme = scheme;
        _screens = new ScreenStack(this);
    }

    public IControlsSchemeProvider Scheme { get; set; }
    private readonly ScreenStack _screens;
    private readonly Thread _render;
    private readonly Thread _input;
    private bool _redraw = true;

    private int _lastW;
    private int _lastH;

    public void CancelRedraw()
    {
        _redraw = false;
    }

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

            s.HandleKey(
                new InputEvent(Scheme.ToAction(k, s.Current), k.Modifiers, k.Key, k.KeyChar), _screens);
            if (_redraw) Redraw();
            _redraw = true;
        }
    }

    public void Redraw()
    {
        _render.Interrupt();
    }

    public ScreenStack this[int index]
    {
        get => _screens;
    }
}