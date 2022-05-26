using System;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace VkNet.Model.GroupUpdate;

/// <summary>
///     Добавление/редактирование/восстановление комментария на стене
///     (<c>WallReplyNew</c>, <c>WallReplyEdit</c>, <c>WallReplyRestore</c>)
///     (<c>Comment</c> с дополнительными полями)
/// </summary>
[Serializable]
public class WallReply : Comment
{
	/// <summary>
	///     Идентификатор записи
	/// </summary>
	public long? PostId { get; set; }

	/// <summary>
	///     Идентификатор владельца записи
	/// </summary>
	public long? PostOwnerId { get; set; }

	/// <summary>
	///     Разобрать из json.
	/// </summary>
	/// <param name="response"> Ответ сервера. </param>
	public new static WallReply FromJson(VkResponse response)
    {
        return new WallReply
        {
            Id = response["id"],
            FromId = response["from_id"],
            Date = response["date"],
            Text = response["text"],
            ReplyToUser = response["reply_to_user"],
            ReplyToComment = response["reply_to_comment"],
            Attachments = response["attachments"].ToReadOnlyCollectionOf<Attachment>(x => x),
            Likes = response["likes"],
            PostId = response["post_id"],
            PostOwnerId = response["post_owner_id"]
        };
    }
}