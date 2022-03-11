namespace TimCodes.ApiAbstractions.Http.Configuration;

public class HttpApiCollectionOptions : Dictionary<string, HttpApiOptions>
{
}

public class HttpApiOptions
{
    public Uri? BaseUri { get; set; }
}
