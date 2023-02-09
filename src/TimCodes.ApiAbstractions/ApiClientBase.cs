using TimCodes.ApiAbstractions.Extensions;
using TimCodes.ApiAbstractions.FaultTolerance;

namespace TimCodes.ApiAbstractions;

public abstract class ApiClientBase : IApiClient
{
    public ApiClientBase(ILogger logger, IServiceProvider serviceProvider)
    {
        DefaultRequestSerlializer = serviceProvider.GetRequiredService<EmptyApiRequestSerializer>();
        DefaultResponeDeserlializer = serviceProvider.GetRequiredService<EmptyApiResponseDeserializer>();
        Logger = logger;
    }

    public virtual string ApiIdentifier => "GenericApi";
    public IApiRequestSerializer DefaultRequestSerlializer { get; set; }
    public IApiResponseDeserializer DefaultResponeDeserlializer { get; set; }
    public IAuthorizationProvider? DefaultAuthorizationProvider { get; set; }
    public IRetryPolicy[] DefaultRetryPolicies { get; set; } = Array.Empty<IRetryPolicy>();

    protected ILogger Logger { get; }

    public virtual async Task<IApiResponse> SendAsync<TSuccess, TFailure>(IApiRequest request, Func<TSuccess, Task>? onSuccess, Func<TFailure, Task>? onFailure = null, Func<ErrorApiResponse, Task>? onException = null)
    {
        var result = await SendAsync(request);

        if (result is ErrorApiResponse exception)
        {
            await onException(exception);
            return result;
        }

        if (result.Success && onSuccess is not null)
        {
            await onSuccess((TSuccess)result);
        } 
        else if (!result.Success && onFailure is not null)
        {
            await onFailure((TFailure)result);
        }
        return result;
    }

    public virtual async Task<IApiResponse> SendAsync(IApiRequest request)
    {
        //Get serializer
        IApiRequestSerializer serializer = GetRequestSerializer(request);
        Logger.LogDebug("Serializing request with {serializer}", serializer.GetType().Name);

        var attempt = 1;
        do
        {
            IApiResponse response = await AttemptSendAsync(request, serializer);

            if (response.Success)
            {
                return response;
            }

            //Retry policies if failed
            if ((request.RetryPolicies.Any() || DefaultRetryPolicies.Any()) && attempt < 50) //for safety
            {
                IRetryPolicy? policy = request.RetryPolicies.FirstOrDefault(q => q.ShouldRetry(response, attempt));
                if (policy is null)
                {
                    policy = DefaultRetryPolicies.FirstOrDefault(q => q.ShouldRetry(response, attempt));
                }

                if (policy is not null)
                {
                    await policy.OnBeforeRetryAsync(request, response, ApiIdentifier);
                    attempt++;
                    continue;
                }
            }

            return response;
        } while (true);
    }

    protected virtual async Task<IApiResponse> AttemptSendAsync(IApiRequest request, IApiRequestSerializer serializer)
    {
        //Serialize
        serializer.Serialize(request);

        //Authorize
        await AddAuthorizationAsync(request);

        request.OnBeforeSend?.Invoke();

        //SendRequest
        var rawResponse = await SendRequest(request);

        //Parse response
        return await DeserializeResponseAsync(rawResponse, request.Resolver);
    }

    protected abstract Task<object> SendRequest(IApiRequest request);

    protected virtual async Task<IApiResponse> DeserializeResponseAsync(object rawResponse, ApiResponseVariationResolver? apiResponseVariationResolver = null)
    {
        try
        {
            IApiResponse response;
            if (apiResponseVariationResolver is null)
            {
                Logger.LogWarning("No response variation resolver used.");
                response = new EmptyApiResponse();
            }
            else
            {
                ApiResponseVariation variation = await apiResponseVariationResolver.ResolveVariationAsync(rawResponse);

                if (variation is null)
                {
                    Logger.LogWarning("No response variation matched.");
                    response = new EmptyApiResponse();
                }
                else
                {
                    Logger.LogDebug("Variant matched, deserializing {type} with {deserializer}", variation.ContentType.Name, variation.Deserializer.GetType().Name);
                    response = await variation.DeserializeAsync(rawResponse);
                }
            }

            await AfterParseAsync(response);

            return response;
        }
        catch (Exception e)
        {
            return new ErrorApiResponse
            {
                Exception = e
            };
        }
    }

    public ApiResponseVariation GetResponseVariation<TContent>(IApiResponseDeserializer? deserializer = null)
    {
        deserializer ??= DefaultResponeDeserlializer;
        return new ApiResponseVariation<TContent>(deserializer);
    }

    public ApiResponseVariation GetUntypedResponseVariation(IApiResponseDeserializer? deserializer = null)
    {
        deserializer ??= DefaultResponeDeserlializer;
        return new ApiResponseVariation(deserializer, typeof(object));
    }

    public ApiResponseVariationResolver GetResolver<TContent>(IApiResponseDeserializer? deserializer = null) 
        => new ApiResponseVariationResolver(GetResponseVariation<TContent>(deserializer));

    public ApiResponseVariationResolver GetUntypedResolver(IApiResponseDeserializer? deserializer = null)
        => new ApiResponseVariationResolver(GetUntypedResponseVariation(deserializer));

    protected virtual Task AfterParseAsync(IApiResponse response) => Task.CompletedTask;

    private Task AddAuthorizationAsync(IApiRequest request)
    {
        IAuthorizationProvider? authProvider = GetAuthorizationProvider(request);
        if (authProvider is null) return Task.CompletedTask;

        Logger.LogDebug("Adding authorization with {authProvider}", authProvider.GetType().Name);

        return authProvider.AddAuthorizationAsync(request, ApiIdentifier);
    }

    protected IAuthorizationProvider? GetAuthorizationProvider(IApiRequest request)
        => request.AuthorizationProvider ?? DefaultAuthorizationProvider;

    protected IApiRequestSerializer GetRequestSerializer(IApiRequest request)
        => request.Serializer ?? DefaultRequestSerlializer;
}
