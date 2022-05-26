﻿using System;
using Newtonsoft.Json;
using VkNet.Utils;

namespace VkNet.Model;

/// <summary>
///     Адрес сервера для загрузки фотографий
/// </summary>
[Serializable]
public class UploadServerInfo
{
	/// <summary>
	///     Адрес для загрузки фотографий
	/// </summary>
	[JsonProperty("upload_url")]
    public string UploadUrl { get; set; }

	/// <summary>
	///     Идентификатор альбома, в который будет загружена фотография
	/// </summary>
	[JsonProperty("album_id")]
    public long? AlbumId { get; set; }

	/// <summary>
	///     Идентификатор пользователя, от чьего имени будет загружено фото
	/// </summary>
	[JsonProperty("user_id")]
    public long? UserId { get; set; }

    #region Methods

    /// <summary>
    ///     Разобрать из json.
    /// </summary>
    /// <param name="response"> Ответ сервера. </param>
    /// <returns> </returns>
    public static UploadServerInfo FromJson(VkResponse response)
    {
        var info = new UploadServerInfo
        {
            UploadUrl = response["upload_url"],
            AlbumId = Utilities.GetNullableLongId(response["album_id"] ?? response["aid"]), UserId =
                Utilities.GetNullableLongId(
                    response["user_id"] ?? response["message_id"] ?? response["mid"])
        };

        return info;
    }

    #endregion
}