namespace VkNet.Enums.SafetyEnums;

/// <summary>
///     Тип объекта (исп. в категории Likes)
/// </summary>
public class LikeObjectType : SafetyEnum<LikeObjectType>
{
	/// <summary>
	///     Запись на стене пользователя или группы
	/// </summary>
	public static readonly LikeObjectType Post = RegisterPossibleValue("post");

	/// <summary>
	///     Комментарий к записи на стене
	/// </summary>
	public static readonly LikeObjectType Comment = RegisterPossibleValue("comment");

	/// <summary>
	///     Фотография
	/// </summary>
	public static readonly LikeObjectType Photo = RegisterPossibleValue("photo");

	/// <summary>
	///     Аудиозапись
	/// </summary>
	public static readonly LikeObjectType Audio = RegisterPossibleValue("audio");

	/// <summary>
	///     Видеозапись
	/// </summary>
	public static readonly LikeObjectType Video = RegisterPossibleValue("video");

	/// <summary>
	///     Заметка
	/// </summary>
	public static readonly LikeObjectType Note = RegisterPossibleValue("note");

	/// <summary>
	///     Комментарий к фотографии
	/// </summary>
	public static readonly LikeObjectType PhotoComment = RegisterPossibleValue("photo_comment");

	/// <summary>
	///     Комментарий к видеозаписи
	/// </summary>
	public static readonly LikeObjectType VideoComment = RegisterPossibleValue("video_comment");

	/// <summary>
	///     Комментарий в обсуждении
	/// </summary>
	public static readonly LikeObjectType TopicComment = RegisterPossibleValue("topic_comment");

	/// <summary>
	///     Страница сайта, на котором установлен виджет «Мне нравится»
	/// </summary>
	public static readonly LikeObjectType SitePage = RegisterPossibleValue("sitepage");

	/// <summary>
	///     Товар
	/// </summary>
	public static readonly LikeObjectType Market = RegisterPossibleValue("market");

	/// <summary>
	///     Комментарий к товару.
	/// </summary>
	public static readonly LikeObjectType MarketComment = RegisterPossibleValue("market_comment");
}