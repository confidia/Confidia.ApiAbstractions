namespace Confidia.ApiAbstractions.Mapping;

/// <summary>
/// Defines a variation of an API response and the deserializer that would be responsible for decoding it
/// </summary>
public class ApiResponseVariation(IApiResponseDeserializer deserializer, Type contentType)
{
    public IApiResponseDeserializer Deserializer { get; set; } = deserializer;

    public Type ContentType { get; set; } = contentType;
}

public class ApiResponseVariation<TContent>(IApiResponseDeserializer deserializer) : ApiResponseVariation(deserializer, typeof(TContent))
{
}