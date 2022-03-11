
namespace TimCodes.ApiAbstractions.Serialization;

public class EmptyApiRequestSerializer : IApiRequestSerializer
{
    private readonly ILogger<EmptyApiRequestSerializer> _logger;

    public EmptyApiRequestSerializer(ILogger<EmptyApiRequestSerializer> logger)
    {
        _logger = logger;
    }

    public void Serialize(IApiRequest request) => _logger.LogDebug("Request serialized using empty serializer");
}
