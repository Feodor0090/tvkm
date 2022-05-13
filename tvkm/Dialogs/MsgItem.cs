using tvkm.Api;
using tvkm.UIEngine;
using tvkm.UIEngine.Controls;
using tvkm.UIEngine.Templates;
using VkNet;
using VkNet.Model;
using Button = tvkm.UIEngine.Controls.Button;

namespace tvkm.Dialogs;

public sealed class MsgItem
{
    public MsgItem(VkUser author, Message msg, VkApi? api)
    {
        Author = author;
        _api = api;
        Id = msg.Id ?? 0;
        Update(msg);
    }

    public MsgItem(string text)
    {
        Author = new VkUser(0, "empty user");
        Text = text.ToCharArray();
    }

    public readonly VkUser Author;
    private readonly VkApi? _api;
    public char[] Text;
    public DateTime Time;
    public MsgItem? Reply;
    public readonly long Id;
    public Attachment[]? Atts;

    public bool HasAtts => Atts is {Length: > 0};

    /// <summary>
    /// Is the text of the message is a valid text to display?
    /// </summary>
    public bool TextValid => Text.Length > 0 && !Text.All(y => y is '\r' or '\n' or ' ');

    public void Update(Message msg)
    {
        Text = msg.Text.ToCharArray();
        Time = msg.Date ?? DateTime.Now;
        if (msg.ReplyMessage != null)
            Reply = new MsgItem(VkUser.Get(msg.ReplyMessage.FromId ?? 0, null!), msg.ReplyMessage, _api);

        if (msg.Attachments?.Any() ?? false)
            Atts = Attachment.Convert(msg.Attachments);
    }

    public void Open(ScreenStack stack)
    {
        ListScreen s = new ListScreen("Меню сообщения");
        s.Add(new Button($"Отправитель: {Author.Name}", () =>
        {
            if (_api != null)
                stack.Push(new UserView(Author, _api));
        }));
        
        if(Atts!=null)
            foreach (var att in Atts)
            {
                s.Add(new Button(att.Caption, () => att.View(stack)));
            }
        
        stack.Push(s);
    }
}