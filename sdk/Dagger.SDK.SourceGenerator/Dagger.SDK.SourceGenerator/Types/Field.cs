using System.Linq;
using System.Text.Json.Serialization;
namespace Dagger.SDK;
public class Field
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("type")]
    public required TypeRef Type { get; set; }

    [JsonPropertyName("args")]
    public required InputValue[] Args { get; set; }

    [JsonPropertyName("isDeprecated")]
    public bool IsDeprecated { get; set; }

    [JsonPropertyName("deprecationReason")]
    public required string DeprecationReason { get; set; }

    /// <summary>
    /// Get optional arguments from Args.
    /// </summary>
    public IOrderedEnumerable<InputValue> OptionalArgs()
    {
        return Args
            .Where(arg => arg.Type.Kind != "NON_NULL")
            .OrderBy(type => type.Name);
    }

    /// <summary>
    /// Get required arguments from Args.
    /// </summary>
    public IOrderedEnumerable<InputValue> RequiredArgs()
    {
        return Args
            .Where(arg => arg.Type.Kind == "NON_NULL")
            .OrderBy(type => type.Name);
    }
}
