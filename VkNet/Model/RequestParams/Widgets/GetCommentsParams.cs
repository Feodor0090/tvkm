using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VkNet.Model.RequestParams;

/// <summary>
///     Параметры запроса widgets.getComments
/// </summary>
[Serializable]
public class GetCommentsParams
{
	/// <summary>
	///     Идентификатор приложения/сайта, с которым инициализируются виджеты.
	/// </summary>
	[JsonProperty("widget_api_id")]
    public long WidgetApiId { get; set; }

	/// <summary>
	///     URL-адрес страницы.
	/// </summary>
	[JsonProperty("url")]
    public string Url { get; set; }

	/// <summary>
	///     Внутренний идентификатор страницы в приложении/сайте
	///     (в случае, если для инициализации виджетов использовался параметр page_id).
	/// </summary>
	[JsonProperty("page_id")]
    public string PageId { get; set; }

	/// <summary>
	///     Тип сортировки комментариев. Возможные значения: date, likes, last_comment.
	///     Значение по умолчанию - date.
	/// </summary>
	[JsonProperty("order")]
    public string Order { get; set; }

	/// <summary>
	///     Перечисленные через запятую поля анкет необходимые для получения.
	///     Если среди полей присутствует replies,
	///     будут возвращены последние комментарии второго уровня для каждого комментария
	///     первого уровня.
	/// </summary>
	[JsonProperty("fields")]
    public IEnumerable<string> Fields { get; set; }

	/// <summary>
	///     Смещение необходимое для выборки определенного подмножества комментариев. По
	///     умолчанию 0.
	/// </summary>
	[JsonProperty("offset")]
    public ulong Offset { get; set; }

	/// <summary>
	///     Количество возвращаемых записей.
	/// </summary>
	[JsonProperty("count")]
    public ulong Count { get; set; }
}