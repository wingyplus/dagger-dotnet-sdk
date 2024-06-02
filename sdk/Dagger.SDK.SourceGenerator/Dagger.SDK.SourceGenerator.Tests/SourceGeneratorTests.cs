using System.IO;
using System.Linq;

using Dagger.SDK.SourceGenerator.Tests.Utils;

using Microsoft.CodeAnalysis.CSharp;

using Xunit;

namespace Dagger.SDK.SourceGenerator.Tests;

public class SourceGeneratorTests
{
    [Fact]
    public void GenerateCodeBasedOnSchema()
    {
        var generator = new SourceGenerator();
        
        var driver = CSharpGeneratorDriver.Create(new[] { generator },
            new[]
            {
                new TestAdditionalFile("./introspection.json", TestData.Schema)
            });
        
        var compilation = CSharpCompilation.Create(nameof(SourceGeneratorTests));
        driver.RunGeneratorsAndUpdateCompilation(compilation, out var newCompilation, out _);
        
        var generatedFiles = newCompilation.SyntaxTrees
            .Select(t => Path.GetFileName(t.FilePath))
            .ToArray();

        Assert.Equivalent(new[] { "Dagger.SDK.g.cs" }, generatedFiles);
    }
}
