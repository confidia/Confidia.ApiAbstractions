namespace Confidia.ApiAbstractions.Mapping;

/// <summary>
/// APIs may send back different types of response that require specialist deserialization from the same endpoint.
/// This class is responsible for identifying the variation that the API sent back.
/// </summary>
public class ApiResponseVariationResolver(ApiResponseVariation defaultResponseVariation)
{
    public ApiResponseVariation DefaultResponseVariation { get; } = defaultResponseVariation;

    public List<Func<object, Task<ApiResponseVariation?>>> Variations { get; set; } 
        = new List<Func<object, Task<ApiResponseVariation?>>>();

    public async Task<ApiResponseVariation> ResolveVariationAsync(object response)
    {
        foreach(Func<object, Task<ApiResponseVariation?>> func in Variations)
        {
            ApiResponseVariation? type = await func(response);
            if (type is not null) return type;
        }

        return DefaultResponseVariation;
    }
}
