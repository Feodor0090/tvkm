using tvkm.Api;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace tvkm.Dialogs;

public abstract class DialogsScreenBase
{
    protected DialogsScreenBase(VkApi api)
    {
        _api = api;
    }

    protected readonly List<DialogItem> Peers = new();
    protected List<MsgItem>? Msgs;
    private readonly VkApi _api;

    protected long CurrentDialog { get; private set; }

    protected string ActiveDialogName = EmptyChatTitle;

    public const string EmptyChatTitle = "Ни один диалог не открыт";

    public void OpenDialog(long peerId, string peerName)
    {
        ActiveDialogName = peerId == 0 ? EmptyChatTitle : peerName;
        CurrentDialog = peerId;
        
        if (peerId == 0)
            Msgs = null;
        else
            LoadHistory(peerId);
    }

    public void OpenDialog(DialogItem? d)
    {
        if (d == null)
            OpenDialog(0, string.Empty);
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
            foreach (var (user, peer) in l)
            {
                var conv = peer.Conversation;
                var name = user.Id != 0 ? user.Name : conv.ChatSettings?.Title ?? "UNTITLED_CHAT";
                Peers.Add(new DialogItem(conv.Peer.Id, name, this)
                {
                    UnreadCount = (int) (conv.UnreadCount ?? 0)
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
        if (Settings.SendReadEvent) _api.Messages.MarkAsRead(peer.ToString(), id);
    }

    protected Message ToFull(LongpollDaemon.LongpollMessage msg) => msg.LoadFull(_api);

    protected Message ToFull(LongpollDaemon.LongpollMessageEdit msg) => msg.LoadFull(_api);

    protected VkUser GetUser(long id) => VkUser.Get(id, _api);
}