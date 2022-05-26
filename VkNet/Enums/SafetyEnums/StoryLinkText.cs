using System;
using VkNet.Utils;

namespace VkNet.Enums.SafetyEnums;

/// <summary>
///     Текст ссылки для перехода из истории (только для историй сообществ).
/// </summary>
[Serializable]
public sealed class StoryLinkText : SafetyEnum<StoryLinkText>
{
	/// <summary>
	///     В магазин
	/// </summary>
	public static readonly StoryLinkText ToStore = RegisterPossibleValue("to_store");

	/// <summary>
	///     Голосовать
	/// </summary>
	public static readonly StoryLinkText Vote = RegisterPossibleValue("vote");

	/// <summary>
	///     Ещё
	/// </summary>
	public static readonly StoryLinkText More = RegisterPossibleValue("more");

	/// <summary>
	///     Забронировать
	/// </summary>
	public static readonly StoryLinkText Book = RegisterPossibleValue("book");

	/// <summary>
	///     Заказать
	/// </summary>
	public static readonly StoryLinkText Order = RegisterPossibleValue("order");

	/// <summary>
	///     Записаться
	/// </summary>
	public static readonly StoryLinkText Enroll = RegisterPossibleValue("enroll");

	/// <summary>
	///     Заполнить
	/// </summary>
	public static readonly StoryLinkText Fill = RegisterPossibleValue("fill");

	/// <summary>
	///     Зарегистрироваться
	/// </summary>
	public static readonly StoryLinkText Signup = RegisterPossibleValue("signup");

	/// <summary>
	///     Купить
	/// </summary>
	public static readonly StoryLinkText Buy = RegisterPossibleValue("buy");

	/// <summary>
	///     Купить билет
	/// </summary>
	public static readonly StoryLinkText Ticket = RegisterPossibleValue("ticket");

	/// <summary>
	///     Написать
	/// </summary>
	public static readonly StoryLinkText Write = RegisterPossibleValue("write");

	/// <summary>
	///     Открыть
	/// </summary>
	public static readonly StoryLinkText Open = RegisterPossibleValue("open");

	/// <summary>
	///     Подробнее
	/// </summary>
	[DefaultValue] public static readonly StoryLinkText LearnMore = RegisterPossibleValue("learn_more");

	/// <summary>
	///     Посмотреть
	/// </summary>
	public static readonly StoryLinkText View = RegisterPossibleValue("view");

	/// <summary>
	///     Перейти
	/// </summary>
	public static readonly StoryLinkText GoTo = RegisterPossibleValue("go_to");

	/// <summary>
	///     Связаться
	/// </summary>
	public static readonly StoryLinkText Contact = RegisterPossibleValue("contact");

	/// <summary>
	///     Смотреть
	/// </summary>
	public static readonly StoryLinkText Watch = RegisterPossibleValue("watch");

	/// <summary>
	///     Слушать
	/// </summary>
	public static readonly StoryLinkText Play = RegisterPossibleValue("play");

	/// <summary>
	///     Установить
	/// </summary>
	public static readonly StoryLinkText Install = RegisterPossibleValue("install");

	/// <summary>
	///     Читать
	/// </summary>
	public static readonly StoryLinkText Read = RegisterPossibleValue("read");
}