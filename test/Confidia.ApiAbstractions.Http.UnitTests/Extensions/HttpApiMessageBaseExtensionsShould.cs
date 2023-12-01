using Confidia.ApiAbstractions.Http.Extensions;
using Confidia.ApiAbstractions.Http.Responses;

namespace Confidia.ApiAbstractions.Http.UnitTests.Extensions;

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
