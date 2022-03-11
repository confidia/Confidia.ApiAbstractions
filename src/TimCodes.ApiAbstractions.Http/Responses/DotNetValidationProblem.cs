namespace TimCodes.ApiAbstractions.Http.Responses;

public class DotNetValidationProblem
{
    public string? Detail { get; set; }

    public IDictionary<string, string[]>? Errors { get; set; }

    public string? Instance { get; set; }

    public int? Status { get; set; }

    public string? Title { get; set; }

    public string? Type { get; set; }

    public IDictionary<string, object?>? Extensions { get; set; }
}
