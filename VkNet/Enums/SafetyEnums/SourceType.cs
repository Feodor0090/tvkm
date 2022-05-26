namespace VkNet.Enums.SafetyEnums;

/// <summary>
///     Тип источника исходной аудитории.
/// </summary>
public sealed class SourceType : SafetyEnum<SourceType>
{
	/// <summary>
	///     Аудитория ретаргетинга
	/// </summary>
	public static readonly SourceType RetargetingGroup = RegisterPossibleValue("retargeting_group");
}