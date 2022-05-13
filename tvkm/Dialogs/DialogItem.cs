using tvkm.UIEngine;

namespace tvkm.Dialogs;

public sealed class DialogItem : IItem
{
    public readonly int PeerId;
    public readonly string PeerName;
    public int UnreadCount;

    public DialogItem(int id, string name)
    {
        PeerId = id;
        PeerName = name;
    }

    public void Draw(bool selected)
    {
        if (selected)
            Console.ForegroundColor = Settings.SelectionColor;
        else
            Console.ForegroundColor = UnreadCount == 0 ? Settings.DefaultColor : Settings.AccentColor;
        var stp = PeerName;
        if (UnreadCount != 0)
            stp = $"(+{UnreadCount}) " + stp;
        Console.Write(stp.PadRightOrTrim(DialogsScreen.DialTabW - 2));
    }

    public void HandleKey(InputEvent e, ScreenStack stack = null!)
    {
        if (e.Action != InputAction.Activate) return;
        UnreadCount = 0;
        DialogsScreen.Open(this);
    }

    public int Height => 1;
}