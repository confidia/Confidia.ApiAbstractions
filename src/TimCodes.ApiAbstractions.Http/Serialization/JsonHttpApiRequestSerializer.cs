using System.Text;
using System.Text.Json.Serialization;

namespace TimCodes.ApiAbstractions.Http.Serialization;

public class JsonHttpApiRequestSerializer : IApiRequestSerializer
{
    private readonly JsonSerializerOptions? _options;

    public JsonHttpApiRequestSerializer(JsonSerializerOptions? options = null)
    {
        _options = options ?? new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public void Serialize(IApiRequest request)
    {
        if (request is not HttpApiRequestBase httpRequest) throw new InvalidCastException($"Request is not of type {nameof(HttpApiRequestBase)}");

        httpRequest.Message = new HttpRequestMessage(httpRequest.Method, httpRequest.Uri);
        httpRequest.Message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypes.Json));

        if (request is HttpApiRequestWithPayload payloadRequest)
        {
            var json = payloadRequest.Payload != null ?
                JsonSerializer.Serialize(payloadRequest.Payload, _options) :
                string.Empty;

            httpRequest.Message.Content = new StringContent(json, Encoding.UTF8, MediaTypes.Json);
        }
    }
}
