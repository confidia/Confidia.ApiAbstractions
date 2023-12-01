namespace TimCodes.ApiAbstractions.Serialization;

public class EmptyHttpApiResponseDeserializer(ILogger<EmptyHttpApiResponseDeserializer> logger) : IApiResponseDeserializer
{
    private readonly ILogger<EmptyHttpApiResponseDeserializer> _logger = logger;

    public Task<IApiResponse> DeserializeAsync<TContent>(object rawApiResponse)
    {
        _logger.LogDebug("Response deserialized with empty http deserializer");
        return Task.FromResult<IApiResponse>(new HttpApiEmptyResponse((HttpResponseMessage)rawApiResponse));
    }
}
