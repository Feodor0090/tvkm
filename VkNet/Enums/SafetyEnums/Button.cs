namespace VkNet.Enums.SafetyEnums;

/// <summary>
///     Кнопка для карусели
/// </summary>
public class Button : SafetyEnum<Button>
{
	/// <summary>
	///     Текст: Запустить
	///     Действие: Переход по ссылке
	///     Ссылка: Приложения и игры
	/// </summary>
	public static readonly Button AppJoin = RegisterPossibleValue("app_join");

	/// <summary>
	///     Текст: Играть
	///     Действие: Переход по ссылке
	///     Ссылка: Игры
	/// </summary>
	public static readonly Button AppGameJoin = RegisterPossibleValue("app_game_join");

	/// <summary>
	///     Текст: Перейти
	///     Действие: Переход по ссылке
	///     Ссылка: Внешние сайты, сообщества, приложения
	/// </summary>
	public static readonly Button OpenUrl = RegisterPossibleValue("open_url");

	/// <summary>
	///     Текст: Открыть
	///     Действие: Переход по ссылке
	///     Ссылка: Внешние сайты
	/// </summary>
	public static readonly Button Open = RegisterPossibleValue("open");

	/// <summary>
	///     Текст: Позвонить
	///     Действие: Набор номера
	///     Ссылка: Телефонные номера
	/// </summary>
	public static readonly Button Call = RegisterPossibleValue("call");

	/// <summary>
	///     Текст: Забронировать
	///     Действие: Набор номера
	///     Ссылка: Телефонные номера
	/// </summary>
	public static readonly Button Book = RegisterPossibleValue("book");

	/// <summary>
	///     Текст: Записаться
	///     Действие: Переход по ссылке или набор номера
	///     Ссылка: Внешние сайты, телефонные номера
	/// </summary>
	public static readonly Button Enroll = RegisterPossibleValue("enroll");

	/// <summary>
	///     Текст: Зарегистрироваться
	///     Действие: Набор номера
	///     Ссылка: Телефонные номера
	/// </summary>
	public static readonly Button Register = RegisterPossibleValue("register");

	/// <summary>
	///     Текст: Купить
	///     Действие: Переход по ссылке
	///     Ссылка: Внешние сайты
	/// </summary>
	public static readonly Button Buy = RegisterPossibleValue("buy");

	/// <summary>
	///     Текст: Купить билет
	///     Действие: Переход по ссылке
	///     Ссылка: Внешние сайты
	/// </summary>
	public static readonly Button BuyTicket = RegisterPossibleValue("buy_ticket");

	/// <summary>
	///     Текст: В магазин
	///     Действие: Переход по ссылке
	///     Ссылка: Внешние сайты
	/// </summary>
	public static readonly Button ToShop = RegisterPossibleValue("to_shop");

	/// <summary>
	///     Текст: Заказать
	///     Действие: Переход по ссылке
	///     Ссылка: Внешние сайты
	/// </summary>
	public static readonly Button Order = RegisterPossibleValue("order");

	/// <summary>
	///     Текст: Создать
	///     Действие: Переход по ссылке
	///     Ссылка: Внешние сайты
	/// </summary>
	public static readonly Button Create = RegisterPossibleValue("create");

	/// <summary>
	///     Текст: Установить
	///     Действие: Переход по ссылке
	///     Ссылка: Внешние сайты
	/// </summary>
	public static readonly Button Install = RegisterPossibleValue("install");

	/// <summary>
	///     Текст: Связаться
	///     Действие: Переход по ссылке
	///     Ссылка: Внешние сайты
	/// </summary>
	public static readonly Button Contact = RegisterPossibleValue("contact");

	/// <summary>
	///     Текст: Заполнить
	///     Действие: Переход по ссылке
	///     Ссылка: Внешние сайты
	/// </summary>
	public static readonly Button Fill = RegisterPossibleValue("fill");

	/// <summary>
	///     Текст: Выбрать
	///     Действие: Переход по ссылке
	///     Ссылка: Внешние сайты
	/// </summary>
	public static readonly Button Choose = RegisterPossibleValue("choose");

	/// <summary>
	///     Текст: Попробовать
	///     Действие: Переход по ссылке
	///     Ссылка: Внешние сайты
	/// </summary>
	public static readonly Button Try = RegisterPossibleValue("try");

	/// <summary>
	///     Текст: Подписаться
	///     Действие: Подписка на публичную страницу
	///     Ссылка: Публичные страницы
	/// </summary>
	public static readonly Button JoinPublic = RegisterPossibleValue("join_public");

	/// <summary>
	///     Текст: Я пойду
	///     Действие: Участие в мероприятии
	///     Ссылка: События
	/// </summary>
	public static readonly Button JoinEvent = RegisterPossibleValue("join_event");

	/// <summary>
	///     Текст: Вступить
	///     Действие: Вступление в сообщество
	///     Ссылка: Сообщества
	/// </summary>
	public static readonly Button JoinGroup = RegisterPossibleValue("join_group");

	/// <summary>
	///     Текст: Связаться
	///     Действие: Переход к диалогу с сообществом
	///     Ссылка: Сообщества, публичные страницы, события
	/// </summary>
	public static readonly Button ImGroup = RegisterPossibleValue("im_group");

	/// <summary>
	///     Текст: Написать
	///     Действие: Переход к диалогу с сообществом
	///     Ссылка: Сообщества, публичные страницы, события
	/// </summary>
	public static readonly Button ImGroup2 = RegisterPossibleValue("im_group2");

	/// <summary>
	///     Текст: Начать
	///     Действие: Переход по ссылке
	///     Ссылка: Внешние сайты
	/// </summary>
	public static readonly Button Begin = RegisterPossibleValue("begin");

	/// <summary>
	///     Текст: Получить
	///     Действие: Переход по ссылке
	///     Ссылка: Внешние сайты
	/// </summary>
	public static readonly Button Get = RegisterPossibleValue("get");
}