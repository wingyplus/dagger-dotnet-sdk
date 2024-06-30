using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Dagger.SDK.Mod.SourceGenerator;

public struct FunctionContext
{
    public MethodDeclarationSyntax Syntax { get; set; }
    public IMethodSymbol Symbol { get; set; }
    public string Name { get => Symbol.Name; }
}
