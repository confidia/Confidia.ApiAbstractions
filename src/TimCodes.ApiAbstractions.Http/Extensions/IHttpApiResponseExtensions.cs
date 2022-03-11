namespace TimCodes.ApiAbstractions.Http.Extensions;

public static class HttpApiResponseExtensions
{
    public static TResponse ToSpecificResponse<TResponse>(this IApiResponse response)
    {
        if (response is HttpApiGenericResponse<TResponse> specificResponse)
        {
            return specificResponse.Content is null
                ? throw new InvalidOperationException("No content found, check the deserializers are working correctly")
                : specificResponse.Content;
        }

        throw new InvalidOperationException($"Unexpected response type {response.GetType().Name}");        
    }
}
