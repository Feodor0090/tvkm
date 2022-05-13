namespace tvkm.UIEngine.Templates;

using static Console;

public abstract class LongLoadingListScreen : ListScreen
{
    protected LongLoadingListScreen(string title) : base(title)
    {
    }

    private bool _ready;
    private CancellationTokenSource? _loadingToken;

    protected abstract void Load();

    public override void OnEnter(ScreenHub screenHub)
    {
        _loadingToken = new CancellationTokenSource();
        base.OnEnter(screenHub);
        Task.Run(() =>
        {
            var t = Task.Run(Load);
            while (true)
            {
                if (t.IsCompleted)
                {
                    _ready = true;
                    screenHub.Redraw();
                    _loadingToken.Dispose();
                    _loadingToken = null;
                    if (t.IsFaulted)
                    {
                        Title = Title + " [ошибка]";
                    }

                    return;
                }

                if (_loadingToken.IsCancellationRequested)
                {
                    _loadingToken.Dispose();
                    _loadingToken = null;
                    return;
                }

                Thread.Sleep(100);
                screenHub.Redraw();
            }
        }, _loadingToken.Token);
    }

    /// <inheritdoc />
    public override void OnLeave()
    {
        base.OnLeave();
        _loadingToken?.Cancel();
    }

    protected override void DrawContent()
    {
        if (_ready)
            base.DrawContent();
    }

    protected override void DrawOverlay()
    {
        if (_ready)
        {
            base.DrawOverlay();
            return;
        }
        
        var w = BufferWidth;
        var s = (int)((DateTime.Now.Ticks / 1000000) % 24);
        
        SetCursorPosition(w / 2 - 5, 1);
        for (var i = 0; i < 10; i++)
        {
            Write((i >= (s - 6) && i <= s) ? '*' : ' ');
        }

        SetCursorPosition(w / 2 - 7, 0);
        Write((char)0x2566);
        SetCursorPosition(w / 2 + 6, 0);
        Write((char)0x2566);
        SetCursorPosition(w / 2 - 7, 1);
        Write((char)0x2551);
        SetCursorPosition(w / 2 + 6, 1);
        Write((char)0x2551);
        SetCursorPosition(w / 2 - 7, 2);
        Write((char)0x255A);
        for (var i = 0; i < 12; i++)
        {
            Write((char)0x2550);
        }

        Write((char)0x255D);
    }
}