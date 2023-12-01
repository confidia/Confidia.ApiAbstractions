namespace TimCodes.ApiAbstractions.Http.UnitTests.Serialization;

public class JsonHttpApiRequestSerializerShould
{
    private readonly JsonHttpApiRequestSerializer _serializer = new();

    [Fact]
    public void SerializeJson()
    {
        var request = new HttpApiPostRequest(new Uri("http://timcodes.net"), new
        {
            test = "te&st",
            date = new DateTime(2000, 1, 1),
            nullint = (int?)null
        }, null);

        _serializer.Serialize(request);
        Assert.Equal("{\"test\":\"te\\u0026st\",\"date\":\"2000-01-01T00:00:00\"}", request.Message.Content.ReadAsStringAsync().Result);
        Assert.Equal("application/json", request.Message.Content.Headers.ContentType.MediaType);
    }

}
