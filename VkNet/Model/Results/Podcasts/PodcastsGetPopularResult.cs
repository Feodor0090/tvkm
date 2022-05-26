using System;
using Newtonsoft.Json;

namespace VkNet.Model;

/// <summary>
///     Результат метода Podcasts.getPopular
/// </summary>
[Serializable]
public class PodcastsGetPopularResult
{
	/// <summary>
	///     Владелец подкаста.
	/// </summary>
	[JsonProperty("owner_id")]
    public long OwnerId { get; set; }

	/// <summary>
	///     Название подкаста.
	/// </summary>
	[JsonProperty("owner_title")]
    public string OwnerTitle { get; set; }

	/// <summary>
	///     Короткое имя группы подкаста.
	/// </summary>
	[JsonProperty("url")]
    public string Url { get; set; }
}