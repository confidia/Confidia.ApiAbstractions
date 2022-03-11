namespace TimCodes.ApiAbstractions.Http.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class HttpFormNameAttribute : Attribute
{
    public HttpFormNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
