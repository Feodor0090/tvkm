using System;
using VkNet.Enums.SafetyEnums;

namespace VkNet.Model;

/// <summary>
///     Является опциональным и содержит следующие данные в зависимости от значения
///     поля type:
/// </summary>
[Serializable]
public class PostSourceData : SafetyEnum<PostSourceData>
{
	/// <summary>
	///     Изменение статуса под именем пользователя.
	/// </summary>
	public static readonly PostSourceData ProfileActivity = RegisterPossibleValue("profile_activity");

	/// <summary>
	///     Изменение профильной фотографии пользователя.
	/// </summary>
	public static readonly PostSourceData ProfilePhoto = RegisterPossibleValue("profile_photo");

	/// <summary>
	///     Виджет комментариев.
	/// </summary>
	public static readonly PostSourceData Comments = RegisterPossibleValue("comments");

	/// <summary>
	///     Виджет «Мне нравится».
	/// </summary>
	public static readonly PostSourceData Like = RegisterPossibleValue("like");

	/// <summary>
	///     Виджет опросов.
	/// </summary>
	public static readonly PostSourceData Poll = RegisterPossibleValue("poll");
}