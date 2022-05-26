using VkNet.Utils;

namespace VkNet.Enums.SafetyEnums;

/// <summary>
///     Цвет кнопки клавиатуры отправляемой ботом.
/// </summary>
public sealed class KeyboardButtonColor : SafetyEnum<KeyboardButtonColor>
{
	/// <summary>
	///     обычная белая кнопка. #FFFFFF
	/// </summary>
	[DefaultValue] public static readonly KeyboardButtonColor Default = RegisterPossibleValue("default");

	/// <summary>
	///     синяя кнопка, обозначает основное действие. #5181B8
	/// </summary>
	public static readonly KeyboardButtonColor Primary = RegisterPossibleValue("primary");

	/// <summary>
	///     опасное действие, или отрицательное действие (отклонить, удалить и тд). #E64646
	/// </summary>
	public static readonly KeyboardButtonColor Negative = RegisterPossibleValue("negative");

	/// <summary>
	///     согласиться, подтвердить. #4BB34B
	/// </summary>
	public static readonly KeyboardButtonColor Positive = RegisterPossibleValue("positive");
}