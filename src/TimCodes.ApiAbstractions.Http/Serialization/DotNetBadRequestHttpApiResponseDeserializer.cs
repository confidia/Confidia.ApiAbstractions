namespace TimCodes.ApiAbstractions.Http.Serialization;

public class DotNetBadRequestHttpApiResponseDeserializer : HttpApiResponseDeserializerBase
{
    private readonly ILogger<DotNetBadRequestHttpApiResponseDeserializer> _logger;
    private readonly JsonSerializerOptions _options;

    public DotNetBadRequestHttpApiResponseDeserializer(ILogger<DotNetBadRequestHttpApiResponseDeserializer> logger)
    {
        _logger = logger;
        _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public override async Task<IApiResponse> DeserializeAsync<TContent>(HttpResponseMessage rawApiResponse)
    {
        var response = new HttpApiValidationResponse(rawApiResponse);
        var jsonContent = await rawApiResponse.Content.ReadAsStringAsync();

        if (!string.IsNullOrEmpty(jsonContent))
        {
            response.Content = JsonSerializer.Deserialize<DotNetValidationProblem>(jsonContent, _options);
        }

        _logger.LogDebug("Deserialized API JSON response as.NET bad request");

        return response;
    }
}
