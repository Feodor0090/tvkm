using System;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace VkNet.Model.GroupUpdate;

/// <summary>
///     Добавление/редактирование/восстановление комментария к фотографии
///     (<c>PhotoCommentNew</c>, <c>PhotoCommentEdit</c>, <c>PhotoCommentRestore</c>)
///     (<c>Comment</c> с дополнительными полями)
/// </summary>
[Serializable]
public class PhotoComment : Comment
{
	/// <summary>
	///     Идентификатор фотографии
	/// </summary>
	public long? PhotoId { get; set; }

	/// <summary>
	///     Идентификатор владельца фотографии
	/// </summary>
	public long? PhotoOwnerId { get; set; }

	/// <summary>
	///     Разобрать из json.
	/// </summary>
	/// <param name="response"> Ответ сервера. </param>
	public new static PhotoComment FromJson(VkResponse response)
    {
        return new PhotoComment
        {
            Id = response["id"],
            FromId = response["from_id"],
            Date = response["date"],
            Text = response["text"],
            ReplyToUser = response["reply_to_user"],
            ReplyToComment = response["reply_to_comment"],
            Attachments = response["attachments"].ToReadOnlyCollectionOf<Attachment>(x => x),
            Likes = response["likes"],
            PhotoId = response["photo_id"],
            PhotoOwnerId = response["photo_owner_id"]
        };
    }
}