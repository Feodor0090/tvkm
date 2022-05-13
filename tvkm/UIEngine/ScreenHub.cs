namespace tvkm.UIEngine;

public sealed class ScreenHub
{
    public ScreenHub(IControlsSchemeProvider scheme)
    {
        _render = Thread.CurrentThread;
        _input = new Thread(HandleInput);
        _scheme = scheme;
    }

    private readonly IControlsSchemeProvider _scheme;
    private readonly Stack<IScreen> _screens = new();
    private readonly Thread _render;
    private readonly Thread _input;
    private bool _redraw = true;

    private int _lastW;
    private int _lastH;

    public App? MainScreen => _screens.FirstOrDefault(x => x is App) as App;

    public void CancelRedraw()
    {
        _redraw = false;
    }

    public void Loop()
    {
        if (_render != Thread.CurrentThread) throw new InvalidOperationException();
        _input.Start();
        while (_screens.Count > 0)
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
        while (_screens.Count > 0)
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
            if (!_screens.TryPeek(out var s))
                return;

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

            s.HandleKey(this,
                new InputEvent(_scheme.ToAction(k, _screens.Peek().Current), k.Modifiers, k.Key, k.KeyChar));
            if (_redraw) Redraw();
            _redraw = true;
        }
    }

    public void Redraw()
    {
        _render.Interrupt();
    }


    public void Push(IScreen s)
    {
        if (_screens.Count > 0)
            _screens.Peek().OnPause();
        _screens.Push(s);
        s.OnEnter(this);
        Redraw();
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
}