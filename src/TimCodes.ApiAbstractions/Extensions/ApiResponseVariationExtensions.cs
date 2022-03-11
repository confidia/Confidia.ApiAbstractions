using System.Reflection;

namespace TimCodes.ApiAbstractions.Extensions;

public static class ApiResponseVariationExtensions
{
    public static async Task<IApiResponse> DeserializeAsync(this ApiResponseVariation variation, object rawResponse)
    {
        MethodInfo? method = variation.Deserializer.GetType().GetMethod(nameof(variation.Deserializer.DeserializeAsync), new[] {typeof(object)});
        MethodInfo? genericMethod = method?.MakeGenericMethod(variation.ContentType);
        return genericMethod is null
            ? new EmptyApiResponse()
            : await genericMethod.InvokeAsync(variation.Deserializer, new[] { rawResponse }) as IApiResponse ?? new EmptyApiResponse();
    }
}
