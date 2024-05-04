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
        using Dagger.SDK;

        public class Scalar
        {
            public readonly string Value;

            public override string ToString() => Value;
        }

        public class Object
        {
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
            var requiredArgs = field.Args.Where(arg => arg.Type.Kind == "NON_NULL").OrderBy(type => type.Name).Select(RenderArgument);
            var optionalArgs = field.Args.Where(arg => arg.Type.Kind != "NON_NULL").OrderBy(type => type.Name).Select(RenderOptionalArgument);
            var args = requiredArgs.Concat(optionalArgs);

            return $$"""
            {{RenderDocComment(field)}}
            public {{RenderType(field.Type)}} {{Formatter.FormatMethod(field.Name)}}({{string.Join(",", args)}}) {}
            """;
        });

        return $$"""
        {{RenderDocComment(type)}}
        public class {{type.Name}} : Object
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
        return CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace().ToFullString();
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
        return $"{RenderType(argument.Type)} {argument.Name}";
    }

    private static string RenderOptionalArgument(InputValue argument)
    {
        return $"{RenderArgument(argument)} = {argument.DefaultValue ?? "null"}";
    }

    private static string RenderType(TypeRef type)
    {
        if (type.Kind == "NON_NULL")
        {
            return NormalizeType(type.OfType.Name);
        }
        else
        {
            return NormalizeType(type.Name);
        }
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
}
