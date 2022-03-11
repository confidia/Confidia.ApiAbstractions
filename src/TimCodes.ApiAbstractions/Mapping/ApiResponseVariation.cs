namespace TimCodes.ApiAbstractions.Mapping;

/// <summary>
/// Defines a variation of an API response and the deserializer that would be responsible for decoding it
/// </summary>
public class ApiResponseVariation
{
    public ApiResponseVariation(IApiResponseDeserializer deserializer, Type contentType)
    {
        ContentType = contentType;
        Deserializer = deserializer;
    }

    public IApiResponseDeserializer Deserializer { get; set; }

    public Type ContentType { get; set; }
}

public class ApiResponseVariation<TContent> : ApiResponseVariation
{
    public ApiResponseVariation(IApiResponseDeserializer deserializer) : base(deserializer, typeof(TContent))
    {
    }
}