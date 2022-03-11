namespace TimCodes.ApiAbstractions.Http.Serialization;

public class StreamHttpApiResponseDeserializer : HttpApiResponseDeserializerBase
{
    private readonly ILogger<StreamHttpApiResponseDeserializer> _logger;

    public StreamHttpApiResponseDeserializer(ILogger<StreamHttpApiResponseDeserializer> logger)
    {
        _logger = logger;
    }

    public override async Task<IApiResponse> DeserializeAsync<TContent>(HttpResponseMessage rawApiResponse)
    {
        var response = new HttpApiGenericResponse<Stream>(rawApiResponse)
        {
            Content = await rawApiResponse.Content.ReadAsStreamAsync()
        };

        _logger.LogDebug("Deserialized API response as stream");

        return response;
    }
}
