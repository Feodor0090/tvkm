﻿using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VkNet.Model.Attachments;
using VkNet.Utils;
using VkNet.Utils.JsonConverter;

namespace VkNet.Model;

/// <summary>
///     Комментарий в обсуждении
/// </summary>
[Serializable]
public class CommentBoard
{
	/// <summary>
	///     Идентификатор комментария.
	/// </summary>
	[JsonProperty("id")]
    public long Id { get; set; }

	/// <summary>
	///     Идентификатор автора комментария.
	/// </summary>
	[JsonProperty("from_id")]
    public long FromId { get; set; }

	/// <summary>
	///     Дата создания (в формате Unixtime).
	/// </summary>
	[JsonProperty("date")]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime Date { get; set; }

	/// <summary>
	///     Текст комментария.
	/// </summary>
	[JsonProperty("text")]
    public string Text { get; set; }

	/// <summary>
	///     Медиавложения комментария (фотографии, ссылки и т.п.).
	/// </summary>
	[JsonProperty("attachments")]
    [JsonConverter(typeof(AttachmentJsonConverter))]
    public ReadOnlyCollection<Attachment> Attachments { get; set; }

	/// <summary>
	///     Информация об отметках «Мне нравится» текущего комментария (если был задан
	///     параметр need_likes)
	/// </summary>
	[JsonProperty("likes")]
    public Likes Likes { get; set; }

    #region Методы

    /// <summary>
    ///     Разобрать из json.
    /// </summary>
    /// <param name="response"> Ответ сервера. </param>
    /// <returns> </returns>
    public static CommentBoard FromJson(VkResponse response)
    {
        return new CommentBoard
        {
            Id = response["id"], FromId = response["from_id"], Date = response["date"], Text = response["text"],
            Likes = response["likes"], Attachments = response["attachments"].ToReadOnlyCollectionOf<Attachment>(x => x)
        };
    }

    /// <summary>
    ///     Преобразовать из VkResponse
    /// </summary>
    /// <param name="response"> Ответ. </param>
    /// <returns>
    ///     Результат преобразования.
    /// </returns>
    public static implicit operator CommentBoard(VkResponse response)
    {
        return !response.HasToken()
            ? null
            : FromJson(response);
    }

    #endregion
}