using tvkm.Api;
using tvkm.UIEngine;
using VkNet;
using VkNet.Model;
using static System.Console;

namespace tvkm.Dialogs;

public class DialogsScreen : DialogsScreenBase, IScreen
{
    public static void Open(DialogItem? d)
    {
        Inst?.OpenDialog(d);
    }


    private int _selectedPeerItem;

    public FocusedSection Focus = FocusedSection.PeersList;

    private readonly List<char> _message = new();
    private int _messageFieldCursorX;

    private ScreenStack? _stack;

    public const int DialTabW = 50;
    private const string DateFormat = " [hh:mm]: ";

    public DialogsScreen(VkApi api) : base(api)
    {
    }


    private static void DrawBlockTitle(int w, string t)
    {
        Write((char)0x250C);
        Write((char)0x2500);
        Write(t);
        for (var i = 2 + t.Length; i < w - 1; i++)
            Write((char)0x2500);
        Write((char)0x2510);
    }

    private static void DrawBlockBottom(int w)
    {
        Write((char)0x2514);
        for (var i = 1; i < w - 1; i++)
            Write((char)0x2500);
        Write((char)0x2518);
    }

    private static void DrawBorder(int x, int y, int w, int h, string title, ConsoleColor clr)
    {
        ForegroundColor = clr;
        SetCursorPosition(x, y);
        DrawBlockTitle(w, title);
        for (var i = 0; i < h - 2; i++)
        {
            SetCursorPosition(x, y + 1 + i);
            Write((char)0x2502);
            SetCursorPosition(x + w - 1, y + 1 + i);
            Write((char)0x2502);
        }

        SetCursorPosition(x, y + h - 1);
        DrawBlockBottom(w);
    }

    private void DrawAllBorders()
    {
        int h = BufferHeight, w = BufferWidth;
        DrawBorder(0, h - 3, w, 3, "Ваше сообщение",
            Focus == FocusedSection.InputField ? ConsoleColor.Yellow : ConsoleColor.White);
        DrawBorder(DialTabW, 0, w - DialTabW, h - 3, ActiveDialogName, ConsoleColor.White);
    }

    private void DrawPeersList()
    {
        var i = 0;
        int h = BufferHeight - 5;
        if (Peers.Count > h)
        {
            if (_selectedPeerItem > h / 2)
                i = _selectedPeerItem - (h / 2);
            if (_selectedPeerItem > Peers.Count - h / 2 - 1)
            {
                i = Peers.Count - h;
            }
        }

        int j = 1;
        for (; i < Peers.Count && j < BufferHeight - 4; i++)
        {
            SetCursorPosition(1, j);
            Peers[i].Draw(Focus == FocusedSection.PeersList && i == _selectedPeerItem);
            j++;
        }

        DrawBorder(0, 0, DialTabW, BufferHeight - 3, "Список диалогов",
            Focus == FocusedSection.PeersList ? ConsoleColor.Yellow : ConsoleColor.White);

        float scrollProgress = (float)_selectedPeerItem / Peers.Count;
        int scrollCursorY = (int)(scrollProgress * h + 1);
        SetCursorPosition(DialTabW - 1, scrollCursorY);
        Write((char)0x2588);
    }

    public void Draw()
    {
        DrawAllBorders();

        DrawPeersList();

        RedrawAllMessages();

        SetCursorPosition(1, BufferHeight - 2);
        PrintInput();

        FixCursorLocation();
    }

    private void RedrawAllMessages()
    {
        if (Msgs == null) return;

        var h = BufferHeight;
        var maxNameL = 0;
        for (var i = 0; i < Msgs.Count; i++)
        {
            maxNameL = Math.Max(maxNameL, Msgs[i].Author.Name.Length);
        }

        maxNameL += DateFormat.Length;
        var msgAvailH = h - 5;
        var contentW = BufferWidth - DialTabW - 2 - maxNameL;
        var sm = CalculateFirstMessageIndex(contentW, msgAvailH);

        // drawing
        var cursorY = 0;
        for (var i = sm + 1; i < Msgs.Count; i++)
        {
            var x = Msgs[i];
            ForegroundColor = x.Author.Id == App.UserId ? ConsoleColor.Green : ConsoleColor.White;
            SetCursorPosition(DialTabW + 1, ++cursorY);
            Write((x.Author.Name + x.Time.ToString(DateFormat)).PadLeft(maxNameL));
            if (x.TextValid)
            {
                var p = 0;
                for (var k = 0; k < x.Text.Length; k++)
                {
                    if (p >= contentW)
                    {
                        p = 0;
                        SetCursorPosition(DialTabW + 1 + maxNameL, ++cursorY);
                    }

                    switch (x.Text[k])
                    {
                        case '\r':
                            break;
                        case '\n':
                            Write(new string(' ', contentW - p));
                            p = int.MaxValue;
                            break;
                        default:
                            Write(x.Text[k]);
                            p++;
                            break;
                    }
                }

                Write(new string(' ', contentW - p));
            }
            else
            {
                cursorY--;
            }

            if (x.HasAtts)
            {
                for (int j = 0; j < x.Atts!.Length; j++)
                {
                    SetCursorPosition(DialTabW + 1 + maxNameL, ++cursorY);
                    Write($"[{x.Atts[j].Caption}]".PadRight(contentW));
                }
            }

            if (x.Reply != null)
            {
                SetCursorPosition(DialTabW + 1 + maxNameL, ++cursorY);
                Write($"[Ответ {x.Reply.Author.Name}]".PadRight(contentW));
            }
        }
    }

