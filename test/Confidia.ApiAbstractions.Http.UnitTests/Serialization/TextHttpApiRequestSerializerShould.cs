namespace Confidia.ApiAbstractions.Http.UnitTests.Serialization;

public class TextHttpApiRequestSerializerShould
{
    private readonly TextHttpApiRequestSerializer _serializer = new();

    [Fact]
    public async void SerializeString()
    {
        var request = new HttpApiPostRequest(new Uri("http://Confidia.net"), "test", null);

        _serializer.Serialize(request);
        Assert.Equal("test", await request.Message.Content.ReadAsStringAsync());
        Assert.Equal("text/plain", request.Message.Content.Headers.ContentType.MediaType);
    }

}
