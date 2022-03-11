using System.Text.Json.Serialization;

namespace TimCodes.ApiAbstractions.Http.TestModels.Reqres;

public class User
{
    public UserData Data { get; set; }
}

public class UserData
{
    public int Id { get; set; }
    public string Email { get; set; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }
}