﻿using System;
using Newtonsoft.Json;
using VkNet.Enums.SafetyEnums;
using VkNet.Utils;
using VkNet.Utils.JsonConverter;

namespace VkNet.Model;

/// <summary>
///     Информация о родственнике.
///     См. описание http://vk.com/dev/fields
/// </summary>
[Serializable]
public class Relative
{
	/// <summary>
	///     Идентификатор родственника.
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	///     Тип родственника (sibling и т.п.)
	/// </summary>
	[JsonConverter(typeof(SafetyEnumJsonConverter))]
    public RelativeType Type { get; set; }

	/// <summary>
	///     Имя родственника, если он не является пользователем ВКонтакте.
	/// </summary>
	public string Name { get; set; }

    #region Методы

    /// <summary>
    ///     Разобрать из json.
    /// </summary>
    /// <param name="response"> Ответ сервера. </param>
    /// <returns> </returns>
    public static Relative FromJson(VkResponse response)
    {
        var relative = new Relative
        {
            Id = response["user_id"] ?? response["uid"] ?? response["id"], Type = response["type"],
            Name = response["name"]
        };

        return relative;
    }

    #endregion
}