    private void FixCursorLocation()
    {
        var h = BufferHeight;
        switch (Focus)
        {
            case FocusedSection.InputField:
                SetCursorPosition(_messageFieldCursorX + 1, h - 2);
                break;
            case FocusedSection.PeersList:
                CursorTop = _selectedPeerItem + 1;
                CursorLeft = 1;
                break;
        }
    }

    private int CalculateFirstMessageIndex(int msgw, int msgAvailH)
    {
        if (Msgs == null) return 0;
        int sm;
        for (sm = Msgs.Count - 1; sm >= 0; sm--)
        {
            var x = Msgs[sm];
            var textH = 0;
            if (x.TextValid)
            {
                var p = 0;
                for (var k = 0; k < x.Text.Length; k++)
                {
                    if (p >= msgw)
                    {
                        textH++;
                        p = 0;
                    }

                    switch (x.Text[k])
                    {
                        case '\r':
                            break;
                        case '\n':
                            p = int.MaxValue;
                            break;
                        default:
                            p++;
                            break;
                    }
                }

                textH++;
            }

            msgAvailH -= textH;
            if (x.Reply != null) msgAvailH--;
            msgAvailH -= x.Atts?.Length ?? 0;
            if (msgAvailH < 0) break;
        }

        return sm;
    }

    private void PrintInput()
    {
        lock (this)
        {
            ForegroundColor = Focus == FocusedSection.InputField ? Settings.SelectionColor : Settings.DefaultColor;
            if (_message.Count <= BufferWidth - 3)
            {
                Write(_message.ToArray());
                for (var i = _message.Count; i < BufferWidth - DialTabW - 3; i++)
                    Write(' ');
            }
            else
            {
                for (var i = _message.Count - (BufferWidth - 3); i < _message.Count; i++)
                {
                    Write(_message[i]);
                }
            }

            Write(' ');
        }
    }

    public void HandleKey(InputEvent e, ScreenStack stack)
    {
        var l = stack.PartialDrawLock;
        void RedrawInput()
        {
            lock (l)
            {
                CursorLeft = 1;
                PrintInput();
                stack.CancelRedraw();
                FixCursorLocation();
            }
        }

        switch (Focus)
        {
            case FocusedSection.PeersList:
                switch (e.Action)
                {
                    case InputAction.Return:
                        stack.Back();
                        return;
                    case InputAction.MoveDown:
                        _selectedPeerItem++;
                        if (_selectedPeerItem >= Peers.Count)
                            _selectedPeerItem = 0;
                        break;
                    case InputAction.MoveUp:
                        _selectedPeerItem--;
                        if (_selectedPeerItem < 0)
                            _selectedPeerItem = Peers.Count - 1;
                        break;
                    case InputAction.Activate:
                        Peers[_selectedPeerItem].HandleKey(e);
                        Focus = FocusedSection.InputField;
                        lock (l)
                        {
                            RedrawAllMessages();
                            DrawAllBorders();
                        }

                        break;
                }


                lock (l)
                {
                    DrawPeersList();
                    stack.CancelRedraw();
                    FixCursorLocation();
                }

                break;
            case FocusedSection.MessagesHistory:
                break;
            case FocusedSection.InputField:
                lock (this)
                {
                    switch (e.Action)
                    {
                        case InputAction.Return:
                            if (_message.Count == 0)
                            {
                                Focus = FocusedSection.PeersList;
                                OpenDialog(null);
                                break;
                            }

                            _message.Clear();
                            _messageFieldCursorX = 0;
                            RedrawInput();

                            break;
                        case InputAction.Activate:
                            if (_message.Count > 0)
                            {
                                if (Send(new string(_message.ToArray())))
                                {
                                    _message.Clear();
                                    _messageFieldCursorX = 0;
                                    RedrawInput();
                                }
                            }

                            break;
                        case InputAction.SpecialKey:
                            if (e.Key == ConsoleKey.Backspace)
                            {
                                if (_message.Count > 0 && _messageFieldCursorX > 0)
                                {
                                    _message.RemoveAt(_messageFieldCursorX - 1);
                                    _messageFieldCursorX--;
                                }

                                RedrawInput();
                            }
                            else if (e.Key == ConsoleKey.Delete)
                            {
                                if (_message.Count > 0 && _messageFieldCursorX < _message.Count)
                                {
                                    _message.RemoveAt(_messageFieldCursorX);
                                }

                                RedrawInput();
                            }

                            break;
                        case InputAction.TextType:
                            _message.Insert(_messageFieldCursorX, e.Char);
                            _messageFieldCursorX++;
                            RedrawInput();
                            break;
                        case InputAction.MoveLeft:
                            if (_messageFieldCursorX > 0)
                                _messageFieldCursorX--;
                            RedrawInput();
                            break;
                        case InputAction.MoveRight:
                            if (_messageFieldCursorX < _message.Count)
                                _messageFieldCursorX++;
                            RedrawInput();
                            break;
                    }
                }

                break;
        }
    }


