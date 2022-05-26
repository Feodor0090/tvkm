using System;
using Newtonsoft.Json;
using VkNet.Enums.SafetyEnums;
using VkNet.Utils.JsonConverter;

namespace VkNet.Model.RequestParams.Ads;

/// <summary>
/// </summary>
[Serializable]
public class CheckLinkParams
{
	/// <summary>
	/// </summary>
	[JsonProperty("account_id")]
    public long AccountId { get; set; }

	/// <summary>
	/// </summary>
	[JsonProperty("link_type")]
    [JsonConverter(typeof(SafetyEnumJsonConverter))]
    public AdsLinkType LinkType { get; set; }

	/// <summary>
	/// </summary>
	[JsonProperty("link_url")]
    public Uri LinkUrl { get; set; }

	/// <summary>
	/// </summary>
	[JsonProperty("campaign_id")]
    public long? CampaignId { get; set; }
}