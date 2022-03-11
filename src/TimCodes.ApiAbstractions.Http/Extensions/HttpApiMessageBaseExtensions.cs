namespace TimCodes.ApiAbstractions.Http.Extensions;

public static class HttpApiMessageBaseExtensions
{
    public static TEnum? GetErrorEnum<TEnum>(this HttpApiMessageBase message)
        where TEnum : Enum 
        => message.ErrorCode is null ?
            default :
            (TEnum?)Enum.ToObject(typeof(TEnum), message.ErrorCode.Value);
}
