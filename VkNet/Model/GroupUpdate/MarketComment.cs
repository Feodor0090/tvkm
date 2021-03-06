using System;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace VkNet.Model.GroupUpdate;

/// <summary>
///     Добавление/редактирование/восстановление комментария к товару
///     (<c>MarketCommentNew</c>, <c>MarketCommentEdit</c>, <c>MarketCommentRestore</c>)
///     (<c>Comment</c> с дополнительными полями)
/// </summary>
[Serializable]
public class MarketComment : Comment
{
	/// <summary>
	///     Идентификатор товара
	/// </summary>
	public ulong? ItemId { get; set; }

	/// <summary>
	///     Идентификатор владельца товара
	/// </summary>
	public long? MarketOwnerId { get; set; }

	/// <summary>
	///     Разобрать из json.
	/// </summary>
	/// <param name="response"> Ответ сервера. </param>
	public new static MarketComment FromJson(VkResponse response)
    {
        return new MarketComment
        {
            Id = response["id"],
            FromId = response["from_id"],
            Date = response["date"],
            Text = response["text"],
            ReplyToUser = response["reply_to_user"],
            ReplyToComment = response["reply_to_comment"],
            Attachments = response["attachments"].ToReadOnlyCollectionOf<Attachment>(x => x),
            Likes = response["likes"],
            ItemId = response["item_id"],
            MarketOwnerId = response["market_owner_id"]
        };
    }
}