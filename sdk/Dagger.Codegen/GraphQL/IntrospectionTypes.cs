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
    public required string Kind { get; set; }
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("ofType")]
    public required TypeRef OfType { get; set; }

    public bool IsLeaf()
    {
        var tr = this;

        if (Kind == "NON_NULL")
        {
            tr = OfType;
        }
        if (tr.Kind == "ENUM")
        {
            return true;
        }
        if (tr.Kind == "SCALAR")
        {
            return true;
        }
        return false;
    }

    public bool IsList()
    {
        var tr = this;

        if (Kind == "NON_NULL")
        {
            tr = OfType;
        }
        if (tr.Kind == "LIST")
        {
            return true;
        }
        return false;
    }

    public bool IsEnum()
    {
        var tr = this;

        if (Kind == "NON_NULL")
        {
            tr = OfType;
        }
        if (tr.Kind == "ENUM")
        {
            return true;
        }
        return false;
    }

    public bool IsInputObject()
    {
        var tr = this;

        if (Kind == "NON_NULL")
        {
            tr = OfType;
        }
        if (tr.Kind == "INPUT_OBJECT")
        {
            return true;
        }
        return false;
    }

    public bool IsScalar()
    {
        var tr = this;

        if (Kind == "NON_NULL")
        {
            tr = OfType;
        }
        if (tr.Kind == "SCALAR")
        {
            return true;
        }
        return false;
    }
}

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
        return Args.Where(arg => arg.Type.Kind != "NON_NULL").OrderBy(type => type.Name);
    }

    /// <summary>
    /// Get required arguments from Args.
    /// </summary>
    public IOrderedEnumerable<InputValue> RequiredArgs()
    {
        return Args.Where(arg => arg.Type.Kind == "NON_NULL").OrderBy(type => type.Name);
    }
}

public class InputValue
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("description")]
    public required string Description { get; set; }
    [JsonPropertyName("type")]
    public required TypeRef Type { get; set; }
    [JsonPropertyName("defaultValue")]
    public string? DefaultValue { get; set; }
}

public class EnumValue
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    [JsonPropertyName("description")]
    public required string Description { get; set; }
    [JsonPropertyName("isDeprecated")]
    public bool IsDeprecated { get; set; }
}

public class Type
{
    [JsonPropertyName("description")]
    public required string Description { get; set; }
    [JsonPropertyName("enumValues")]
    public required EnumValue[] EnumValues { get; set; }
    [JsonPropertyName("fields")]
    public required Field[] Fields { get; set; }
    [JsonPropertyName("inputFields")]
    public required InputValue[] InputFields { get; set; }
    [JsonPropertyName("kind")]
    public required string Kind { get; set; }
    [JsonPropertyName("name")]
    public required string Name { get; set; }
}

public class Schema
{
    [JsonPropertyName("types")]
    public required Type[] Types { get; set; }
}

public class Introspection
{
    [JsonPropertyName("__schema")]
    public required Schema Schema { get; set; }
}
