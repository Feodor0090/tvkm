using System;
using Newtonsoft.Json;
using VkNet.Utils;

namespace VkNet.Model;

/// <summary>
///     Subscriptions
/// </summary>
[Serializable]
public class SubscriptionsInfo
{
	/// <summary>
	///     Массив объектов подписок.
	/// </summary>
	[JsonProperty("subscritions")]
    public VkCollection<Subscription> Subscriptions { get; set; }

	/// <summary>
	///     Количество подписок.
	/// </summary>
	[JsonProperty("count")]
    public long Count { get; set; }

	/// <summary>
	///     Массив объектов пользователей.
	/// </summary>
	[JsonProperty("profiles")]
    public VkCollection<User> Profiles { get; set; }

	/// <summary>
	///     Массив объектов сообществ.
	/// </summary>
	[JsonProperty("groups")]
    public VkCollection<Group> Groups { get; set; }

    public static SubscriptionsInfo FromJson(VkResponse response)
    {
        var subscriptions = new SubscriptionsInfo
        {
            Subscriptions = response["subscritions"].ToVkCollectionOf<Subscription>(r => r),
            Count = response["count"],
            Profiles = response["profiles"].ToVkCollectionOf<User>(r => r),
            Groups = response["groups"].ToVkCollectionOf<Group>(r => r)
        };

        return subscriptions;
    }
}