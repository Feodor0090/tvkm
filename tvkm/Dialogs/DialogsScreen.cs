using tvkm.UIEngine;
using VkNet.Model;
using static System.Console;
using static tvkm.Api.LongpollDaemon;
using static tvkm.Dialogs.DialogsSection;
using static tvkm.Settings;

namespace tvkm.Dialogs;

public class DialogsScreen : DialogsScreenBase, IScreen<App>
{
    private int _selectedPeerItem;
    private int _selectedChatItem;

    public DialogsSection Focus = PeersList;

    private readonly List<char> _message = new();
    private int _messageFieldCursorX;

    private readonly ScreenStack<App> _stack;

    private object _drawLock => _stack.PartialDrawLock;

    public const int DialTabW = 50;
    private const string DateFormat = " [hh:mm]: ";

    public DialogsScreen(ScreenStack<App> stack) : base(stack.MainScreen.Api)
    {
        _stack = stack;
    }

    #region Draw utils

    private static void DrawBlockTitle(int w, string t)
    {
        Write((char) 0x250C);
        Write((char) 0x2500);
        Write(t);
        for (var i = 2 + t.Length; i < w - 1; i++)
            Write((char) 0x2500);
        Write((char) 0x2510);
    }

    private static void DrawBlockBottom(int w)
    {
        Write((char) 0x2514);
        for (var i = 1; i < w - 1; i++)
            Write((char) 0x2500);
        Write((char) 0x2518);
    }

    private static void DrawBorder(int x, int y, int w, int h, string title, ConsoleColor clr)
    {
        ForegroundColor = clr;
        SetCursorPosition(x, y);
        DrawBlockTitle(w, title);
        for (var i = 0; i < h - 2; i++)
        {
            SetCursorPosition(x, y + 1 + i);
            Write((char) 0x2502);
            SetCursorPosition(x + w - 1, y + 1 + i);
            Write((char) 0x2502);
        }

        SetCursorPosition(x, y + h - 1);
        DrawBlockBottom(w);
    }

    private void DrawTextboxBorder()
    {
        DrawBorder(0, BufferHeight - 3, BufferWidth, 3, "Ваше сообщение",
            Focus == InputField ? ConsoleColor.Yellow : ConsoleColor.White);
    }

    private void DrawHistoryBorder()
    {
        DrawBorder(DialTabW, 0, BufferWidth - DialTabW, BufferHeight - 3, ActiveDialogName,
            Focus == MessagesHistory ? ConsoleColor.Yellow : ConsoleColor.White);
    }

    private static void FillSpace(int width) => Write(new string(' ', width));

    #endregion

    private void DrawPeersList()
    {
        var i = 0;
        var h = BufferHeight - 5;
        if (Peers.Count > h)
        {
            if (_selectedPeerItem > h / 2)
                i = _selectedPeerItem - h / 2;
            if (_selectedPeerItem > Peers.Count - h / 2 - 1)
                i = Peers.Count - h;
        }

        var j = 1;
        for (; i < Peers.Count && j < h + 1; i++)
        {
            SetCursorPosition(1, j);
            Peers[i].Draw(Focus == PeersList && i == _selectedPeerItem);
            j++;
        }

        DrawBorder(0, 0, DialTabW, BufferHeight - 3, "Список диалогов",
            Focus == PeersList ? SelectionColor : DefaultColor);

        var scrollProgress = (float) _selectedPeerItem / Peers.Count;
        var scrollCursorY = (int) (scrollProgress * h + 1);
        SetCursorPosition(DialTabW - 1, scrollCursorY);
        Write((char) 0x2588);
    }

    public void Draw()
    {
        DrawTextboxBorder();
        DrawHistoryBorder();
        DrawPeersList();
        RedrawAllMessages();
        PrintInput();
        FixCursorLocation();
    }

