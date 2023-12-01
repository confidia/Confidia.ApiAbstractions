namespace Confidia.ApiAbstractions.Models.Responses;

/// <summary>
/// Can be used as a basis for responses from custom APIs
/// </summary>
public class ApiMessageBase : IApiMessage
{
    public bool IsSuccess { get; set; }

    public string? Message { get; set; }

    public int? ErrorCode { get; set; }

    public Dictionary<string, string[]>? ValidationErrors { get; set; }
}
