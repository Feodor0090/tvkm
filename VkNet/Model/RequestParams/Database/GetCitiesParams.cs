using System;
using Newtonsoft.Json;

namespace VkNet.Model.RequestParams.Database;

/// <summary>
///     Параметры запроса database.getCities
/// </summary>
[Serializable]
public class GetCitiesParams
{
	/// <summary>
	///     Идентификатор страны, полученный в методе database.getCountries
	/// </summary>
	[JsonProperty("country_id")]
    public int CountryId { get; set; }

	/// <summary>
	///     идентификатор региона, города которого необходимо получить. (параметр не
	///     обязателен)
	/// </summary>
	[JsonProperty("region_id")]
    public int? RegionId { get; set; }

	/// <summary>
	///     Строка поискового запроса. Например, Санкт. Максимальная длина строки — 15
	///     символов.
	/// </summary>
	[JsonProperty("q")]
    public string Query { get; set; }

	/// <summary>
	///     1 – возвращать все города. 0 – возвращать только основные города.
	/// </summary>
	[JsonProperty("need_all")]
    public bool? NeedAll { get; set; }

	/// <summary>
	///     Количество городов, которые необходимо вернуть.
	/// </summary>
	[JsonProperty("count")]
    public uint? Count { get; set; }

	/// <summary>
	///     Отступ, необходимый для получения определенного подмножества городов.
	/// </summary>
	[JsonProperty("offset")]
    public uint? Offset { get; set; }
}