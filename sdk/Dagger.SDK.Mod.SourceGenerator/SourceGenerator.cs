using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Dagger.SDK.Mod.SourceGenerator;

/// <summary>
/// Generate source code for running Dagger Function.
/// </summary>
[Generator(LanguageNames.CSharp)]
public class SourceGenerator : IIncrementalGenerator
{
    private const string EntrypointAttribute = "Dagger.SDK.Mod.EntrypointAttribute";
    private const string ObjectAttribute = "Dagger.SDK.Mod.ObjectAttribute";
    private const string FunctionAttribute = "Dagger.SDK.Mod.FunctionAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var entrypoint = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: EntrypointAttribute,
            predicate: IsPartialClass,
            transform: ToObjectContext
        );

        context.RegisterSourceOutput(entrypoint, GenerateEntrypoint);

        var objects = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: ObjectAttribute,
            predicate: IsPartialClass,
            transform: ToObjectContext
        );

        context.RegisterSourceOutput(objects, GenerateIDagSetter);

        var functions = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: FunctionAttribute,
            predicate: IsPublicMethod,
            transform: ToFunctionContext
        );

        var objectWithFunctions = objects
            .Combine(functions.Collect())
            .Select(GroupIntoObjectContext);

        context.RegisterSourceOutput(objectWithFunctions, GenerateObjectTypeDef);
    }

    private T ToEntrypointContext<T>(GeneratorAttributeSyntaxContext arg1, CancellationToken arg2)
    {
        throw new NotImplementedException();
    }

    private ObjectContext GroupIntoObjectContext(
        (ObjectContext Left, ImmutableArray<FunctionContext> Right) tuple,
        CancellationToken token
    )
    {
        var (objectContext, functionContexts) = tuple;

        objectContext.Functions = functionContexts.Where(context =>
        {
            var classDecl = (ClassDeclarationSyntax)context.Syntax.Parent!;
            return objectContext.Syntax.Equals(classDecl);
        });

        return objectContext;
    }

    private static FunctionContext ToFunctionContext(
        GeneratorAttributeSyntaxContext context,
        CancellationToken token
    )
    {
        return new FunctionContext
        {
            Syntax = (MethodDeclarationSyntax)context.TargetNode,
            Symbol = (IMethodSymbol)context.TargetSymbol,
        };
    }

    private static bool IsPublicMethod(SyntaxNode node, CancellationToken token)
    {
        return node is MethodDeclarationSyntax methodDecl
            && methodDecl.Modifiers.Any(SyntaxKind.PublicKeyword);
    }

    private static ObjectContext ToObjectContext(
        GeneratorAttributeSyntaxContext context,
        CancellationToken token
    )
    {
        var classDecl = (ClassDeclarationSyntax)context.TargetNode;
        var symbol = (INamedTypeSymbol)context.TargetSymbol;

        return new ObjectContext { Syntax = classDecl, Symbol = symbol };
    }

    private static bool IsPartialClass(SyntaxNode node, CancellationToken token)
    {
        return node is ClassDeclarationSyntax classDecl
            && classDecl.Modifiers.Any(SyntaxKind.PublicKeyword)
            && classDecl.Modifiers.Any(SyntaxKind.PartialKeyword);
    }

    private static void GenerateObjectTypeDef(
        SourceProductionContext context,
        ObjectContext objectContext
    )
    {
        var ns = objectContext.Namespace;
        var name = objectContext.Name;

        var withFunctionDefs = objectContext
            .Functions.Select(static m => m.Name)
            .Select(static methodName =>
                $"""
                .WithFunction(
                    dag.Function("{methodName}", dag.TypeDef().WithKind(Dagger.SDK.TypeDefKind.STRING_KIND))
                )
                """
            );

        var source = $$"""
            namespace {{ns}};

            public partial class {{name}}
            {
                public Dagger.SDK.TypeDef ToObjectTypeDef(Dagger.SDK.Query dag)
                {
                    var objTypeDef = dag.TypeDef().WithObject("{{name}}");
                    return objTypeDef
                    {{string.Join("\n", withFunctionDefs)}};
                }
            }
            """;
        context.AddSource(
            $"{objectContext.Name}_ObjectTypeDef.g.cs",
            SourceText.From(CSharpSource.Format(source), Encoding.UTF8)
        );
    }

    private static void GenerateIDagSetter(
        SourceProductionContext context,
        ObjectContext objectContext
    )
    {
        var ns = objectContext.Namespace;
        var name = objectContext.Name;

        var source = $$"""
            namespace {{ns}};

            public partial class {{name}} : Dagger.SDK.Mod.IDagSetter
            {
                private Dagger.SDK.Query _dag;

                public void SetDag(Dagger.SDK.Query dag)
                {
                    _dag = dag;
                }
            }
            """;

        context.AddSource(
            $"{name}_IDagSetter.g.cs",
            SourceText.From(CSharpSource.Format(source), Encoding.UTF8)
        );
    }

    private static void GenerateEntrypoint(
        SourceProductionContext context,
        ObjectContext objectContext
    )
    {
        var ns = objectContext.Namespace;
        var name = objectContext.Name;

        var entrypoint = $$"""
            namespace {{ns}};

            public partial class {{name}} : Dagger.SDK.Mod.IEntrypoint
            {
                public Dagger.SDK.Module Register(Dagger.SDK.Query dag, Dagger.SDK.Module module)
                {
                    return module.WithObject(ToObjectTypeDef(dag));
                }

                public object Invoke(string name, Dictionary<string, System.Text.Json.JsonElement> args) {
                    return null;
                }
            }
            """;
        context.AddSource(
            $"{name}_IEntrypoint.g.cs",
            SourceText.From(CSharpSource.Format(entrypoint), Encoding.UTF8)
        );

        var main = $$"""
            namespace {{ns}};

            public static class Entrypoint
            {
                public static async Task Invoke(string[] args)
                {
                    var dag = Dagger.SDK.Dagger.Connect();
                    await Dagger.SDK.Mod.ModuleEntrypoint.Invoke<{{name}}>(dag);
                }
            }
            """;

        context.AddSource(
            "Entrypoint.g.cs",
            SourceText.From(CSharpSource.Format(main), Encoding.UTF8)
        );
    }
}
