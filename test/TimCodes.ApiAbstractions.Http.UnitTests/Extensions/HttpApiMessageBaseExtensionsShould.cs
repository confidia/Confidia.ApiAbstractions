using TimCodes.ApiAbstractions.Http.Extensions;
using TimCodes.ApiAbstractions.Http.Responses;

namespace TimCodes.ApiAbstractions.Http.UnitTests.Extensions;

public class HttpApiMessageBaseExtensionsShould
{
    [Fact]
    public void ConvertToEnum()
    {
        var message = new HttpApiMessageBase
        {
            ErrorCode = 10
        };

        Assert.Equal(TestEnum.Member, message.GetErrorEnum<TestEnum>());
    }
}
