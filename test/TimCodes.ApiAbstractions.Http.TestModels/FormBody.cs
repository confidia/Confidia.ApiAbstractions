using TimCodes.ApiAbstractions.Http.Attributes;

namespace TimCodes.ApiAbstractions.Http.TestModels;

public class FormBody
{
    [HttpFormName("property")]
    public string? Property1 { get; set; }
    public int? Property2 { get; set; }
}
