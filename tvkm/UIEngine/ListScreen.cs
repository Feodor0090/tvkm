using static System.Console;

namespace tvkm.UIEngine;

public class ListScreen : List<IItem>, IScreen
{
    public ListScreen(string title)
    {
        Title = title;
    }

    private int _selectedItem;

    protected string Title { get; set; }

    public void Draw()
    {
        DrawContent();
        ForegroundColor = Settings.DefaultColor;
        SetCursorPosition(0, 0);
        Write((char)0x2554);
        Write((char)0x2550);
        Write(Title);
        for (int i = 2 + Title.Length; i < BufferWidth - 1; i++)
            Write((char)0x2550);
        Write((char)0x2557);

        for (int i = 1; i <= BufferHeight - 3; i++)
        {
            SetCursorPosition(0, i);
            Write((char)0x2551);
            SetCursorPosition(BufferWidth - 1, i);
            Write((char)0x2551);
        }

        SetCursorPosition(0, BufferHeight - 2);
        Write((char)0x255A);
        for (int i = 0; i < BufferWidth - 2; i++)
        {
            Write((char)0x2550);
        }

        Write((char)0x255D);
        Write("ArrU/ArrD - навиг., enter - ок, esc - назад, ^C - вых. ");
        DrawOverlay();
    }

    protected virtual void DrawContent()
    {
        var y = 1;
        var h = ContentH;
        var tih = TotalItemsH;

        if (h < tih)
        {
            var cy = SelectedItemGlobalY;
            if (cy > h >> 1)
                y = (h >> 1) - cy;
            if (cy > tih - h >> 1)
                y = -(tih - h);
        }

        for (var i = 0; i < Count; i++)
        {
            var item = this[i];
            if (y > 0)
            {
                SetCursorPosition(0, y);
                item.Draw(_selectedItem == i);
            }

            y += item.Height;
            if (y > h) return;
        }
    }

    protected virtual void DrawOverlay()
    {
        if (ContentH < TotalItemsH)
        {
            float scrollProgress = (float)SelectedItemGlobalY / TotalItemsH;
            int scrollCursorY = (int)(scrollProgress * ContentH + 1);
            SetCursorPosition(BufferWidth - 1, scrollCursorY);
            Write((char)0x2588);
        }
    }

    public virtual void HandleKey(ScreenHub sh, InputEvent e)
    {
        switch (e.Action)
        {
            case InputAction.MoveDown:
            {
                _selectedItem++;
                if (_selectedItem >= Count) _selectedItem = 0;
                return;
            }
            case InputAction.MoveUp:
            {
                _selectedItem--;
                if (_selectedItem < 0) _selectedItem = Count - 1;
                return;
            }
            case InputAction.Return:
            {
                sh.Back();
                return;
            }
            default:
                this[_selectedItem].HandleKey(e);
                return;
        }
    }

    public int TotalItemsH => this.Sum(x => x.Height);
    protected static int ContentH => BufferHeight - 3;
    public int SelectedItemGlobalY => this.Take(_selectedItem).Sum(x => x.Height);

    public virtual void OnEnter(ScreenHub screenHub)
    {
    }

    public virtual void OnPause()
    {
    }

    public virtual void OnResume()
    {
    }

    public virtual void OnLeave()
    {
    }
}