using TimCodes.ApiAbstractions.Http.Responses;

namespace TimCodes.ApiAbstractions.Http.Sample.Models.Responses;

public class RecipeResponse : HttpApiMessageBase
{
    public Recipe? Recipe { get; set; }
}
