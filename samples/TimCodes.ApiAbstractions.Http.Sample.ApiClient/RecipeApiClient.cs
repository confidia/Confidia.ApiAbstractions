using Microsoft.Extensions.Logging;
using TimCodes.ApiAbstractions.Http.Requests;
using TimCodes.ApiAbstractions.Http.Responses;
using TimCodes.ApiAbstractions.Http.Sample.Models.Requests;
using TimCodes.ApiAbstractions.Http.Sample.Models.Responses;
using TimCodes.ApiAbstractions.Models.Responses;

namespace TimCodes.ApiAbstractions.Http.Sample.ApiClient;

public class RecipeApiClient : DefaultJsonHttpApiClient, IRecipeApiClient
{
    private const string BasePath = "/api/recipes";

    public RecipeApiClient(
        ILogger<RecipeApiClient> logger, 
        HttpClient httpClient, 
        IServiceProvider serviceProvider) : 
        base(logger, httpClient, serviceProvider)
    {
    }

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
