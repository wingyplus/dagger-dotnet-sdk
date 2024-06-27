using System;
using System.Collections.Immutable;
using System.Diagnostics;
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

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(postInitializationContext =>
        {
            postInitializationContext.AddSource("Dagger.SDK.ModAttributes.g.cs", """
                using System;
                
                namespace Dagger.SDK.Mod;

                /// <summary>
                /// Expose the class as a Dagger.ObjectTypeDef.
                /// </summary>
                [AttributeUsage(AttributeTargets.Class)]
                public sealed class ObjectAttribute : Attribute;

                /// <summary>
                /// Expose the class as a Dagger.Function.
                /// </summary>
                [AttributeUsage(AttributeTargets.Method)]
                public sealed class FunctionAttribute : Attribute;
                """);
        });
        var objectClasses = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: ObjectAttribute,
            predicate: IsPartialClass,
            transform: ExtractTarget
        );

        context.RegisterSourceOutput(objectClasses, GenerateIDagSetter);
    }

    private static (ClassDeclarationSyntax classDef, INamedTypeSymbol classSymbol) ExtractTarget(
        GeneratorAttributeSyntaxContext context,
        CancellationToken token)
    {
        var classDef = (ClassDeclarationSyntax)context.TargetNode;
        var classSymbol = (INamedTypeSymbol)context.TargetSymbol;
        return (classDef, classSymbol);
    }

    private static bool IsPartialClass(SyntaxNode node, CancellationToken token)
    {
        return node is ClassDeclarationSyntax classDef && classDef.Modifiers.Any(SyntaxKind.PartialKeyword);
    }

    private static void GenerateIDagSetter(SourceProductionContext context,
        (ClassDeclarationSyntax classDef, INamedTypeSymbol classSymbol) tuple)
    {
        (ClassDeclarationSyntax classDef, INamedTypeSymbol symbol) = tuple;
        var ns = symbol.ContainingNamespace?.ToDisplayString(
            SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle
                .Omitted));
        var className = symbol.Name;

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

        context.AddSource($"{className}.g.cs", SourceText.From(source, Encoding.UTF8));
    }
}
