using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VkNet.Utils;

// ReSharper disable UnusedAutoPropertyAccessor.Global

// ReSharper disable MemberCanBePrivate.Global

namespace VkNet.Model;

/// <summary>
///     Обновления в личных сообщениях пользователя.
/// </summary>
[Serializable]
public class LongPollHistoryResponse
{
	/// <summary>
	///     Обновления в личных сообщениях пользователя.
	/// </summary>
	public LongPollHistoryResponse()
    {
        History = new List<ReadOnlyCollection<long>>();
    }

	/// <summary>
	///     История.
	/// </summary>

	// ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public List<ReadOnlyCollection<long>> History { get; set; }

	/// <summary>
	///     Количество непрочитанных сообщений
	/// </summary>
	public ulong UnreadMessages { get; set; }

	/// <summary>
	///     Колекция сообщений.
	/// </summary>
	public ReadOnlyCollection<Message> Messages { get; set; }

	/// <summary>
	///     Колекция профилей.
	/// </summary>
	public ReadOnlyCollection<User> Profiles { get; set; }

	/// <summary>
	///     Колекция профилей.
	/// </summary>
	public ReadOnlyCollection<Group> Groups { get; set; }

	/// <summary>
	///     Последнее значение параметра new_pts, полученное от Long Poll сервера,
	///     используется для получения действий, которые
	///     хранятся всегда.
	/// </summary>
	public ulong NewPts { get; set; }

	/// <summary>
	///     Если true — это означает, что нужно запросить оставшиеся данные с помощью
	///     запроса с параметром max_msg_id
	/// </summary>
	public bool More { get; set; }

	/// <summary>
	///     Разобрать из json.
	/// </summary>
	/// <param name="response"> Ответ сервера. </param>
	/// <returns> </returns>
	public static LongPollHistoryResponse FromJson(VkResponse response)
    {
        var fromJson = new LongPollHistoryResponse
        {
            UnreadMessages = response["messages"]["count"],
            Messages = response["messages"]["items"].ToReadOnlyCollectionOf<Message>(x => x),
            Profiles = response["profiles"].ToReadOnlyCollectionOf<User>(x => x),
            Groups = response["groups"].ToReadOnlyCollectionOf<Group>(x => x),
            NewPts = response["new_pts"],
            More = response["more"]
        };

        VkResponseArray histories = response["history"];

        foreach (var history in histories)
        {
            VkResponseArray item = history;
            fromJson.History.Add(new ReadOnlyCollection<long>(item.ToReadOnlyCollectionOf<long>(x => x)));
        }

        return fromJson;
    }
}