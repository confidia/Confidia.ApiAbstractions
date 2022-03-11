using TimCodes.ApiAbstractions.Http.Responses;
using TimCodes.ApiAbstractions.Http.Sample.Models.Requests;
using TimCodes.ApiAbstractions.Http.Sample.Models.Responses;

namespace TimCodes.ApiAbstractions.Http.Sample.ApiClient;

public interface IRecipeApiClient
{
    Task<RecipeListResponse> GetAllAsync();
    Task<RecipeResponse> GetAsync(int id);
    Task<RecipeResponse> AddAsync(RecipeRequest request);
    Task<RecipeResponse> UpdateAsync(RecipeRequest request);
    Task<HttpApiMessageBase> DeleteAsync(int id);
}
