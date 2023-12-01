namespace TimCodes.ApiAbstractions.Http.Extensions;

public static class ApiResponseVariationResolverExtensions
{
    public static void AddVariationForStatusCode(this ApiResponseVariationResolver apiResponseVariationResolver, HttpStatusCode code, ApiResponseVariation variation)
    {
        apiResponseVariationResolver.Variations.Add(response =>
        {
            var httpResponse = response as HttpResponseMessage;
            if (httpResponse?.StatusCode == code)
            {
                return Task.FromResult<ApiResponseVariation?>(variation);
            }

            return Task.FromResult<ApiResponseVariation?>(null);
        });
    }
}
