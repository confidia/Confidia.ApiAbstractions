using Confidia.ApiAbstractions.Http.Sample.Models.Requests;
using Confidia.ApiAbstractions.Models.Responses;

namespace Confidia.ApiAbstractions.Http.Sample.ApiClient;

public interface IRecipeApiClient
{
    Task<IApiResponse> GetAllAsync();
    Task<IApiResponse> GetAsync(int id);
    Task<IApiResponse> AddAsync(RecipeRequest request);
    Task<IApiResponse> UpdateAsync(RecipeRequest request);
    Task<IApiResponse> DeleteAsync(int id);
}
