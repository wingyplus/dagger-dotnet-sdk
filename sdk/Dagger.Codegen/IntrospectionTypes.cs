using System.Text.Json.Serialization;

namespace Dagger.Codegen;

enum TypeKind
{
    Scalar,
    Object,
    Interface,
    Union,
    Enum,
    InputObject,
    List,
    NonNull
}

public class TypeRef
{
    // TODO: use TypeKind.
    [JsonPropertyName("kind")]
    public string Kind { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("ofType")]
    public TypeRef OfType { get; set; }
}

public class Field
{
    [JsonPropertyName("name")]

    public string Name { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("type")]

    public TypeRef Type { get; set; }
    [JsonPropertyName("args")]
    public InputValue Args { get; set; }
    [JsonPropertyName("isDeprecated")]
    public bool IsDeprecated { get; set; }
    [JsonPropertyName("deprecationReason")]
    public string DeprecationReason { get; set; }
}

public class InputValue
{
    [JsonPropertyName("name")]

    public string Name { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("type")]

    public TypeRef Type { get; set; }
}

public class EnumValue
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("type")]
    public TypeRef Type { get; set; }
    [JsonPropertyName("isDeprecated")]
    public bool IsDeprecated { get; set; }
}

public class Type
{
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("enumValues")]
    public List<EnumValue> EnumValues { get; set; }
    [JsonPropertyName("fields")]
    public List<Field> Fields { get; set; }
    [JsonPropertyName("inputFields")]
    public List<InputValue> InputFields { get; set; }
    [JsonPropertyName("kind")]
    public string Kind { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class Schema
{
    [JsonPropertyName("types")]
    public List<Type> Types { get; set; }
}

public class Introspection
{
    [JsonPropertyName("__schema")]
    public Schema Schema { get; set; }
}

