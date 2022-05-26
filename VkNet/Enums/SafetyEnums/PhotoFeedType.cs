namespace VkNet.Enums.SafetyEnums;

/// <summary>
///     Тип канала новостей.
/// </summary>
public sealed class PhotoFeedType : SafetyEnum<PhotoFeedType>
{
	/// <summary>
	///     Фото.
	/// </summary>
	public static readonly PhotoFeedType Photo = RegisterPossibleValue("photo");

	/// <summary>
	///     Тег фото.
	/// </summary>
	public static readonly PhotoFeedType PhotoTag = RegisterPossibleValue("photo_tag");
}