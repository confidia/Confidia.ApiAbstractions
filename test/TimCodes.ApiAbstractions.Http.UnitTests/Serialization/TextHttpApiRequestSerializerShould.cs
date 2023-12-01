namespace TimCodes.ApiAbstractions.Http.UnitTests.Serialization;

public class TextHttpApiRequestSerializerShould
{
    private readonly TextHttpApiRequestSerializer _serializer = new();

    [Fact]
    public void SerializeString()
    {
        var request = new HttpApiPostRequest(new Uri("http://timcodes.net"), "test", null);

        _serializer.Serialize(request);
        Assert.Equal("test", request.Message.Content.ReadAsStringAsync().Result);
        Assert.Equal("text/plain", request.Message.Content.Headers.ContentType.MediaType);
    }

}
