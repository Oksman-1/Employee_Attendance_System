using System.Text.Json.Serialization;

namespace Attendance.Shared.GenericResponse;

public class GenericResponse<T>
{
    [JsonPropertyName("responseCode")]
    public string ResponseCode { get; set; } = string.Empty;

    [JsonPropertyName("responseMessage")]
    public string ResponseMessage { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    // SUCCESS RESPONSE
    public static GenericResponse<T> Success(string message, T data, string responseCode = "200")
    {
        return new GenericResponse<T>
        {
            ResponseCode = responseCode,
            ResponseMessage = message,
            Data = data
        };
    }

    // BAD REQUEST - Invalid input
    public static GenericResponse<T> BadRequest(string message, T? data = default)
    {
        return new GenericResponse<T>
        {
            ResponseCode = "400",
            ResponseMessage = message,
            Data = data
        };
    }

    // UNAUTHORIZED
    public static GenericResponse<T> Unauthorized(string message, T? data = default)
    {
        return new GenericResponse<T>
        {
            ResponseCode = "401",
            ResponseMessage = message,
            Data = data
        };
    }

    // NOT FOUND
    public static GenericResponse<T> NotFound(string message, T? data = default)
    {
        return new GenericResponse<T>
        {
            ResponseCode = "404",
            ResponseMessage = message,
            Data = data
        };
    }

    // DUPLICATE ENTRY
    public static GenericResponse<T> Duplicate(string message, T? data = default)
    {
        return new GenericResponse<T>
        {
            ResponseCode = "409",
            ResponseMessage = message,
            Data = data
        };
    }

    // INTERNAL SERVER ERROR
    public static GenericResponse<T> InternalError(string message, T? data = default)
    {
        return new GenericResponse<T>
        {
            ResponseCode = "500",
            ResponseMessage = message,
            Data = data
        };
    }
}