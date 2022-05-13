using tvkm.UIEngine;

namespace tvkm.Dialogs;

public sealed class DialogItem : IItem
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

    public void Draw(bool selected)
    {
        if (selected)
            Console.ForegroundColor = Settings.SelectionColor;
        else
            Console.ForegroundColor = UnreadCount == 0 ? Settings.DefaultColor : Settings.SpecialColor;
        var stp = PeerName;
        if (UnreadCount != 0)
            stp = $"(+{UnreadCount}) " + stp;
        Console.Write(stp.PadRightOrTrim(DialogsScreen.DialTabW - 2));
    }

    public void HandleKey(InputEvent e, ScreenStack stack = null!)
    {
        if (e.Action != InputAction.Activate) return;
        UnreadCount = 0;
        _target?.OpenDialog(this);
    }

    public int Height => 1;
}