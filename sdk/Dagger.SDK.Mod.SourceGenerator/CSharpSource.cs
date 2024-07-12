using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Dagger.SDK.Mod.SourceGenerator;

public static class CSharpSource
{
    /// <summary>
    /// Formatting CSharp source code.
    /// </summary>
    /// <param name="source">The source code string.</param>
    /// <returns></returns>
    public static string Format(string source) =>
        CSharpSyntaxTree.ParseText(source).GetRoot().NormalizeWhitespace(eol: "\n").ToFullString();
}
