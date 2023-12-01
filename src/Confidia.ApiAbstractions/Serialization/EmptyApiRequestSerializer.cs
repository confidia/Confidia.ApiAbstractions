
namespace Confidia.ApiAbstractions.Serialization;

public class EmptyApiRequestSerializer(ILogger<EmptyApiRequestSerializer> logger) : IApiRequestSerializer
{
    private readonly ILogger<EmptyApiRequestSerializer> _logger = logger;

    public void Serialize(IApiRequest request) => _logger.LogDebug("Request serialized using empty serializer");
}
