using Confidia.ApiAbstractions.Http.Responses;

namespace Confidia.ApiAbstractions.Http.Sample.Models.Responses;

public class RecipeListResponse : HttpApiMessageBase
{
    public List<Recipe> Recipes { get; set; } = new List<Recipe>();
}
