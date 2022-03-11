namespace TimCodes.ApiAbstractions.Http.Serialization;

public class TextHttpApiResponseDeserializer : HttpApiResponseDeserializerBase
{
    private readonly ILogger<TextHttpApiResponseDeserializer> _logger;

    public TextHttpApiResponseDeserializer(ILogger<TextHttpApiResponseDeserializer> logger)
    {
        _logger = logger;
    }

    public override async Task<IApiResponse> DeserializeAsync<TContent>(HttpResponseMessage rawApiResponse)
    {
        var response = new HttpApiGenericResponse<string>(rawApiResponse)
        {
            Content = await rawApiResponse.Content.ReadAsStringAsync()
        };

        _logger.LogDebug("Deserialized API response as string");

        return response;
    }
}
