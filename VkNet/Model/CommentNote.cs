using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VkNet.Utils;

namespace VkNet.Model;

/// <summary>
///     Комментарий к заметке
/// </summary>
[Serializable]
public class CommentNote
{
	/// <summary>
	///     идентификатор комментария
	/// </summary>
	[JsonProperty("id")]
    public long? Id { get; set; }

	/// <summary>
	///     идентификатор автора комментария
	/// </summary>
	[JsonProperty("uid")]
    public long? UserId { get; set; }

	/// <summary>
	///     идентификатор заметки
	/// </summary>
	[JsonProperty("nid")]
    public long? NoteId { get; set; }

	/// <summary>
	///     идентификатор владельца заметки
	/// </summary>
	[JsonProperty("oid")]
    public long? OwnerId { get; set; }

	/// <summary>
	///     Дата добавления комментария в формате unixtime
	/// </summary>
	[JsonConverter(typeof(UnixDateTimeConverter))]
    [JsonProperty("date")]
    public DateTime? Date { get; set; }

	/// <summary>
	///     текст комментария
	/// </summary>
	[JsonProperty("message")]
    public string Message { get; set; }

	/// <summary>
	///     идентификатор пользователя, в ответ на комментарий которого
	///     был оставлен текущий комментарий (если доступно).
	/// </summary>
	[JsonProperty("reply_to")]
    public long? ReplyTo { get; set; }

    #region Методы

    /// <summary>
    ///     Разобрать из json.
    /// </summary>
    /// <param name="response">Ответ сервера. </param>
    /// <returns>Результат преобразования.</returns>
    public static CommentNote FromJson(VkResponse response)
    {
        return new CommentNote
        {
            Id = response["id"],
            UserId = response["uid "],
            NoteId = response["nid "],
            OwnerId = response["oid"],
            Date = response["date "],
            Message = response["message"],
            ReplyTo = response["reply_to"]
        };
    }

    /// <summary>
    ///     Преобразовать из VkResponse
    /// </summary>
    /// <param name="response"> Ответ. </param>
    /// <returns>
    ///     Результат преобразования.
    /// </returns>
    public static implicit operator CommentNote(VkResponse response)
    {
        return !response.HasToken()
            ? null
            : FromJson(response);
    }

    #endregion
}