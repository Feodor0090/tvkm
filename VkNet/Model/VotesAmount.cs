using System;
using Newtonsoft.Json;

namespace VkNet.Model;

/// <summary>
///     Количество голосов
/// </summary>
[Serializable]
public class VotesAmount
{
	/// <summary>
	///     Количество голосов
	/// </summary>
	[JsonProperty("votes")]
    public string Votes { get; set; }

	/// <summary>
	///     Общая сумма голосов, переведённая в валюту
	/// </summary>
	[JsonProperty("amount")]
    public int Amount { get; set; }

	/// <summary>
	///     Описание общей суммы с наименованием валюты
	/// </summary>
	[JsonProperty("description")]
    public string Description { get; set; }

	/// <summary>
	///     Название валюты
	/// </summary>
	[JsonProperty("currency")]
    public string Currency { get; set; }
}