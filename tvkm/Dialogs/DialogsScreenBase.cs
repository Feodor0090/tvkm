using tvkm.Api;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace tvkm.Dialogs;

public abstract class DialogsScreenBase
{
    protected DialogsScreenBase(VkApi api)
    {
        if (Inst != null) throw new InvalidOperationException("DialogsScreen already exists.");
        _api = api;
        Inst = this;
    }

    protected List<DialogItem> Peers = new();

    protected List<MsgItem>? Msgs;

    protected static DialogsScreenBase? Inst;

    private readonly VkApi _api;

    protected static long CurrentDialog { get; private set; }

    protected string ActiveDialogName = "Ни один диалог не открыт";

    public void OpenDialog(long peerId, string peerName)
    {
        if (peerId == 0)
        {
            ActiveDialogName = "Ни один диалог не открыт";
            Msgs = null;
            CurrentDialog = 0;
            return;
        }

        CurrentDialog = peerId;
        ActiveDialogName = peerName;
        LoadHistory(peerId);
    }

    public void OpenDialog(DialogItem? d)
    {
        if (d == null)
            OpenDialog(0, "");
        else
            OpenDialog(d.PeerId, d.PeerName);
    }

    protected void FetchPeersList()
    {
        var d = _api.Messages.GetConversations(new GetConversationsParams {Count = 100, Extended = true});
        var users = VkUser.ToUsers(d.Profiles, d.Groups);
        var l = VkUser.MapObjectsWithUsers(d.Items, users, x => (int) x.Conversation.Peer.Id);
        lock (Peers)
        {
            Peers.Clear();
            foreach (var x in l)
            {
                var peerId = (int) x.Item2.Conversation.Peer.Id;
                var unread = (int) (x.Item2.Conversation.UnreadCount ?? 0);
                var name = x.Item1.Id != 0 ? x.Item1.Name : x.Item2.Conversation.ChatSettings?.Title ?? "UNTITLED_CHAT";
                Peers.Add(new DialogItem(peerId, name)
                {
                    UnreadCount = unread
                });
            }
        }
    }

    private void LoadHistory(long peer)
    {
        lock (this)
        {
            var m = _api.Messages.GetHistory(
                new MessagesGetHistoryParams {Count = 100, Extended = true, PeerId = peer});
            var u = VkUser.ToUsers(m.Users, m.Groups);
            foreach (var x in u)
            {
                x.Cache();
            }

            Msgs = VkUser.MapObjectsWithUsers(m.Messages, u, x => (int) (x.FromId ?? 0))
                .Select(x => new MsgItem(x.Item1.Id != 0 ? x.Item1 : VkUser.Get(0, _api), x.Item2))
                .ToList();
            ReportRead(peer, Msgs[0].Id);
            Msgs.Reverse();
        }
    }

    protected bool Send(string msg)
    {
        try
        {
            _api.Messages.Send(new MessagesSendParams
            {
                PeerId = CurrentDialog,
                Message = msg,
                RandomId = DateTime.Now.Ticks,
                ReplyTo = 0,
            });
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected void ReportRead(long peer, long id)
    {
        if (!Settings.SendReadEvent) return;
        _api.Messages.MarkAsRead(peer.ToString(), id);
    }

    protected Message ToFull(LongpollDaemon.LongpollMessage msg) => msg.LoadFull(_api);

    protected Message ToFull(LongpollDaemon.LongpollMessageEdit msg) => msg.LoadFull(_api);

    protected VkUser GetUser(long id) => VkUser.Get(id, _api);
}