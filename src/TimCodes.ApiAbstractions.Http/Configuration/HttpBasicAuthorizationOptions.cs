namespace TimCodes.ApiAbstractions.Http.Authorization.Basic;

public class HttpBasicAuthorizationCredentials
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

public class HttpBasicAuthorizationOptions : Dictionary<string, HttpBasicAuthorizationCredentials>
{
    
}
