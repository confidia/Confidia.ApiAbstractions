namespace Confidia.ApiAbstractions.Http.UnitTests.Serialization;

public class FormHttpApiRequestSerializerShould
{
    private readonly FormHttpApiRequestSerializer _serializer = new();

    [Fact]
    public async void SerializeFormBody()
    {
        var request = new HttpApiPostRequest(new Uri("http://Confidia.net"), new
        {
            test = "te&st",
            date = new DateTime(2000, 1, 1),
            nullint = (int?)null
        }, null);

        _serializer.Serialize(request);
        Assert.Equal("test=te%26st&date=2000-01-01T00%3A00%3A00.0000000", await request.Message?.Content?.ReadAsStringAsync());
        Assert.Equal("application/x-www-form-urlencoded", request.Message?.Content?.Headers?.ContentType?.MediaType);
    }

    [Fact]
    public async void SerializeFormBodyWithAttribute()
    {
        var request = new HttpApiPostRequest(new Uri("http://Confidia.net"), new FormBody
        {
            Property1 = "test",
            Property2 = 1
        }, null);

        _serializer.Serialize(request);
        Assert.Equal("property=test&Property2=1", await request.Message?.Content?.ReadAsStringAsync());
        Assert.Equal("application/x-www-form-urlencoded", request.Message?.Content?.Headers?.ContentType?.MediaType);
    }

    [Fact]
    public async void SerializeFormBodyFromDictionary()
    {
        var request = new HttpApiPostRequest(new Uri("http://Confidia.net"), new Dictionary<string, string>
        {
            { "test", "te&st" },
            { "date", "2000-01-01T00:00:00.0000000" }
        }, null);

        _serializer.Serialize(request);
        Assert.Equal("test=te%26st&date=2000-01-01T00%3A00%3A00.0000000", await request.Message.Content.ReadAsStringAsync());
        Assert.Equal("application/x-www-form-urlencoded", request.Message.Content.Headers.ContentType.MediaType);
    }
}
