using Microsoft.Extensions.DependencyInjection;
using Confidia.ApiAbstractions.Configuration;

namespace Confidia.ApiAbstractions.Http.Sample.ApiClient;

public static class HttpApiAbstractionsBuilderExtensions
{
    public static HttpApiAbstractionsBuilder AddRecipeApiClient(this HttpApiAbstractionsBuilder builder)
    {
        builder.Services.AddScoped<IRecipeApiClient, RecipeApiClient>();
        return builder;
    }
}
