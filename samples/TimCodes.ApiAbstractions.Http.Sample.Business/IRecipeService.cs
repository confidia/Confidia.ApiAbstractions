using TimCodes.ApiAbstractions.Http.Sample.Models.Responses;

namespace TimCodes.ApiAbstractions.Http.Sample.Business;

public interface IRecipeService
{
    Task<RecipeListResponse> GetAllAsync();
    Task<RecipeResponse> GetAsync(int id);
}
