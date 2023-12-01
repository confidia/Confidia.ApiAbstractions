using Confidia.ApiAbstractions.Http.Responses;

namespace Confidia.ApiAbstractions.Http.Sample.Models.Responses;

public class RecipeResponse : HttpApiMessageBase
{
    public Recipe? Recipe { get; set; }
}
