namespace VkNet.Enums.SafetyEnums;

/// <summary>
///     Тип статуса ссылки
/// </summary>
public class LinkStatusType : SafetyEnum<LinkStatusType>
{
	/// <summary>
	///     Ссылку допустимо использовать в рекламных объявлениях;
	/// </summary>
	public static readonly LinkStatusType Allowed = RegisterPossibleValue("allowed");

	/// <summary>
	///     Ссылку допустимо использовать в рекламных объявлениях;
	/// </summary>
	public static readonly LinkStatusType Disallowed = RegisterPossibleValue("disallowed");

	/// <summary>
	///     Ссылку допустимо использовать в рекламных объявлениях;
	/// </summary>
	public static readonly LinkStatusType InProgress = RegisterPossibleValue("in_progress");
}