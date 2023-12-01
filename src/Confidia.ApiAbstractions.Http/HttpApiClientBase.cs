using Microsoft.Extensions.Options;
using Confidia.ApiAbstractions.Http.Configuration;
using Confidia.ApiAbstractions.Http.Extensions;

namespace Confidia.ApiAbstractions.Http;

public class HttpApiClientBase : ApiClientBase
{
    private readonly IServiceProvider _serviceProvider;

    public HttpApiClientBase(ILogger logger, HttpClient httpClient, IServiceProvider serviceProvider) : base(logger, serviceProvider)
    {
        HttpClient = httpClient;
        _serviceProvider = serviceProvider;
        if (serviceProvider.GetRequiredService<IOptions<HttpApiCollectionOptions>>().Value.TryGetValue(ApiIdentifier, out var apiOptions))
        {
            BaseUri = apiOptions.BaseUri;
        }
    }

    public Uri? BaseUri { get; }

    protected HttpClient HttpClient { get; }

    protected override async Task<object> SendRequest(IApiRequest request)
    {
        if (request is not HttpApiRequestBase httpRequest) throw new InvalidCastException($"Request is not of type {nameof(HttpApiRequestBase)}");
        if (httpRequest.Message is null) throw new InvalidOperationException($"Request HTTP message has not been generated yet");
        Logger.LogDebug("Sending {method} request to {uri}", httpRequest.Method, httpRequest.Uri);

        HttpResponseMessage response = await HttpClient.SendAsync(httpRequest.Message);
        return response;
    }

    protected HttpApiGetRequest CreateGetRequest(string path, ApiResponseVariationResolver resolver) =>
        new (CombinePathWithBaseUri(path), resolver);

    protected HttpApiPostRequest CreatePostRequest(string path, object payload, ApiResponseVariationResolver resolver) =>
        new(CombinePathWithBaseUri(path), payload, resolver);

    protected HttpApiPatchRequest CreatePatchRequest(string path, object payload, ApiResponseVariationResolver resolver) =>
        new(CombinePathWithBaseUri(path), payload, resolver);

    protected HttpApiPutRequest CreatePutRequest(string path, object payload, ApiResponseVariationResolver resolver) =>
        new(CombinePathWithBaseUri(path), payload, resolver);

    protected HttpApiDeleteRequest CreateDeleteRequest<TContent>(string path, ApiResponseVariationResolver resolver) =>
        new (CombinePathWithBaseUri(path), resolver);

    protected HttpApiGetRequest CreateGetRequest<TContent>(string path, IApiResponseDeserializer? deserializer = null) =>
        new(CombinePathWithBaseUri(path), GetResolver<TContent>(deserializer));

    protected HttpApiPostRequest CreatePostRequest<TContent>(string path, object payload, IApiResponseDeserializer? deserializer = null) =>
        new(CombinePathWithBaseUri(path), payload, GetResolver<TContent>(deserializer));

    protected HttpApiPatchRequest CreatePatchRequest<TContent>(string path, object payload, IApiResponseDeserializer? deserializer = null) =>
        new(CombinePathWithBaseUri(path), payload, GetResolver<TContent>(deserializer));

    protected HttpApiPutRequest CreatePutRequest<TContent>(string path, object payload, IApiResponseDeserializer? deserializer = null) =>
        new(CombinePathWithBaseUri(path), payload, GetResolver<TContent>(deserializer));

    protected HttpApiDeleteRequest CreateDeleteRequest<TContent>(string path, IApiResponseDeserializer? deserializer = null) =>
        new(CombinePathWithBaseUri(path), GetResolver<TContent>(deserializer));

    public ApiResponseVariationResolver GetResolverWithDotNetBadRequestHandling<TContent>(IApiResponseDeserializer? deserializer = null)
    {
        var resolver = new ApiResponseVariationResolver(GetResponseVariation<TContent>(deserializer));
        resolver.Variations.Add(async response =>
        {
            if (response is HttpResponseMessage message)
            {
                if (await message.IsDotNetBadRequest())
                {
                    return GetResponseVariation<TContent>(_serviceProvider.GetRequiredService<DotNetBadRequestHttpApiResponseDeserializer>());
                }
            }

            return null;
        });
        return resolver;
    }

    public ApiResponseVariationResolver GetUntypedResolverWithDotNetBadRequestHandling(IApiResponseDeserializer? deserializer = null)
    {
        var resolver = new ApiResponseVariationResolver(GetUntypedResponseVariation(deserializer));
        resolver.Variations.Add(async response =>
        {
            if (response is HttpResponseMessage message)
            {
                if (await message.IsDotNetBadRequest())
                {
                    return GetUntypedResponseVariation(_serviceProvider.GetRequiredService<DotNetBadRequestHttpApiResponseDeserializer>());
                }
            }

            return null;
        });
        return resolver;
    }

    private Uri CombinePathWithBaseUri(string path) =>
        new(BaseUri is not null ?
            BaseUri.AbsoluteUri.TrimEnd('/') + '/' + path.TrimStart('/') :
            path);
}
