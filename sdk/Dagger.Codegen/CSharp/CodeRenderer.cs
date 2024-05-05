using System.Diagnostics;

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

        public class InputObject
        {
        }
        """;
    }

    // TODO: test value converter.
    public override string RenderEnum(Type type)
    {
        var evs = type.EnumValues.Select(ev => ev.Name);
        return $$"""
        {{RenderDocComment(type)}}
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
        public class {{type.Name}} : InputObject
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

            var (requiredArgs, optionalArgs) = SplitArguments(field.Args);
            var args = requiredArgs.Select(RenderArgument).Concat(optionalArgs.Select(RenderOptionalArgument));

            return $$"""
            {{RenderDocComment(field)}}
            public {{RenderReturnType(field.Type)}} {{methodName}}({{string.Join(",", args)}})
            {
                {{RenderArgumentBuilder(field)}}
                var queryBuilder = QueryBuilder.Select("{{field.Name}}");
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
        var description = doc.Split("\n").Select(text => $"/// {text}");
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
        if (argument.Type.Kind == "LIST")
        {
            return "null";
        }
        if (argument.Type.Kind == "ENUM" && argument.DefaultValue != null)
        {
            return $"{argument.Type.Name}.{argument.DefaultValue}";
        }
        return argument.DefaultValue ?? "null";
    }

    private static string RenderType(TypeRef type)
    {
        var _ref = type;
        if (_ref.Kind == "NON_NULL")
        {
            _ref = _ref.OfType;
        }

        if (_ref.Kind == "LIST")
        {
            return $"{RenderType(_ref.OfType)}[]";
        }
        return NormalizeType(_ref.Name);
    }

    private static string NormalizeType(string name)
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
        if (type.Kind == "ENUM" || IsNonNull(type, "ENUM") || type.Kind == "SCALAR" || IsNonNull(type, "SCALAR") || IsNonNull(type, "LIST"))
        {
            return $"async Task<{RenderType(type)}>";
        }
        return RenderType(type);
    }

    private static string RenderReturnValue(Field field)
    {
        var type = field.Type;
        if (type.Kind == "ENUM" || IsNonNull(type, "ENUM") || type.Kind == "SCALAR" || IsNonNull(type, "SCALAR") || IsNonNull(type, "LIST"))
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

        return "var arguments = ImmutableList<Argument>.Empty;";
    }

    private static bool IsNonNull(TypeRef type, string kind)
    {
        return type.Kind == "NON_NULL" && type.OfType.Kind == kind;
    }

    private static (IOrderedEnumerable<InputValue>, IOrderedEnumerable<InputValue>) SplitArguments(InputValue[] arguments)
    {
        var requiredArgs = arguments.Where(arg => arg.Type.Kind == "NON_NULL").OrderBy(type => type.Name);
        var optionalArgs = arguments.Where(arg => arg.Type.Kind != "NON_NULL").OrderBy(type => type.Name);
        return (requiredArgs, optionalArgs);
    }
}
