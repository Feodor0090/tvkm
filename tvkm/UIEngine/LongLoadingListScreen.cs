namespace tvkm.UIEngine;

using static Console;

public abstract class LongLoadingListScreen : ListScreen
{
    public LongLoadingListScreen(string title) : base(title)
    {
    }

    private bool ready = false;
    private CancellationTokenSource? loadingToken;

    public abstract void Load();

    public override void OnEnter(ScreenHub screenHub)
    {
        loadingToken = new CancellationTokenSource();
        base.OnEnter(screenHub);
        Task.Run(() =>
        {
            var t = Task.Run(Load);
            while (true)
            {
                if (t.IsCompleted)
                {
                    ready = true;
                    screenHub.Redraw();
                    loadingToken.Dispose();
                    loadingToken = null;
                    if (t.IsFaulted)
                    {
                        Title = Title + " [ошибка]";
                    }

                    return;
                }

                if (loadingToken.IsCancellationRequested)
                {
                    loadingToken.Dispose();
                    loadingToken = null;
                    return;
                }

                Thread.Sleep(100);
                screenHub.Redraw();
            }
        }, loadingToken.Token);
    }

    /// <inheritdoc />
    public override void OnLeave()
    {
        base.OnLeave();
        loadingToken?.Cancel();
    }

    protected override void DrawContent()
    {
        if (ready)
            base.DrawContent();
    }

    protected override void DrawOverlay()
    {
        if (ready)
        {
            base.DrawOverlay();
            return;
        }
        int w = BufferWidth;
        long t = DateTime.Now.Ticks / 1000000;
        int s = (int)(t % 24);

        int x = s - 6;
        SetCursorPosition(w / 2 - 5, 1);
        for (int i = 0; i < 10; i++)
        {
            Write((i >= x && i <= s) ? '*' : ' ');
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
        for (int i = 0; i < 12; i++)
        {
            Write((char)0x2550);
        }

        Write((char)0x255D);
    }
}