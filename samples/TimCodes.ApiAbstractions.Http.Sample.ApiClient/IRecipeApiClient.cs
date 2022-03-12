using TimCodes.ApiAbstractions.Http.Sample.Models.Requests;
using TimCodes.ApiAbstractions.Models.Responses;

namespace TimCodes.ApiAbstractions.Http.Sample.ApiClient;

public interface IRecipeApiClient
{
    Task<IApiResponse> GetAllAsync();
    Task<IApiResponse> GetAsync(int id);
    Task<IApiResponse> AddAsync(RecipeRequest request);
    Task<IApiResponse> UpdateAsync(RecipeRequest request);
    Task<IApiResponse> DeleteAsync(int id);
}
