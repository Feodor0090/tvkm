using System;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace VkNet.Model.GroupUpdate;

/// <summary>
///     Добавление/редактирование/восстановление комментария в обсуждении(<c>BoardPostNew</c>, <c>BoardPostEdit</c>,
///     <c>BoardPostRestore</c>)
///     (<c>CommentBoard</c> с дополнительными полями)
/// </summary>
[Serializable]
public class BoardPost : CommentBoard
{
	/// <summary>
	///     Идентификатор обсуждения
	/// </summary>
	public ulong? TopicId { get; set; }

	/// <summary>
	///     Идентификатор владельца обсуждения
	/// </summary>
	public long? TopicOwnerId { get; set; }

	/// <summary>
	///     Разобрать из json.
	/// </summary>
	/// <param name="response"> Ответ сервера. </param>
	public new static BoardPost FromJson(VkResponse response)
    {
        return new BoardPost
        {
            Id = response["id"],
            FromId = response["from_id"],
            Date = response["date"],
            Text = response["text"],
            Likes = response["likes"],
            Attachments = response["attachments"].ToReadOnlyCollectionOf<Attachment>(x => x),
            TopicId = response["topic_id"],
            TopicOwnerId = response["topic_owner_id"]
        };
    }
}