    private void OnNewMessage(LongpollDaemon.LongpollMessage msg)
    {
        if (msg.TargetId == CurrentDialog && Msgs != null)
        {
            Message m;
            var id = msg.FromId;
            if (msg.Extra.Count > 0)
            {
                m = ToFull(msg);
                id = m.FromId ?? id;
            }
            else
            {
                m = new Message
                {
                    Id = msg.MessageId,
                    Text = msg.Text,
                    Date = DateTime.Now,
                };
            }

            Msgs.Add(new MsgItem(GetUser(id), m));
            if (_stack == null) return;
            lock (_stack.PartialDrawLock)
            {
                RedrawAllMessages();
            }

            ReportRead(msg.TargetId, msg.MessageId);

            FixCursorLocation();
        }
        else
        {
            var p = Peers.FirstOrDefault(x => x.PeerId == msg.TargetId);
            if (p == null) return;
            p.UnreadCount++;
            if (Focus != FocusedSection.PeersList)
            {
                Peers.Remove(p);
                Peers.Insert(0, p);
            }

            if (_stack == null) return;
            lock (_stack.PartialDrawLock)
            {
                DrawPeersList();
            }
        }
    }

    private void OnMessageEdit(LongpollDaemon.LongpollMessageEdit msg)
    {
        //another chat
        if (msg.PeerId != CurrentDialog) return;
        //searching by ID
        MsgItem? m = Msgs?.Where(x => x.Id == msg.MessageId).FirstOrDefault();
        //not loaded message
        if (m == null) return;

        m.Update(ToFull(msg));

        if (_stack == null) return;
        lock (_stack.PartialDrawLock)
        {
            RedrawAllMessages();
        }
    }

    private CancellationTokenSource _typingLabelToken = new();

    private async void OnMessageWrite(LongpollDaemon.LongpollWriteStatus msg)
    {
        if (msg.PeerId != CurrentDialog || _stack == null) return;

        lock (_stack.PartialDrawLock)
        {
            SetCursorPosition(DialTabW, 0);
            DrawBlockTitle(BufferWidth - DialTabW, ActiveDialogName + " (печатает)");
            FixCursorLocation();
        }

        await Task.Delay(6000, _typingLabelToken.Token);
        if (_typingLabelToken.IsCancellationRequested)
        {
            _typingLabelToken.Dispose();
            _typingLabelToken = new CancellationTokenSource();
            return;
        }

        lock (_stack.PartialDrawLock)
        {
            SetCursorPosition(DialTabW, 0);
            DrawBlockTitle(BufferWidth - DialTabW, ActiveDialogName);
            FixCursorLocation();
        }
    }

    public void OnEnter(ScreenStack stack)
    {
        _stack = stack;
        OpenDialog(null);
        FetchPeersList();
        LongpollDaemon.OnNewMessage += OnNewMessage;
        LongpollDaemon.OnMessageEdit += OnMessageEdit;
        LongpollDaemon.OnMessageWrite += OnMessageWrite;
    }

    public void OnPause()
    {
    }

    public void OnResume()
    {
    }

    public void OnLeave()
    {
        LongpollDaemon.OnNewMessage -= OnNewMessage;
        LongpollDaemon.OnMessageEdit -= OnMessageEdit;
        LongpollDaemon.OnMessageWrite -= OnMessageWrite;
        Inst = null;
    }

    public IItem? Current => null;

    public enum FocusedSection : byte
    {
        PeersList,
        MessagesHistory,
        InputField
    }
}