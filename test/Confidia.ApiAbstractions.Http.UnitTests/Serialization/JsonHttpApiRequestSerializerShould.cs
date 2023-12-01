namespace Confidia.ApiAbstractions.Http.UnitTests.Serialization;

public class JsonHttpApiRequestSerializerShould
{
    private readonly JsonHttpApiRequestSerializer _serializer = new();

    [Fact]
    public async void SerializeJson()
    {
        var request = new HttpApiPostRequest(new Uri("http://Confidia.net"), new
        {
            test = "te&st",
            date = new DateTime(2000, 1, 1),
            nullint = (int?)null
        }, null);

        _serializer.Serialize(request);
        Assert.Equal("{\"test\":\"te\\u0026st\",\"date\":\"2000-01-01T00:00:00\"}", await request.Message.Content.ReadAsStringAsync());
        Assert.Equal("application/json", request.Message.Content.Headers.ContentType.MediaType);
    }

}
