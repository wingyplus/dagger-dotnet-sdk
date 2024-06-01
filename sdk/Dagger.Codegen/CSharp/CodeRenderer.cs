using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Dagger.Codegen.CSharp;

// TODO: rename Query -> Client.
// TODO: deprecated message.
public class CodeRenderer : Codegen.CodeRenderer
{
    public override string RenderPre()
    {
        return """
        using System.Collections.Immutable;
        using System.Text.Json.Serialization;

        using Dagger.SDK.GraphQL;

        namespace Dagger.SDK;

        public class Scalar
        {
            public readonly string Value;

            public override string ToString() => Value;
        }

        public class Object(QueryBuilder queryBuilder, GraphQLClient gqlClient)
        {
            public QueryBuilder QueryBuilder { get; } = queryBuilder;
            public GraphQLClient GraphQLClient { get; } = gqlClient;
        }
        """;
    }

    // TODO: test value converter.
    public override string RenderEnum(Type type)
    {
        var evs = type.EnumValues.Select(ev => ev.Name);
        return $$"""
        {{RenderDocComment(type)}}
        [JsonConverter(typeof(JsonStringEnumConverter<{{type.Name}}>))]
        public enum {{type.Name}} {
            {{string.Join(",", evs)}}
        }
        """;
    }

    public override string RenderInputObject(Type type)
    {
        var properties = type.InputFields.Select(field => $$"""
        {{RenderDocComment(field)}}
        public string {{Formatter.FormatProperty(field.Name)}};
        """);

        return $$"""
        {{RenderDocComment(type)}}
        public struct {{type.Name}}
        {
            {{string.Join("\n\n", properties)}}
        }
        """;
    }

    public override string RenderObject(Type type)
    {
        var methods = type.Fields.Select(field =>
        {
            var methodName = Formatter.FormatMethod(field.Name);
            if (type.Name.Equals(field.Name, StringComparison.CurrentCultureIgnoreCase))
            {
                methodName = $"{methodName}_";
            }

            var requiredArgs = field.RequiredArgs();
            var optionalArgs = field.OptionalArgs();
            var args = requiredArgs.Select(RenderArgument).Concat(optionalArgs.Select(RenderOptionalArgument));

            return $$"""
            {{RenderDocComment(field)}}
            public {{RenderReturnType(field.Type)}} {{methodName}}({{string.Join(",", args)}})
            {
                {{RenderArgumentBuilder(field)}}
                {{RenderQueryBuilder(field)}}
                return {{RenderReturnValue(field)}};
            }
            """;
        });

        return $$"""
        {{RenderDocComment(type)}}
        public class {{type.Name}}(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
        {
            {{string.Join("\n\n", methods)}}
        }
        """;
    }

    public override string RenderScalar(Type type)
    {
        return $$"""
        {{RenderDocComment(type)}}
        public class {{type.Name}} : Scalar {}
        """;
    }

    public override string Format(string source)
    {
        return CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace(eol: "\n").ToFullString();
    }

    private static string RenderDocComment(Type type)
    {
        return RenderDocComment(type.Description);
    }

    private static string RenderDocComment(Field field)
    {
        return RenderDocComment(field.Description);
    }

    private static string RenderDocComment(InputValue field)
    {
        return RenderDocComment(field.Description);
    }

    private static string RenderDocComment(string doc)
    {
        if (string.IsNullOrEmpty(doc))
        {
            return "";
        }
        var description = doc
            .Split("\n")
            .Select(line => $"/// {line}")
            .Select(line => line.Trim());
        return $$"""
        /// <summary>
        {{string.Join("\n", description)}}
        /// </summary>
        """;
    }

    private static string RenderArgument(InputValue argument)
    {
        return $"{RenderType(argument.Type)} {Formatter.FormatVarName(argument.Name)}";
    }

    private static string RenderOptionalArgument(InputValue argument)
    {
        var nullableType = argument.DefaultValue == null ? "?" : "";
        return $"{RenderType(argument.Type)}{nullableType} {Formatter.FormatVarName(argument.Name)} = {RenderDefaultValue(argument)}";
    }

    private static string RenderDefaultValue(InputValue argument)
    {
        if (argument.Type.IsList())
        {
            return "null";
        }
        if (argument.Type.IsEnum() && argument.DefaultValue != null)
        {
            return $"{argument.Type.Name}.{argument.DefaultValue}";
        }
        return argument.DefaultValue ?? "null";
    }

    private static string RenderType(TypeRef type)
    {
        var tr = type.GetType_();
        if (tr.IsList())
        {
            return $"{RenderType(tr.OfType)}[]";
        }
        return ToCSharpType(tr.Name);
    }

    private static string ToCSharpType(string name)
    {
        return name switch
        {
            "String" => "string",
            "Boolean" => "bool",
            "Int" => "int",
            "Float" => "float",
            _ => name,
        };
    }

    private static string RenderReturnType(TypeRef type)
    {
        if (type.IsLeaf() || type.IsList())
        {
            return $"async Task<{RenderType(type)}>";
        }
        return RenderType(type);
    }

    private static string RenderReturnValue(Field field)
    {
        var type = field.Type;
        if (type.IsLeaf() || type.IsList())
        {
            return $"await Engine.Execute<{RenderType(field.Type)}>(GraphQLClient, QueryBuilder)";
        }
        return $"new {RenderType(field.Type)}(QueryBuilder, GraphQLClient)";
    }

    private object RenderArgumentBuilder(Field field)
    {
        if (field.Args.Length == 0)
        {
            return "";
        }

        var requiredArgs = field.RequiredArgs();
        var builder = new StringBuilder("var arguments = ImmutableList<Argument>.Empty;");
        builder.Append('\n');

        if (requiredArgs.Count() > 0)
        {
            builder.Append("arguments = arguments.").Append(string.Join(".", requiredArgs.Select(arg => $$"""Add(new Argument("{{arg.Name}}", {{RenderArgumentValue(arg)}}))"""))).Append(';');
            builder.Append('\n');
        }

        return builder.ToString();
    }

    private static string RenderArgumentValue(InputValue arg)
    {
        var argName = Formatter.FormatVarName(arg.Name);

        if (arg.Type.IsScalar())
        {
            var type = RenderType(arg.Type);
            switch (type)
            {
                case "string": return $"new StringValue({argName})";
                case "bool": return $"new BooleanValue({argName})";
                case "int": return $"new IntValue({argName})";
                case "float": return $"new FloatValue({argName})";
                default: return $"new StringValue({argName}.Value)";
            }
        }

        if (arg.Type.IsEnum())
        {
            return $"new StringValue({argName}.ToString())";
        }

        if (arg.Type.IsInputObject())
        {
            return $"new ObjectValue({argName}.ToKeyValues())";
        }

        if (arg.Type.IsList())
        {
            // FIXME: put correct value.
            return $"new ListValue([])";
        }

        throw new Exception($"SHIT! {arg.Type.OfType.Kind}");
    }

    private static string RenderQueryBuilder(Field field)
    {
        var builder = new StringBuilder("var queryBuilder = QueryBuilder.Select(");
        builder.Append($"\"{field.Name}\"");
        if (field.Args.Length > 0)
        {
            builder.Append(", arguments");
        }
        builder.Append(')');
        if (field.Type.IsList())
        {
            builder.Append(".Select(\"id\")");
        }
        builder.Append(';');
        return builder.ToString();
    }
}