    private void RedrawAllMessages()
    {
        if (Msgs == null) return;

        var h = BufferHeight;
        var maxNameL = GetMaxSenderNameWidth() + DateFormat.Length;
        var msgAvailH = h - 5; // vertical space for printing
        var contentW = BufferWidth - DialTabW - 2 - maxNameL; // horizontal space for printing
        var sm = CalculateFirstMessageIndex(contentW, msgAvailH);

        // drawing
        var cursorY = 0;
        for (var i = sm + 1; i < Msgs.Count; i++)
        {
            var msg = Msgs[i];
            ForegroundColor = _selectedChatItem == i ? SelectionColor :
                msg.Author.Id == _stack.MainScreen.UserId ? SpecialColor : DefaultColor;
            SetCursorPosition(DialTabW + 1, ++cursorY);
            Write((msg.Author.Name + msg.Time.ToString(DateFormat)).PadLeft(maxNameL));
            if (msg.TextValid)
            {
                var x = 0;
                for (var k = 0; k < msg.Text.Length; k++)
                {
                    if (x >= contentW)
                    {
                        x = 0;
                        SetCursorPosition(DialTabW + 1 + maxNameL, ++cursorY);
                    }

                    switch (msg.Text[k])
                    {
                        case '\r':
                            break;
                        case '\n':
                            FillSpace(contentW - x);
                            x = int.MaxValue;
                            break;
                        default:
                            Write(msg.Text[k]);
                            x++;
                            break;
                    }
                }

                FillSpace(contentW - x);
            }
            else
                cursorY--;

            if (msg.HasAtts)
            {
                for (int j = 0; j < msg.Atts!.Length; j++)
                {
                    SetCursorPosition(DialTabW + 1 + maxNameL, ++cursorY);
                    Write($"[{msg.Atts[j].Caption}]".PadRight(contentW));
                }
            }

            if (msg.Reply != null)
            {
                SetCursorPosition(DialTabW + 1 + maxNameL, ++cursorY);
                Write($"[Ответ {msg.Reply.Author.Name}]".PadRight(contentW));
            }
        }
    }

