namespace Confidia.ApiAbstractions.Http.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class HttpFormNameAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;
}
