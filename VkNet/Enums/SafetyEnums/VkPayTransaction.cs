using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VkNet.Utils;

namespace VkNet.Model.GroupUpdate;

/// <summary>
///     платёж через VK Pay
/// </summary>
[Serializable]
public class VkPayTransaction
{
	/// <summary>
	///     Идентификатор пользователя-отправителя перевода.
	/// </summary>
	[JsonProperty("from_id")]
    public long? FromId { get; set; }

	/// <summary>
	///     Cумма перевода в тысячных рубля.
	/// </summary>
	[JsonProperty("amount")]
    public long Amount { get; set; }

	/// <summary>
	///     Комментарий к переводу.
	/// </summary>
	[JsonProperty("description")]
    public string? Description { get; set; }

	/// <summary>
	///     Время отправки перевода в Unixtime.
	/// </summary>
	[JsonConverter(typeof(UnixDateTimeConverter))]
    [JsonProperty("date")]
    public DateTime Date { get; set; }


    #region Методы

    /// <summary>
    /// </summary>
    /// <param name="response"> </param>
    /// <returns> </returns>
    public static VkPayTransaction FromJson(VkResponse response)
    {
        return new VkPayTransaction
        {
            FromId = response["from_id"],
            Amount = response["amount"],
            Description = response["description"],
            Date = response["date"]
        };
    }


    /// <summary>
    ///     Преобразование класса <see cref="VkPayTransaction" /> в <see cref="VkParameters" />
    /// </summary>
    /// <param name="response"> Ответ сервера. </param>
    /// <returns>Результат преобразования в <see cref="VkPayTransaction" /></returns>
    public static implicit operator VkPayTransaction(VkResponse response)
    {
        if (response == null) return null;

        return response.HasToken()
            ? FromJson(response)
            : null;
    }

    #endregion
}