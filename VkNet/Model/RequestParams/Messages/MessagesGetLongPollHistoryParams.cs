using System;
using Newtonsoft.Json;
using VkNet.Enums.Filters;

namespace VkNet.Model.RequestParams;

/// <summary>
///     Параметры метода messages.getLongPollHistoryParams
/// </summary>
[Serializable]
public class MessagesGetLongPollHistoryParams
{
	/// <summary>
	///     Лимит ивентов.
	/// </summary>
	[JsonIgnore] public const long EVENTS_LIMIT_MIN = 1000;

    [JsonIgnore] private long? _eventsLimit;

    /// <summary>
    ///     Список дополнительных полей профилей, которые необходимо вернуть.
    /// </summary>
    [JsonProperty("fields")]
    public UsersFields Fields { get; set; }

    /// <summary>
    ///     Последнее значение параметра ts, полученное от Long Poll сервера или с помощью
    ///     метода messages.getLongPollServer
    /// </summary>
    [JsonProperty("ts")]
    public ulong Ts { get; set; }

    /// <summary>
    ///     Последнее значение параметра new_pts, полученное от Long Poll сервера,
    ///     используется для получения действий, которые
    ///     хранятся всегда.
    /// </summary>
    [JsonProperty("pts")]
    public ulong? Pts { get; set; }

    /// <summary>
    ///     Количество символов, по которому нужно обрезать сообщение. Укажите 0, если Вы
    ///     не хотите обрезать сообщение. (по
    ///     умолчанию сообщения не обрезаются).
    /// </summary>
    [JsonProperty("preview_length")]
    public long? PreviewLength { get; set; }

    /// <summary>
    ///     При передаче в этот параметра значения 1 будет возвращена история только от тех
    ///     пользователей, которые сейчас
    ///     online. флаг, может принимать значения 1 или 0.
    /// </summary>
    [JsonProperty("onlines")]
    public bool? Onlines { get; set; }

    /// <summary>
    ///     Если количество событий в истории превысит это значение, будет возвращена
    ///     ошибка.
    ///     Положительное число. По умолчанию - 1000. Минимальное значение - 1000
    /// </summary>
    [JsonProperty("events_limit")]
    public long? EventsLimit
    {
        get => _eventsLimit;
        set => _eventsLimit = !value.HasValue || value <= EVENTS_LIMIT_MIN ? value : EVENTS_LIMIT_MIN;
    }

    /// <summary>
    ///     Количество сообщений, которое нужно вернуть.
    /// </summary>
    [JsonProperty("msgs_limit")]
    public long? MsgsLimit { get; set; }

    /// <summary>
    ///     Максимальный идентификатор сообщения среди уже имеющихся в локальной копии.
    ///     Необходимо учитывать как сообщения,
    ///     полученные через методы API (например messages.getDialogs,
    ///     messages.getHistory), так и данные, полученные из Long
    ///     Poll сервера (события с кодом 4).
    /// </summary>
    [JsonProperty("max_msg_id")]
    public long? MaxMsgId { get; set; }

    /// <summary>
    ///     Идентификатор сообщества (для сообщений сообщества с ключом доступа пользователя).
    /// </summary>
    [JsonProperty("group_id")]
    public ulong? GroupId { get; set; }

    /// <summary>
    ///     Версия Long Poll.
    /// </summary>
    [JsonProperty("lp_version")]
    public ulong? LpVersion { get; set; }
}