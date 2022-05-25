using System.Collections.Generic;
using static System.Console;

namespace tvkm.UIEngine.Templates;

/// <summary>
/// Common screen type. Has a border, header and list of items to draw. Can be navigated.
/// </summary>
/// <typeparam name="T">"Main" screen of your application.</typeparam>
public class ListScreen<T> : List<IItem<T>>, IScreen<T> where T : IScreen<T>
{
    /// <summary>
    /// Creates a new empty screen. Use <see cref="List{T}.Add"/> to push content in it.
    /// </summary>
    /// <param name="title">Text to display in header.</param>
    public ListScreen(string title)
    {
        Title = title;
    }

    /// <summary>
    /// Creates a new screen with content.
    /// </summary>
    /// <param name="title">Text to display in header.</param>
    /// <param name="items">Content of the screen.</param>
    public ListScreen(string title, IEnumerable<IItem<T>> items)
    {
        Title = title;
        AddRange(items);
    }

    private int _selectedItem;

    /// <summary>
    /// Text, shown in header.
    /// </summary>
    protected string Title { get; set; }

    /// <summary>
    /// Set this field as (X,Y) to place cursor somewhere after drawing is finished. Use <see cref="CurrentRenderYPos"/> to learn your Y position.
    /// </summary>
    public (int, int)? CursorPosition;

    /// <summary>
    /// Y on screen, where currently rendering item is placed.
    /// </summary>
    public int CurrentRenderYPos { get; private set; }

    /// <summary>
    /// Draws items, title, border, hint and overlay.
    /// </summary>
    public void Draw()
    {
        DrawContent();
        ForegroundColor = Settings.DefaultColor;
        SetCursorPosition(0, 0);
        Write((char) 0x2554);
        Write((char) 0x2550);
        Write(Title);
        for (int i = 2 + Title.Length; i < BufferWidth - 1; i++)
            Write((char) 0x2550);
        Write((char) 0x2557);

        for (int i = 1; i <= BufferHeight - 3; i++)
        {
            SetCursorPosition(0, i);
            Write((char) 0x2551);
            SetCursorPosition(BufferWidth - 1, i);
            Write((char) 0x2551);
        }

        SetCursorPosition(0, BufferHeight - 2);
        Write((char) 0x255A);
        for (int i = 0; i < BufferWidth - 2; i++)
        {
            Write((char) 0x2550);
        }

        Write((char) 0x255D);
        Write("ArrU/ArrD - навиг., enter - ок, esc - назад, ^C - вых. ");
        DrawOverlay();
        if (CursorPosition != null)
        {
            SetCursorPosition(CursorPosition.Value.Item1, CursorPosition.Value.Item2);
            CursorPosition = null;
        }
    }

    /// <summary>
    /// Draws items. Override to draw something else.
    /// </summary>
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
                CurrentRenderYPos = y;
                SetCursorPosition(0, y);
                item.Draw(_selectedItem == i, this);
            }

            y += item.Height;
            if (y > h) return;
        }
    }

    /// <summary>
    /// Draws a scrollbar above screen's border. Override to draw something else.
    /// </summary>
    protected virtual void DrawOverlay()
    {
        if (ContentH < TotalItemsH)
        {
            float scrollProgress = (float) SelectedItemGlobalY / TotalItemsH;
            int scrollCursorY = (int) (scrollProgress * ContentH + 1);
            SetCursorPosition(BufferWidth - 1, scrollCursorY);
            Write((char) 0x2588);
        }
    }

    public virtual void HandleKey(InputEvent e, ScreenStack<T> stack)
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
                stack.Back();
                return;
            }
            default:
                this[_selectedItem].HandleKey(e, stack);
                return;
        }
    }

    /// <summary>
    /// Total number of lines that all items consume.
    /// </summary>
    public int TotalItemsH => this.Sum(x => x.Height);

    protected static int ContentH => BufferHeight - 3;
    public int SelectedItemGlobalY => this.Take(_selectedItem).Sum(x => x.Height);

    public virtual void OnEnter(ScreenStack<T> stack)
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

    public IItem<T>? Current => Count == 0 ? null : this[_selectedItem];
}