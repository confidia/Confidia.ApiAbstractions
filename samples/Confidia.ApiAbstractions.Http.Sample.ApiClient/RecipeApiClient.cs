using Microsoft.Extensions.Logging;
using Confidia.ApiAbstractions.Http.Requests;
using Confidia.ApiAbstractions.Http.Responses;
using Confidia.ApiAbstractions.Http.Sample.Models.Requests;
using Confidia.ApiAbstractions.Http.Sample.Models.Responses;
using Confidia.ApiAbstractions.Models.Responses;

namespace Confidia.ApiAbstractions.Http.Sample.ApiClient;

public class RecipeApiClient(
    ILogger<RecipeApiClient> logger,
    HttpClient httpClient,
    IServiceProvider serviceProvider) : DefaultJsonHttpApiClient(logger, httpClient, serviceProvider), IRecipeApiClient
{
    private const string BasePath = "/api/recipes";

    public override string ApiIdentifier => "RecipeApi";

    public async Task<IApiResponse> GetAllAsync()
    {
        HttpApiGetRequest request = CreateGetRequest<RecipeListResponse>(BasePath);

        return await SendAsync(request).ConfigureAwait(false);
    }

    public async Task<IApiResponse> GetAsync(int id)
    {
        HttpApiGetRequest request = CreateGetRequest<RecipeResponse>($"{BasePath}/{id}");

        return await SendAsync(request).ConfigureAwait(false);
    }

    public async Task<IApiResponse> AddAsync(RecipeRequest recipeRequest)
    {
        HttpApiPostRequest request = CreatePostRequest<RecipeResponse>($"{BasePath}", recipeRequest);

        return await SendAsync(request).ConfigureAwait(false);
    }

    public async Task<IApiResponse> UpdateAsync(RecipeRequest recipeRequest)
    {
        HttpApiPutRequest request = CreatePutRequest<RecipeResponse>($"{BasePath}", recipeRequest);

        return await SendAsync(request).ConfigureAwait(false);
    }

    public async Task<IApiResponse> DeleteAsync(int id)
    {
        HttpApiDeleteRequest request = CreateDeleteRequest<HttpApiMessageBase>($"{BasePath}/{id}");

        return await SendAsync(request).ConfigureAwait(false);
    }

}
