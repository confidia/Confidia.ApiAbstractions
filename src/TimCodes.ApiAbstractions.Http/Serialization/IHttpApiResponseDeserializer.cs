namespace TimCodes.ApiAbstractions.Http.Serialization;

public abstract class HttpApiResponseDeserializerBase : IApiResponseDeserializer
{
    public abstract Task<IApiResponse> DeserializeAsync<TContent>(HttpResponseMessage rawApiResponse);

    public Task<IApiResponse> DeserializeAsync<TContent>(object rawApiResponse) 
        => DeserializeAsync<TContent>((HttpResponseMessage)rawApiResponse);
}
