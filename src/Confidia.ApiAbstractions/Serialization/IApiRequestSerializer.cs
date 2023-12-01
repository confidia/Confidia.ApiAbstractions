namespace Confidia.ApiAbstractions.Serialization;

/// <summary>
/// Represents a class responsible for serializing API requests
/// </summary>
public interface IApiRequestSerializer
{
    void Serialize(IApiRequest request);
}
