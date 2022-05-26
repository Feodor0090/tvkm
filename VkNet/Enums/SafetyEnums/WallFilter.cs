using VkNet.Utils;

namespace VkNet.Enums.SafetyEnums;

/// <summary>
///     Фильтр для задания типов сообщений, которые необходимо получить со стены.
/// </summary>
public sealed class WallFilter : SafetyEnum<WallFilter>
{
	/// <summary>
	///     Необходимо получить сообщения на стене только от ее владельца.
	/// </summary>
	public static readonly WallFilter Owner = RegisterPossibleValue("owner");

	/// <summary>
	///     Необходимо получить сообщения на стене не от владельца стены.
	/// </summary>
	public static readonly WallFilter Others = RegisterPossibleValue("others");

	/// <summary>
	///     Необходимо получить все сообщения на стене (Owner + Others).
	/// </summary>
	[DefaultValue] public static readonly WallFilter All = RegisterPossibleValue("all");

	/// <summary>
	///     Предложенные записи на стене сообщества
	/// </summary>
	public static readonly WallFilter Suggests = RegisterPossibleValue("suggests");

	/// <summary>
	///     Отложенные записи
	/// </summary>
	public static readonly WallFilter Postponed = RegisterPossibleValue("postponed");
}