﻿using System;
using Newtonsoft.Json;
using VkNet.Enums.SafetyEnums;
using VkNet.Utils.JsonConverter;

namespace VkNet.Model.RequestParams;

/// <summary>
///     Параметры запроса LinkStats
/// </summary>
[Serializable]
public class LinkStatsParams
{
	/// <summary>
	///     Сокращенная ссылка (часть URL после "vk.cc/").
	/// </summary>
	[JsonProperty("key")]
    public string Key { get; set; }

	/// <summary>
	///     Ключ доступа к приватной статистике ссылки.
	/// </summary>
	[JsonProperty("access_key")]
    public string AccessKey { get; set; }

	/// <summary>
	///     Единица времени для подсчета статистики.
	/// </summary>
	[JsonProperty("interval")]
    [JsonConverter(typeof(SafetyEnumJsonConverter))]
    public LinkStatInterval Interval { get; set; }

	/// <summary>
	///     Длительность периода для получения статистики в выбранных единицах (из
	///     параметра interval).
	/// </summary>
	[JsonProperty("intervals_count")]
    public uint IntervalsCount { get; set; }

	/// <summary>
	///     1 — возвращать расширенную статистику (пол/возраст/страна/город),
	///     0 — возвращать только количество переходов.
	/// </summary>
	[JsonProperty("extended")]
    public bool? Extended { get; set; }
}