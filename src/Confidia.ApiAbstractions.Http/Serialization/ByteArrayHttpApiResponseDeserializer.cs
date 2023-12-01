namespace Confidia.ApiAbstractions.Http.Serialization;

public class ByteArrayHttpApiResponseDeserializer(ILogger<ByteArrayHttpApiResponseDeserializer> logger) : HttpApiResponseDeserializerBase
{
    private readonly ILogger<ByteArrayHttpApiResponseDeserializer> _logger = logger;

    public override async Task<IApiResponse> DeserializeAsync<TContent>(HttpResponseMessage rawApiResponse)
    {
        var response = new HttpApiGenericResponse<byte[]>(rawApiResponse)
        {
            Content = await rawApiResponse.Content.ReadAsByteArrayAsync()
        };

        _logger.LogDebug("Deserialized API response as byte array");

        return response;
    }
}
