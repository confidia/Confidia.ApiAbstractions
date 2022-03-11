namespace TimCodes.ApiAbstractions.Http.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<bool> IsDotNetBadRequest(this HttpResponseMessage message)
    {
        var json = await message.Content.ReadAsStringAsync();

        return message.StatusCode == HttpStatusCode.BadRequest 
            && json.Contains("https://tools.ietf.org/html/rfc7231#section-6.5.1");
    }
}
