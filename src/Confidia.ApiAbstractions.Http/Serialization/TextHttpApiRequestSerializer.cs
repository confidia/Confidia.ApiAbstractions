using System.Text;

namespace Confidia.ApiAbstractions.Http.Serialization;

public class TextHttpApiRequestSerializer : IApiRequestSerializer
{
    public void Serialize(IApiRequest request)
    {
        if (request is not HttpApiRequestBase httpRequest) throw new InvalidCastException($"Request is not of type {nameof(HttpApiRequestBase)}");

        httpRequest.Message = new HttpRequestMessage(httpRequest.Method, httpRequest.Uri);

        if (request is HttpApiRequestWithPayload payloadRequest)
        {
            httpRequest.Message.Content = new StringContent(payloadRequest.Payload?.ToString() ?? string.Empty, Encoding.UTF8, MediaTypes.Text);
        }
    }
}
