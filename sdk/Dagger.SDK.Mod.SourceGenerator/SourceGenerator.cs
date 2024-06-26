using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Dagger.SDK.Mod.SourceGenerator;

/// <summary>
/// Generate source code for running Dagger Function.
/// </summary>
[Generator(LanguageNames.CSharp)]
public class SourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: SyntaxPredicate,
                transform: IntoClassDeclaration
            )
            .Where(m => m is not null);

        var compilation = context.CompilationProvider.Combine(provider.Collect());

        context.RegisterSourceOutput(compilation, ExecuteGeneration);
    }

    private static void ExecuteGeneration(SourceProductionContext context,
        (Compilation Left, ImmutableArray<ClassDeclarationSyntax> Right) tuple)
    {
        var (compilation, syntaxes) = tuple;

        var symbols = syntaxes
            .Select(syntax =>
                compilation.GetSemanticModel(syntax.SyntaxTree).GetDeclaredSymbol(syntax) as INamedTypeSymbol)
            .Aggregate(context, (ctx, namedSymbol) =>
            {
                var ns = namedSymbol.ContainingNamespace.Name;
                var className = namedSymbol.Name;

                var source = $$"""
                               using Dagger.SDK;

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

                ctx.AddSource($"{className}.g.cs", source);
                return ctx;
            });
    }

    private static bool SyntaxPredicate(SyntaxNode node, CancellationToken token)
    {
        // Support only partial class that has at least one attribute.
        return node is ClassDeclarationSyntax { AttributeLists.Count: > 0 } classDeclaration &&
               classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword);
    }

    private static ClassDeclarationSyntax IntoClassDeclaration(GeneratorSyntaxContext context, CancellationToken token)
    {
        return (ClassDeclarationSyntax)context.Node;
    }
}
