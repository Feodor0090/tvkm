using tvkm.UIEngine;
using tvkm.UIEngine.Templates;

namespace tvkm.Dialogs;

public sealed class DialogItem
{
    public readonly long PeerId;
    public readonly string PeerName;
    private readonly DialogsScreenBase? _target;
    public int UnreadCount;

    public DialogItem(long id, string name, DialogsScreenBase? target)
    {
        PeerId = id;
        PeerName = name;
        _target = target;
    }

    public void Draw(bool selected, DialogsScreen screen)
    {
        if (selected)
            Console.ForegroundColor = Settings.SelectionColor;
        else
            Console.ForegroundColor = UnreadCount == 0 ? Settings.DefaultColor : Settings.SpecialColor;
        var stp = PeerName;
        if (UnreadCount != 0)
            stp = $"(+{UnreadCount}) " + stp;
        Console.Write(stp.PadRightOrTrim(screen.ChatsListWidth - 2));
    }

    public void HandleKey(InputEvent e, ScreenStack<App> stack = null!)
    {
        if (e.Action != InputAction.Activate) return;
        UnreadCount = 0;
        _target?.OpenDialog(this);
    }

    public int Height => 1;
}