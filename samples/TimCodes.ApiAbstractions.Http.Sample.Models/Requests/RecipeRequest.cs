namespace TimCodes.ApiAbstractions.Http.Sample.Models.Requests;

public class RecipeRequest
{
    public Recipe Recipe { get; set; } = new ();

    public string ChangedBy { get; set; } = string.Empty;
}
