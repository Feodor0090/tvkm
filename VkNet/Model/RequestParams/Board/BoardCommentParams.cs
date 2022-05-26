using System;
using Newtonsoft.Json;
using VkNet.Utils;

namespace VkNet.Model.RequestParams;

/// <summary>
///     Параметры метода wall.addComment
/// </summary>
[Serializable]
public class BoardCommentParams
{
	/// <summary>
	///     Идентификатор сообщества, в котором находится обсуждение. положительное число,
	///     обязательный параметр
	/// </summary>
	[JsonProperty("group_id")]
    public long GroupId { get; set; }

	/// <summary>
	///     Идентификатор обсуждения. положительное число,
	///     обязательный параметр
	/// </summary>
	[JsonProperty("topic_id")]
    public long TopicId { get; set; }

	/// <summary>
	///     Идентификатор комментария в обсуждении, обязательный параметр.
	/// </summary>
	[JsonProperty("comment_id")]
    public long CommentId { get; set; }

	/// <summary>
	///     Идентификатор капчи
	/// </summary>
	[JsonProperty("captcha_sid")]
    [Obsolete(ObsoleteText.CaptchaNeeded, true)]
    public long? CaptchaSid { get; set; }

	/// <summary>
	///     Текст, который ввел пользователь
	/// </summary>
	[JsonProperty("captcha_key")]
    [Obsolete(ObsoleteText.CaptchaNeeded, true)]
    public string CaptchaKey { get; set; }
}