namespace Confidia.ApiAbstractions.Http.Serialization;

public class JsonHttpApiResponseDeserializer : HttpApiResponseDeserializerBase
{
    private readonly ILogger<JsonHttpApiResponseDeserializer> _logger;
    private readonly JsonSerializerOptions _options;

    public JsonHttpApiResponseDeserializer(ILogger<JsonHttpApiResponseDeserializer> logger, JsonSerializerOptions? options = null)
    {
        _logger = logger;
        _options = options ?? new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public override async Task<IApiResponse> DeserializeAsync<TContent>(HttpResponseMessage rawApiResponse)
    {
        var response = new HttpApiGenericResponse<TContent>(rawApiResponse);
        var jsonContent = await rawApiResponse.Content.ReadAsStringAsync();

        if (!string.IsNullOrEmpty(jsonContent))
        {
            response.Content = JsonSerializer.Deserialize<TContent>(jsonContent, _options);
        }

        _logger.LogDebug("Deserialized API JSON response as {type}", typeof(TContent).Name);

        return response;
    }
}
