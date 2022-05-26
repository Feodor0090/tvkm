using System;
using Newtonsoft.Json;
using VkNet.Enums.Filters;

namespace VkNet.Model.RequestParams;

/// <summary>
///     Параметры запроса Search.GetHints
/// </summary>
[Serializable]
public class SearchGetHintsParams
{
	/// <summary>
	///     текст запроса, результаты которого нужно получить
	/// </summary>
	[JsonProperty("q")]
    public string Query { get; set; }

	/// <summary>
	///     смещение для выборки определённого подмножества результатов.
	/// </summary>
	[JsonProperty("offset")]
    public uint Offset { get; set; }

	/// <summary>
	///     ограничение на количество возвращаемых результатов.
	/// </summary>
	[JsonProperty("limit")]
    public uint Limit { get; set; }

	/// <summary>
	///     Перечисленные через запятую типы данных, которые необходимо вернуть. По
	///     умолчанию возвращаются все.
	/// </summary>
	[JsonProperty("filters")]
    public SearchFilter Filters { get; set; }

	/// <summary>
	///     дополнительные поля профилей и сообществ для получения.
	/// </summary>
	[JsonProperty("fields")]
    public ProfileFields ProfileFields { get; set; }

	/// <summary>
	///     1 — к результатам поиска добавляются результаты глобального поиска по всем
	///     пользователям и группам.
	/// </summary>
	[JsonProperty("search_global")]
    public bool SearchGlobal { get; set; } = true;
}