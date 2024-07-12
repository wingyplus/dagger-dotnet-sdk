using System.Text.Json.Serialization;

namespace Dagger.SDK.SourceGenerator.Types;

public class EnumValue
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("description")]
    public required string Description { get; set; }
    [JsonPropertyName("isDeprecated")]
    public required bool IsDeprecated { get; set; }
    [JsonPropertyName("deprecationReason")]
    public required string DeprecationReason { get; set; }
}
