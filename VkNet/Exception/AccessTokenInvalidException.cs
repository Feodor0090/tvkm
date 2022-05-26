using System;
using VkNet.Utils;

namespace VkNet.Exception;

/// <summary>
///     Исключение, которое выбрасывается, в случае, если предоставленный маркер
///     доступа является недействительным.
/// </summary>
[Serializable]
public class AccessTokenInvalidException : VkApiException
{
	/// <summary>
	///     Инициализирует новый экземпляр класса AccessTokenInvalidException
	/// </summary>
	public AccessTokenInvalidException()
    {
    }

	/// <summary>
	///     Инициализирует новый экземпляр класса AccessTokenInvalidException
	/// </summary>
	/// <param name="message"> Описание исключения. </param>
	public AccessTokenInvalidException(string message) : base(message)
    {
    }

	/// <summary>
	///     Инициализирует новый экземпляр класса AccessTokenInvalidException
	/// </summary>
	/// <param name="message"> Описание исключения. </param>
	/// <param name="innerException"> Внутреннее исключение. </param>
	public AccessTokenInvalidException(string message, System.Exception innerException) : base(message
        , innerException)
    {
    }

	/// <summary>
	///     Инициализирует новый экземпляр класса VkApiException
	/// </summary>
	/// <param name="response"> Ответ от сервера vk </param>
	public AccessTokenInvalidException(VkResponse response) : base(response["error_msg"])
    {
        ErrorCode = response["error_code"];
    }
}