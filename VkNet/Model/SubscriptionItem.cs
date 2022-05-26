using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VkNet.Enums;

namespace VkNet.Model;

/// <summary>
///     Элемент подписки
/// </summary>
[Serializable]
public class SubscriptionItem
{
    private DateTime? _nextBillTime;

    /// <summary>
    ///     Идентификатор подписки
    /// </summary>
    [JsonProperty("id")]
    public ulong Id { get; set; }

    /// <summary>
    ///     Идентификатор товара в приложении
    /// </summary>
    [JsonProperty("item_id")]
    public string ItemId { get; set; }

    /// <summary>
    ///     Статус подписки. Возможные значения:
    ///     active — подписка активна.
    /// </summary>
    [JsonProperty("status")]
    public SubscriptionStatus Status { get; set; }

    /// <summary>
    ///     Стоимость подписки
    /// </summary>
    [JsonProperty("price")]
    public long Price { get; set; }

    /// <summary>
    ///     Период подписки
    /// </summary>
    [JsonProperty("period")]
    public int Period { get; set; }

    /// <summary>
    ///     Дата создания в Unixtime
    /// </summary>
    [JsonProperty("create_time")]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? CreateTime { get; set; }

    /// <summary>
    ///     Дата обновления в Unixtime
    /// </summary>
    [JsonProperty("update_time")]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    ///     Дата начала периода в Unixtime
    /// </summary>
    [JsonProperty("period_start_time")]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? PeriodStartTime { get; set; }

    /// <summary>
    ///     Дата следующего платежа в Unixtime (если status = active)
    /// </summary>
    [JsonProperty("next_bill_time")]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? NextBillTime
    {
        get
        {
            if (Status.Equals("active")) return _nextBillTime;
            return null;
        }
        set
        {
            if (_nextBillTime == value) return;
            if (Status.Equals("active"))
                _nextBillTime = value;
        }
    }

    /// <summary>
    ///     Дата истечения триал-периода (если есть)
    /// </summary>
    [JsonProperty("trial_expire_time")]
    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime? TrialExpireTime { get; set; }

    /// <summary>
    ///     true, если подписка ожидает отмены.
    /// </summary>
    [JsonProperty("pending_cancel")]
    public bool PendingCancel { get; set; }

    /// <summary>
    ///     Причина отмены (если есть). Возможные значения:
    ///     user_decision — по инициативе пользователя;
    ///     app_decision — по инициативе приложения;
    ///     payment_fail — из-за проблемы с платежом;
    ///     unknown — причина неизвестна.
    /// </summary>
    [JsonProperty("cancel_reason")]
    public CancelSubscriptionReason CancelReason { get; set; }

    /// <summary>
    ///     true, если используется тестовый режим.
    /// </summary>
    [JsonProperty("test_mode")]
    public bool TestMode { get; set; }
}