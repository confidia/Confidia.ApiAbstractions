using System.Reflection;
using Confidia.ApiAbstractions.Http.Attributes;

namespace Confidia.ApiAbstractions.Http.Serialization;

public class FormHttpApiRequestSerializer : IApiRequestSerializer
{
    public void Serialize(IApiRequest request)
    {
        if (request is not HttpApiRequestBase httpRequest) throw new InvalidCastException($"Request is not of type {nameof(HttpApiRequestBase)}");

        httpRequest.Message = new HttpRequestMessage(httpRequest.Method, httpRequest.Uri);
        httpRequest.Message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypes.Json));

        if (request is HttpApiRequestWithPayload payloadRequest)
        {
            if (payloadRequest.Payload != null)
            {
                Dictionary<string, string> payload;

                if (payloadRequest.Payload is Dictionary<string,string> payloadDictionary)
                {
                    payload = payloadDictionary;
                }
                else
                {
                    payload = new Dictionary<string, string>();

                    foreach(PropertyInfo property in payloadRequest.Payload
                        .GetType()
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {
                        var name = (property.GetCustomAttribute(typeof(HttpFormNameAttribute)) as HttpFormNameAttribute)?.Name ?? 
                            property.Name;
                        var value = FormatFormValue(property.GetValue(payloadRequest.Payload, null));

                        if (value is null) continue;

                        payload.Add(name, value);
                    }
                }

                httpRequest.Message.Content = new FormUrlEncodedContent(payload);
            }
        }
    }

    protected virtual string? FormatFormValue(object? v)
    {
        if (v is null) return null;
        if (v is DateTime time) return time.ToString("o");
        if (v is DateTime?) return ((DateTime?)v).Value.ToString("o");
        return v.ToString();
    }
}
