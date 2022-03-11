namespace TimCodes.ApiAbstractions.Serialization;

/// <summary>
/// Represents a class responsible for deserializing API responses
/// </summary>
public interface IApiResponseDeserializer
{
    Task<IApiResponse> DeserializeAsync<TContent>(object rawApiResponse);
}
