using System.Collections.Generic;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Dagger.SDK.Mod.SourceGenerator;

public struct ObjectContext
{
    public ClassDeclarationSyntax Syntax { get; set; }
    public INamedTypeSymbol Symbol { get; set; }

    public IEnumerable<FunctionContext> Functions { get; set; }

    /// <summary>
    /// The namespace of an object.
    /// </summary>
    public string Namespace
    {
        get => Symbol.ContainingNamespace?.ToDisplayString(
            SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle
                .Omitted));
    }

    /// <summary>
    /// The object name.
    /// </summary>
    public string Name
    {
        get => Symbol.Name;
    }
}
