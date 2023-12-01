using Confidia.ApiAbstractions.Http.Sample.Models.Responses;

namespace Confidia.ApiAbstractions.Http.Sample.Business;

public interface IRecipeService
{
    Task<RecipeListResponse> GetAllAsync();
    Task<RecipeResponse> GetAsync(int id);
}
