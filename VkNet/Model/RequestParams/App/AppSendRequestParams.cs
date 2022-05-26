using System;
using Newtonsoft.Json;
using VkNet.Enums.SafetyEnums;
using VkNet.Utils.JsonConverter;

namespace VkNet.Model.RequestParams;

/// <summary>
///     Параметры запроса SendRequest для приложений.
/// </summary>
[Serializable]
public class AppSendRequestParams
{
	/// <summary>
	///     Идентификатор пользователя, которому следует отправить запрос.
	/// </summary>
	[JsonProperty("user_id")]
    public ulong UserId { get; set; }

	/// <summary>
	///     Текст запроса.
	/// </summary>
	[JsonProperty("text")]
    public string Text { get; set; }

	/// <summary>
	///     Тип запроса, может принимать значения:.
	/// </summary>
	[JsonProperty("type")]
    [JsonConverter(typeof(SafetyEnumJsonConverter))]
    public AppRequestType Type { get; set; }

	/// <summary>
	///     Уникальное в рамках приложения имя для каждого вида отправляемого запроса.
	/// </summary>
	[JsonProperty("name")]
    public string Name { get; set; }

	/// <summary>
	///     Строка, которая будет возвращена назад при переходе пользователя по запросу в
	///     приложение. Может использоваться для
	///     подсчета конверсии.
	/// </summary>
	[JsonProperty("key")]
    public string Key { get; set; }

	/// <summary>
	///     Запрет на группировку запроса с другими, имеющими тот же name. По умолчанию
	///     отключен.
	/// </summary>
	[JsonProperty("separate")]
    public bool Separate { get; set; }
}