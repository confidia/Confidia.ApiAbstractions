namespace Confidia.ApiAbstractions.Models.Responses;

/// <summary>
/// Represents a response from an API
/// </summary>
public interface IApiResponse : IDisposable
{
    public bool Success { get; }
}
