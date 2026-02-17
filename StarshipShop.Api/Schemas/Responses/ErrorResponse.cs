using System.Text.Json;
using FluentValidation.Results;

namespace StarshipShop.Api.Schemas.Responses;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = null!;
    public Dictionary<string, string[]>? Errors { get; set; }

    public static ErrorResponse FromException(Exception exception, int statusCode)
    {
        return new ErrorResponse
        {
            StatusCode = statusCode,
            Message = exception.Message
        };
    }

    public static ErrorResponse FromValidation(IEnumerable<ValidationFailure> failures)
    {
        var errors = failures
            .GroupBy(f => ToCamelCase(f.PropertyName))
            .ToDictionary(
                g => g.Key,
                g => g.Select(f => f.ErrorMessage).ToArray()
            );

        return new ErrorResponse
        {
            StatusCode = 400,
            Message = "Validation failed",
            Errors = errors
        };
    }

    private static string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
            return str;
        
        return char.ToLowerInvariant(str[0]) + str.Substring(1);
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
}
