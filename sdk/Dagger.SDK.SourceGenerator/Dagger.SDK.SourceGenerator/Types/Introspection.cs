using System.Text.Json.Serialization;
namespace Dagger.SDK;
public class Introspection
{
    [JsonPropertyName("__schema")]
    public required Schema Schema { get; set; }
}
