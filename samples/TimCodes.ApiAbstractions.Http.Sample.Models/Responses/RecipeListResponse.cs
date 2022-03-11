using TimCodes.ApiAbstractions.Http.Responses;

namespace TimCodes.ApiAbstractions.Http.Sample.Models.Responses;

public class RecipeListResponse : HttpApiMessageBase
{
    public List<Recipe> Recipes { get; set; } = new List<Recipe>();
}
