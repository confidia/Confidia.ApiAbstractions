namespace Confidia.ApiAbstractions.Http.Sample.Models;

public class Recipe
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string[] Ingredients { get; set; } = Array.Empty<string>();

    public string? Method { get; set; }
}
