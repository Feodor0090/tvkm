using tvkm.Api;
using VkNet.Model;

namespace tvkm.Dialogs;

public sealed class MsgItem
{
    public MsgItem(VkUser author, Message msg)
    {
        Author = author;
        Id = msg.Id ?? 0;
        Update(msg);
    }

    public MsgItem(string text)
    {
        Author = new VkUser(0, "empty user");
        Text = text.ToCharArray();
    }

    public readonly VkUser Author;
    public char[] Text;
    public DateTime Time;
    public MsgItem? Reply;
    public readonly long Id;
    public Attachment[]? Atts;

    public bool HasAtts => Atts is { Length: > 0 };

    /// <summary>
    /// Is the text of the message is a valid text to display?
    /// </summary>
    public bool TextValid => Text.Length > 0 && !Text.All(y => y is '\r' or '\n' or ' ');

    public void Update(Message msg)
    {
        Text = msg.Text.ToCharArray();
        Time = msg.Date ?? DateTime.Now;
        if (msg.ReplyMessage != null)
        {
            Reply = new MsgItem(VkUser.Get(msg.ReplyMessage.FromId ?? 0, null!), msg.ReplyMessage);
        }

        if (msg.Attachments?.Any() ?? false)
        {
            Atts = Attachment.Convert(msg.Attachments);
        }
    }
}