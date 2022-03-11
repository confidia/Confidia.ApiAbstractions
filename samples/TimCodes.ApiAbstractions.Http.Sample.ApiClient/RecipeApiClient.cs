using Microsoft.Extensions.Logging;
using TimCodes.ApiAbstractions.Http.Extensions;
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

    public async Task<RecipeListResponse> GetAllAsync()
    {
        HttpApiGetRequest request = CreateGetRequest<RecipeListResponse>(BasePath);

        IApiResponse response = await SendAsync(request).ConfigureAwait(false);

        return response.ToSpecificResponse<RecipeListResponse>();
    }

    public async Task<RecipeResponse> GetAsync(int id)
    {
        HttpApiGetRequest request = CreateGetRequest<RecipeResponse>($"{BasePath}/{id}");

        IApiResponse response = await SendAsync(request).ConfigureAwait(false);

        return response.ToSpecificResponse<RecipeResponse>();
    }

    public async Task<RecipeResponse> AddAsync(RecipeRequest recipeRequest)
    {
        HttpApiPostRequest request = CreatePostRequest<RecipeResponse>($"{BasePath}", recipeRequest);

        IApiResponse response = await SendAsync(request).ConfigureAwait(false);

        return response.ToSpecificResponse<RecipeResponse>();
    }

    public async Task<RecipeResponse> UpdateAsync(RecipeRequest recipeRequest)
    {
        HttpApiPutRequest request = CreatePutRequest<RecipeResponse>($"{BasePath}", recipeRequest);

        IApiResponse response = await SendAsync(request).ConfigureAwait(false);

        return response.ToSpecificResponse<RecipeResponse>();
    }

    public async Task<HttpApiMessageBase> DeleteAsync(int id)
    {
        HttpApiDeleteRequest request = CreateDeleteRequest<HttpApiMessageBase>($"{BasePath}/{id}");

        IApiResponse response = await SendAsync(request).ConfigureAwait(false);

        return response.ToSpecificResponse<HttpApiMessageBase>();
    }

}
