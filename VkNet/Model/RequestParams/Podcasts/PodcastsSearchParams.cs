using System;
using Newtonsoft.Json;

namespace VkNet.Model.RequestParams;

/// <summary>
///     Параметры метода Podcasts.search
/// </summary>
[Serializable]
public class PodcastsSearchParams
{
	/// <summary>
	///     Поисковый запрос.
	/// </summary>
	[JsonProperty("search_string")]
    public string SearchString { get; set; }

	/// <summary>
	///     Смещение, необходимое для выборки определённого подмножества результатов поиска.
	///     <remarks>
	///         По умолчанию — 0.
	///     </remarks>
	/// </summary>
	[JsonProperty("offset")]
    public int? Offset { get; set; }

	/// <summary>
	///     Количество результатов поиска, которое необходимо вернуть.
	/// </summary>
	[JsonProperty("count")]
    public int? Count { get; set; }
}