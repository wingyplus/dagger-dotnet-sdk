using System;
using System.Linq;
using System.Text;

using Dagger.SDK.SourceGenerator.Extensions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using Dagger.SDK.SourceGenerator.Types;

using Type = Dagger.SDK.SourceGenerator.Types.Type;

namespace Dagger.SDK.SourceGenerator.Code;

public class CodeRenderer : ICodeRenderer
{
    public string RenderPre()
    {
        return """
               #nullable enable

               using System.Collections.Immutable;
               using System.Text.Json.Serialization;

               using Dagger.SDK.GraphQL;
               using Dagger.SDK.JsonConverters;

               namespace Dagger.SDK;

               public class Scalar
               {
                   public string Value;
               
                   public override string ToString() => Value;
               }

               public class Object(QueryBuilder queryBuilder, GraphQLClient gqlClient)
               {
                   public QueryBuilder QueryBuilder { get; } = queryBuilder;
                   public GraphQLClient GraphQLClient { get; } = gqlClient;
               }

               public interface IInputObject {
                   List<KeyValuePair<string, Value>> ToKeyValuePairs();
               }
               """;
    }

    // TODO: test value converter.
    public string RenderEnum(Type type)
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

    public string RenderInputObject(Type type)
    {
        var properties = type.InputFields.Select(field => $$"""
                                                            {{RenderDocComment(field)}}
                                                            public {{field.Type.GetTypeName()}} {{Formatter.FormatProperty(field.Name)}} { get; } = {{field.GetVarName()}};
                                                            """);

        var constructorFields =
            type.InputFields.Select(field => $"""{field.Type.GetTypeName()} {field.GetVarName()}""");

        var toKeyValuePairsProperties = type.InputFields.Select(field => $"""
                                                                          kvPairs.Add(KeyValuePair.Create("{field.Name}", {RenderArgumentValue(field, asProperty: true)} as Value));
                                                                          """);

        var toKeyValuePairsMethod = $$"""
                                      public List<KeyValuePair<string,Value>> ToKeyValuePairs()
                                      {
                                          var kvPairs = new List<KeyValuePair<string, Value>>();
                                          {{string.Join("\n", toKeyValuePairsProperties)}}
                                          return kvPairs;
                                      }
                                      """;
        return $$"""
                 {{RenderDocComment(type)}}
                 public struct {{type.Name}}({{string.Join(", ", constructorFields)}}) : IInputObject
                 {
                     {{string.Join("\n\n", properties)}}
                     {{toKeyValuePairsMethod}}
                 }
                 """;
    }

    public string RenderObject(Type type)
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
                     {{RenderObsolete(field)}}
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

    public string RenderScalar(Type type)
    {
        return $$"""
                 {{RenderDocComment(type)}}
                 [JsonConverter(typeof(ScalarIDConverter<{{type.Name}}>))]
                 public class {{type.Name}} : Scalar 
                 {
                 }
                 """;
    }

    public string Format(string source)
    {
        return CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace(eol: "\n").ToFullString();
    }

    private static string RenderObsolete(Field field)
    {
        return !field.IsDeprecated ? "" : $"[Obsolete(\"{field.DeprecationReason}\")]";
    }

    private static string RenderDocComment(Type type)
    {
        return RenderSummaryDocComment(type.Description);
    }

    private static string RenderDocComment(Field field)
    {
        var builder = new StringBuilder();
        builder.AppendLine(RenderSummaryDocComment(field.Description));
        builder = field.Args.Aggregate(builder, (sb, arg) =>
        {
            string[] lines = arg.Description.Split('\n');
            sb.AppendLine($"/// <param name=\"{arg.GetVarName()}\">");
            foreach (var line in lines)
            {
                sb.AppendLine($"/// {line}");
            }

            sb.AppendLine($"/// </param>");
            return sb;
        });
        return builder.ToString();
    }

    private static string RenderDocComment(InputValue field)
    {
        return RenderSummaryDocComment(field.Description);
    }

    private static string RenderSummaryDocComment(string doc)
    {
        if (string.IsNullOrEmpty(doc))
        {
            return "";
        }

        var description = doc
            .Split('\n')
            .Select(line => $"/// {line}")
            .Select(line => line.Trim());
        return $"""
                /// <summary>
                {string.Join("\n", description)}
                /// </summary>
                """;
    }

