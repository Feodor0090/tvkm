namespace VkNet.Enums.Filters;

/// <summary>
///     Определяет, какие типы объектов необходимо получить.
/// </summary>
public sealed class AudioBroadcastFilter : MultivaluedFilter<AudioBroadcastFilter>
{
	/// <summary>
	///     Только друзья.
	/// </summary>
	public static readonly AudioBroadcastFilter Friends = RegisterPossibleValue("friends");

	/// <summary>
	///     Только сообщества.
	/// </summary>
	public static readonly AudioBroadcastFilter Groups = RegisterPossibleValue("groups");

	/// <summary>
	///     Друзья и сообщества.
	/// </summary>
	public static readonly AudioBroadcastFilter All = RegisterPossibleValue("all");
}