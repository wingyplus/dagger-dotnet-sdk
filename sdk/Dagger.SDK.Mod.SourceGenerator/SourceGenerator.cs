using System;
using System.Collections.Generic;
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
    private const string ObjectAttribute = "Dagger.SDK.Mod.ObjectAttribute";
    private const string FunctionAttribute = "Dagger.SDK.Mod.FunctionAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(postInitializationContext =>
        {
            postInitializationContext.AddSource("Dagger.SDK.Mod_Attributes.g.cs",
                GenerateSources.ModuleAttributesSource());
            postInitializationContext.AddSource("Dagger.SDK.Mod_Interfaces.g.cs",
                GenerateSources.ModuleInterfacesSource());
        });

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

        var objectWithFunctions = objects.Combine(functions.Collect()).Select(GroupIntoObjectContext);

        context.RegisterSourceOutput(objectWithFunctions, GenerateObjectTypeDef);
    }

    private ObjectContext GroupIntoObjectContext((ObjectContext Left, ImmutableArray<FunctionContext> Right) tuple,
        CancellationToken token)
    {
        var (objectContext, functionContexts) = tuple;

        objectContext.Functions = functionContexts.Where(context =>
        {
            var classDecl = (ClassDeclarationSyntax)context.Syntax.Parent!;
            return objectContext.Syntax.Equals(classDecl);
        });

        return objectContext;
    }

    private static FunctionContext ToFunctionContext(GeneratorAttributeSyntaxContext context, CancellationToken token)
    {
        return new FunctionContext
        {
            Syntax = (MethodDeclarationSyntax)context.TargetNode, Symbol = (IMethodSymbol)context.TargetSymbol
        };
    }

    private static bool IsPublicMethod(SyntaxNode node, CancellationToken token)
    {
        return node is MethodDeclarationSyntax methodDecl &&
               methodDecl.Modifiers.Any(SyntaxKind.PublicKeyword);
    }

    private static ObjectContext ToObjectContext(
        GeneratorAttributeSyntaxContext context,
        CancellationToken token)
    {
        var classDecl = (ClassDeclarationSyntax)context.TargetNode;
        var symbol = (INamedTypeSymbol)context.TargetSymbol;

        return new ObjectContext { Syntax = classDecl, Symbol = symbol };
    }

    private static bool IsPartialClass(SyntaxNode node, CancellationToken token)
    {
        return node is ClassDeclarationSyntax classDecl && classDecl.Modifiers.Any(SyntaxKind.PartialKeyword);
    }

    private static void GenerateObjectTypeDef(SourceProductionContext context,
        ObjectContext objectContext)
    {
        var ns = objectContext.Namespace;
        var className = objectContext.Name;

        var withFunctionDefs = objectContext
            .Functions
            .Select(static m => m.Name)
            .Select(static methodName => $"""
                                          .WithFunction(
                                              dag.Function("{methodName}", dag.TypeDef().WithKind(TypeDefKind.STRING_KIND))
                                          )
                                          """);

        var source = $$"""
                       using Dagger.SDK;

                       namespace {{ns}};

                       public partial class {{className}}
                       {
                           public TypeDef ToObjectTypeDef(Query dag)
                           {
                               var objTypeDef = dag.TypeDef().WithObject("{{className}}");
                               return objTypeDef
                               {{string.Join("\n", withFunctionDefs)}};
                           }
                       }
                       """;
        context.AddSource($"{className}_ObjectTypeDef.g.cs",
            SourceText.From(CSharpSource.Format(source), Encoding.UTF8));
    }

    private static void GenerateIDagSetter(SourceProductionContext context, ObjectContext objectContext)
    {
        var ns = objectContext.Namespace;
        var className = objectContext.Name;

        var source = $$"""
                       using Dagger.SDK;
                       using Dagger.SDK.Mod;

                       namespace {{ns}};

                       public partial class {{className}} : IDagSetter
                       {
                           private Query _dag;

                           public void SetDag(Query dag)
                           {
                               _dag = dag;
                           }
                       }
                       """;

        context.AddSource($"{className}.g.cs", SourceText.From(CSharpSource.Format(source), Encoding.UTF8));
    }
}
