namespace tvkm.UIEngine.Templates;

using static Console;

/// <summary>
/// List screen that supports executing a long operation to prepare all content.
/// </summary>
/// <typeparam name="T">"Main" screen of your application.</typeparam>
public abstract class LongLoadingListScreen<T> : ListScreen<T> where T : IScreen<T>
{
    protected LongLoadingListScreen(string title) : base(title)
    {
    }

    private bool _ready;
    private CancellationTokenSource? _loadingToken;

    /// <summary>
    /// Loads screen's content.
    /// </summary>
    /// <param name="stack">Screen stack in which this screen was opened.</param>
    protected abstract void Load(ScreenStack<T> stack);

    public override void OnEnter(ScreenStack<T> stack)
    {
        _loadingToken = new CancellationTokenSource();
        base.OnEnter(stack);
        Task.Run(() =>
        {
            var t = Task.Run(() => { Load(stack); });
            while (true)
            {
                if (t.IsCompleted)
                {
                    _ready = true;
                    stack.Redraw();
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
                stack.Redraw();
            }
        }, _loadingToken.Token);
    }

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
        var s = (int) ((DateTime.Now.Ticks / 1000000) % 24);

        SetCursorPosition(w / 2 - 5, 1);
        for (var i = 0; i < 10; i++)
        {
            Write((i >= (s - 6) && i <= s) ? '*' : ' ');
        }

        SetCursorPosition(w / 2 - 7, 0);
        Write((char) 0x2566);
        SetCursorPosition(w / 2 + 6, 0);
        Write((char) 0x2566);
        SetCursorPosition(w / 2 - 7, 1);
        Write((char) 0x2551);
        SetCursorPosition(w / 2 + 6, 1);
        Write((char) 0x2551);
        SetCursorPosition(w / 2 - 7, 2);
        Write((char) 0x255A);
        for (var i = 0; i < 12; i++)
        {
            Write((char) 0x2550);
        }

        Write((char) 0x255D);
    }
}