    private static string RenderArgument(InputValue argument)
    {
        return $"{argument.Type.GetTypeName()} {argument.GetVarName()}";
    }

    private static string RenderOptionalArgument(InputValue argument)
    {
        return $"{argument.Type.GetTypeName()}? {argument.GetVarName()} = null";
    }

    private static string RenderReturnType(TypeRef type)
    {
        if (type.IsLeaf() || type.IsList())
        {
            return $"async Task<{type.GetTypeName()}>";
        }

        return type.GetTypeName();
    }

    private static string RenderReturnValue(Field field)
    {
        var type = field.Type;

        if (type.IsLeaf())
        {
            return $"await Engine.Execute<{field.Type.GetTypeName()}>(GraphQLClient, queryBuilder)";
        }

        if (type.IsList() && type.GetType_().OfType.IsObject())
        {
            var typeName = type.GetType_().OfType.GetTypeName();
            return $"""
                    (await Engine.ExecuteList<{typeName}ID>(GraphQLClient, queryBuilder))
                        .Select(id =>
                            new {typeName}(
                                QueryBuilder.Builder().Select("load{type}FromID", [new Argument("id", new StringValue(id.Value))]),
                                GraphQLClient
                            )
                        )
                        .ToArray();
                    """;
        }

        if (type.IsList() && type.GetType_().OfType.IsScalar())
        {
            var typeName = type.GetType_().OfType.GetTypeName();
            return $"await Engine.ExecuteList<{typeName}>(GraphQLClient, queryBuilder);";
        }

        return $"new {field.Type.GetTypeName()}(queryBuilder, GraphQLClient)";
    }

    private object RenderArgumentBuilder(Field field)
    {
        if (field.Args.Length == 0)
        {
            return "";
        }


        var builder = new StringBuilder("var arguments = ImmutableList<Argument>.Empty;");
        builder.Append('\n');

        var requiredArgs = field.RequiredArgs();
        if (requiredArgs.Count() > 0)
        {
            builder.Append("arguments = arguments.")
                .Append(string.Join(".",
                    requiredArgs.Select(arg =>
                        $$"""Add(new Argument("{{arg.Name}}", {{RenderArgumentValue(arg)}}))"""))).Append(';');
            builder.Append('\n');
        }

        var optionalArgs = field.OptionalArgs();
        if (optionalArgs.Count() > 0)
        {
            optionalArgs.Aggregate(builder, (builder, arg) =>
                {
                    var varName = arg.GetVarName();
                    return builder
                        .Append($"""if ({varName} is {arg.Type.GetTypeName()} {varName}_)""")
                        .Append("{\n")
                        .Append(
                            $$"""    arguments = arguments.Add(new Argument("{{arg.Name}}", {{RenderArgumentValue(arg, addVarSuffix: true)}}));""")
                        .Append("}\n");
                })
                .Append("\n");
        }

        return builder.ToString();
    }

    private static string RenderArgumentValue(InputValue arg, bool addVarSuffix = false, bool asProperty = false)
    {
        var argName = arg.GetVarName();
        if (addVarSuffix)
        {
            argName = $"{argName}_";
        }

        if (asProperty)
        {
            argName = Formatter.FormatProperty(argName);
        }

        if (arg.Type.IsScalar())
        {
            var type = arg.Type.GetTypeName();
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
            return $"new ObjectValue({argName}.ToKeyValuePairs())";
        }

        if (arg.Type.IsList())
        {
            var tr = arg.Type.GetType_().OfType.GetType_();

            if (tr.IsScalar())
            {
                var value = tr.GetType_().Name switch
                {
                    "String" => "new StringValue(v)",
                    "Integer" => "new IntValue(v)",
                    "Float" => "new FloatValue(v)",
                    "Boolean" => "new BooleanValue(v)",
                    _ => "new StringValue(v.Value)"
                };

                return $"new ListValue({argName}.Select(v => {value} as Value).ToList())";
            }

            if (tr.IsInputObject())
            {
                return $"new ListValue({argName}.Select(v => new ObjectValue(v.ToKeyValuePairs()) as Value).ToList())";
            }
        }

        throw new Exception($"The type {arg.Type.OfType.Kind} should not be enter here.");
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
        if (field.Type.IsList() && !field.Type.GetType_().OfType.IsLeaf())
        {
            builder.Append(".Select(\"id\")");
        }

        builder.Append(';');
        return builder.ToString();
    }
}