    private void FixCursorLocation()
    {
        switch (Focus)
        {
            case InputField:
                SetCursorPosition(_messageFieldCursorX + 1, BufferHeight - 2);
                break;
            case PeersList:
                SetCursorPosition(1, _selectedPeerItem + 1);
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

    private int GetMaxSenderNameWidth()
    {
        var maxNameL = 0;
        for (var i = 0; i < Msgs.Count; i++)
            maxNameL = Math.Max(maxNameL, Msgs[i].Author.Name.Length);
        return maxNameL;
    }

    private void PrintInput()
    {
        lock (_drawLock)
        {
            SetCursorPosition(1, BufferHeight - 2);
            ForegroundColor = Focus == InputField ? SelectionColor : DefaultColor;
            if (_message.Count <= BufferWidth - 3)
            {
                Write(_message.ToArray());
                for (var i = _message.Count; i < BufferWidth - DialTabW - 3; i++)
                    Write(' ');
                return;
            }

            for (var i = _message.Count - (BufferWidth - 3); i < _message.Count; i++)
            {
                Write(_message[i]);
            }

            Write(' ');
        }
    }

    public void HandleKey(InputEvent e, ScreenStack<App> stack)
    {
        var l = stack.PartialDrawLock;

        void RedrawInput()
        {
            lock (_drawLock)
            {
                PrintInput();
                stack.CancelRedraw();
                FixCursorLocation();
            }
        }

        void ClearInput()
        {
            _message.Clear();
            _messageFieldCursorX = 0;
        }

        switch (Focus)
        {
            case PeersList:
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
                        Focus = InputField;
                        _selectedChatItem = Msgs?.Count ?? -1;
                        lock (_drawLock)
                        {
                            RedrawAllMessages();
                            DrawTextboxBorder();
                            DrawHistoryBorder();
                        }

                        break;
                }


                lock (_drawLock)
                {
                    DrawPeersList();
                    stack.CancelRedraw();
                    FixCursorLocation();
                }

                break;
            case MessagesHistory:
                switch (e.Action)
                {
                    case InputAction.Return:
                        Focus = InputField;
                        _selectedChatItem = -1;
                        RedrawInput();
                        break;
                    case InputAction.MoveUp:
                        _selectedChatItem--;
                        break;
                    case InputAction.MoveDown:
                        _selectedChatItem++;
                        if (_selectedChatItem < Msgs.Count)
                            break;

                        Focus = InputField;
                        RedrawInput();
                        lock (_drawLock)
                        {
                            DrawTextboxBorder();
                            DrawHistoryBorder();
                        }

                        break;
                    case InputAction.Activate:
                        if (_selectedChatItem >= 0 && _selectedChatItem < Msgs.Count)
                            Msgs[_selectedChatItem].Open(_stack);
                        return;
                }

                lock (_drawLock)
                {
                    RedrawAllMessages();
                    FixCursorLocation();
                    _stack.CancelRedraw();
                }

                break;
            case InputField:
                lock (this)
                {
                    switch (e.Action)
                    {
                        case InputAction.Return:
                            if (_message.Count == 0)
                            {
                                Focus = PeersList;
                                OpenDialog(null);
                                break;
                            }

                            ClearInput();
                            break;
                        case InputAction.Activate:
                            if (_message.Count > 0 && Send(new string(_message.ToArray())))
                                ClearInput();
                            break;
                        case InputAction.SpecialKey:
                            if (e.Key == ConsoleKey.Backspace)
                            {
                                if (_message.Count > 0 && _messageFieldCursorX > 0)
                                {
                                    _message.RemoveAt(_messageFieldCursorX - 1);
                                    _messageFieldCursorX--;
                                }
                            }
                            else if (e.Key == ConsoleKey.Delete)
                            {
                                if (_message.Count > 0 && _messageFieldCursorX < _message.Count)
                                    _message.RemoveAt(_messageFieldCursorX);
                            }

                            break;
                        case InputAction.TextType:
                            _message.Insert(_messageFieldCursorX, e.Char);
                            _messageFieldCursorX++;
                            break;
                        case InputAction.MoveLeft:
                            if (_messageFieldCursorX > 0)
                                _messageFieldCursorX--;
                            break;
                        case InputAction.MoveRight:
                            if (_messageFieldCursorX < _message.Count)
                                _messageFieldCursorX++;
                            break;
                        case InputAction.MoveUp:
                            Focus = MessagesHistory;
                            _selectedChatItem = Msgs.Count - 1;
                            RedrawAllMessages();
                            break;
                    }

                    RedrawInput();
                }

                break;
        }
    }

    #region Longpoll handlers

    private void OnNewMessage(LongpollMessage msg)
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

            Msgs.Add(new MsgItem(GetUser(id), m, _api));
            lock (_drawLock)
            {
                RedrawAllMessages();
                FixCursorLocation();
            }

            ReportRead(msg.TargetId, msg.MessageId);
        }
        else
        {
            var p = Peers.FirstOrDefault(x => x.PeerId == msg.TargetId);
            if (p == null) return;
            p.UnreadCount++;
            if (Focus != PeersList)
            {
                Peers.Remove(p);
                Peers.Insert(0, p);
            }

            lock (_drawLock)
            {
                DrawPeersList();
                FixCursorLocation();
            }
        }
    }

    private void OnMessageEdit(LongpollMessageEdit msg)
    {
        //another chat
        if (msg.PeerId != CurrentDialog) return;
        //searching by ID
        MsgItem? m = Msgs?.Where(x => x.Id == msg.MessageId).FirstOrDefault();
        //not loaded message
        if (m == null) return;

        m.Update(ToFull(msg));

        lock (_drawLock)
        {
            RedrawAllMessages();
            FixCursorLocation();
        }
    }

    private CancellationTokenSource _typingLabelToken = new();

    private async void OnMessageWrite(LongpollWriteStatus msg)
    {
        if (msg.PeerId != CurrentDialog) return;

        lock (_drawLock)
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

        lock (_drawLock)
        {
            SetCursorPosition(DialTabW, 0);
            DrawBlockTitle(BufferWidth - DialTabW, ActiveDialogName);
            FixCursorLocation();
        }
    }

    #endregion

    #region Screen stack event handlers

    public void OnEnter(ScreenStack<App> stack)
    {
        OpenDialog(null);
        FetchPeersList();
        var lpd = _stack.MainScreen.Longpoll;
        lpd.OnNewMessage += OnNewMessage;
        lpd.OnMessageEdit += OnMessageEdit;
        lpd.OnMessageWrite += OnMessageWrite;
    }

    public void OnPause()
    {
    }

    public void OnResume()
    {
    }

    public void OnLeave()
    {
        var lpd = _stack.MainScreen.Longpoll;
        lpd.OnNewMessage -= OnNewMessage;
        lpd.OnMessageEdit -= OnMessageEdit;
        lpd.OnMessageWrite -= OnMessageWrite;
    }

    #endregion

    //TODO return actual items
    public IItem<App>? Current => null;
}