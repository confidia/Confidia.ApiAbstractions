namespace TimCodes.ApiAbstractions.Serialization;

public class EmptyApiResponseDeserializer : IApiResponseDeserializer
{
    private readonly ILogger<EmptyApiResponseDeserializer> _logger;

    public EmptyApiResponseDeserializer(ILogger<EmptyApiResponseDeserializer> logger)
    {
        _logger = logger;
    }

    public Task<IApiResponse> DeserializeAsync<TContent>(object rawApiResponse)
    {
        _logger.LogDebug("Response deserialized with empty deserializer");
        return Task.FromResult<IApiResponse>(new EmptyApiResponse());
    }
}
