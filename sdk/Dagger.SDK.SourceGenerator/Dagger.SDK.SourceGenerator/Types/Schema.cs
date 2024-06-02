using System.Text.Json.Serialization;

namespace Dagger.SDK;

public class Schema
{
    [JsonPropertyName("types")]
    public required Type[] Types { get; set; }
}
