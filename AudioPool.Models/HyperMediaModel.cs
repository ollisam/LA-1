using System.Dynamic;
using System.Text.Json.Serialization;

namespace AudioPool.Models;

public class HyperMediaModel
{
    public HyperMediaModel() { Links = new ExpandoObject(); }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyName("_links")]
    public ExpandoObject Links { get; set; }
}