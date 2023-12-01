namespace TimCodes.ApiAbstractions.Http.Serialization;

public class StreamHttpApiResponseDeserializer(ILogger<StreamHttpApiResponseDeserializer> logger) : HttpApiResponseDeserializerBase
{
    private readonly ILogger<StreamHttpApiResponseDeserializer> _logger = logger;

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
