using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VkNet.Enums;
using VkNet.Utils;

namespace VkNet.Model;

/// <summary>
///     Информация о забанненом (добавленном в черный список) пользователе сообщества.
/// </summary>
/// <remarks>
///     Страница документации ВКонтакте http://vk.com/dev/groups.getBanned
/// </remarks>
[DebuggerDisplay("[{AdminId}] {Comment} ({Reason})")]
[Serializable]
public class BanInfo
{
	/// <summary>
	///     Идентификатор администратора, который добавил пользователя в черный список.
	/// </summary>
	[JsonProperty("admin_id")]
    public long? AdminId { get; set; }

	/// <summary>
	///     Дата добавления пользователя в черный список.
	/// </summary>
	[JsonProperty("date")]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? Date { get; set; }

	/// <summary>
	///     Текст комментария к бану.
	/// </summary>
	[JsonProperty("comment")]
    public string Comment { get; set; }

	/// <summary>
	///     Дата, когда пользователь будет разбанен.
	/// </summary>
	[JsonProperty("end_date")]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? EndDate { get; set; }

	/// <summary>
	///     Причина добавления пользователя в черный список.
	/// </summary>
	[JsonProperty("reason")]
    public BanReason Reason { get; set; }

    #region Методы

    /// <summary>
    ///     Разобрать из json.
    /// </summary>
    /// <param name="response"> Ответ сервера. </param>
    /// <returns> </returns>
    public static BanInfo FromJson(VkResponse response)
    {
        var info = new BanInfo
        {
            AdminId = response["admin_id"], Date = response["date"], Comment = response["comment"],
            EndDate = response["end_date"], Reason = response["reason"]
        };

        return info;
    }

    #endregion
}