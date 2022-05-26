using System;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace VkNet.Model.GroupUpdate;

/// <summary>
///     Новая запись на стене (<c>WallPost</c>, <c>WallRepost</c>)
///     (<c>Post</c> с дополнительными полями)
/// </summary>
[Serializable]
public class WallPost : Post
{
	/// <summary>
	///     <c>Id</c> отложенной записи
	/// </summary>
	public long? PostponedId { get; set; }

	/// <summary>
	///     Разобрать из json.
	/// </summary>
	/// <param name="response"> Ответ сервера. </param>
	public new static WallPost FromJson(VkResponse response)
    {
        return new WallPost
        {
            Id = response["id"],
            OwnerId = response["owner_id"],
            FromId = response["from_id"],
            Date = response["date"],
            Text = response["text"],
            ReplyOwnerId = response["reply_owner_id"],
            ReplyPostId = response["reply_post_id"],
            FriendsOnly = response["friends_only"],
            Comments = response["comments"],
            Likes = response["likes"],
            Reposts = response["reposts"],
            PostType = response["post_type"],
            PostSource = response["post_source"],
            Attachments = response["attachments"].ToReadOnlyCollectionOf<Attachment>(x => x),
            Geo = response["geo"],
            SignerId = response["signer_id"],
            CopyPostDate = response["copy_post_date"],
            CopyPostType = response["copy_post_type"],
            CopyOwnerId = response["copy_owner_id"],
            CopyPostId = response["copy_post_id"],
            CopyText = response["copy_text"],
            CopyHistory = response["copy_history"].ToReadOnlyCollectionOf<Post>(x => x),
            IsPinned = response["is_pinned"],
            CreatedBy = response["created_by"],
            CopyCommenterId = response["copy_commenter_id"],
            CopyCommentId = response["copy_comment_id"],
            CanDelete = response["can_delete"],
            CanEdit = response["can_edit"],
            CanPin = response["can_pin"],
            Views = response["views"],
            MarkedAsAds = response["marked_as_ads"],
            AccessKey = response["access_key"],
            PostponedId = response["postponed_id"],
            Donut = response["donut"]
        };
    }
}