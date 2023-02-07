using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Secret_Santa_App.GlobalErrorHandling;

public class ExceptionDetails
{
    public int StatusCode { get; set; }

    public string? Message { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}