using System.IO;
using System.Linq;

using Dagger.SDK.SourceGenerator.Tests.Utils;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dagger.SDK.SourceGenerator.Tests;

[TestClass]
public class SourceGeneratorTests
{
    [TestMethod]
    [DataRow("./introspection.json", TestData.Schema)]
    public void GenerateCodeBasedOnSchema(string path, string text)
    {
        var generator = new SourceGenerator();

        var driver = CSharpGeneratorDriver.Create(new[] { generator },
            new[]
            {
                new TestAdditionalFile(path, text)
            });

        var compilation = CSharpCompilation.Create(nameof(SourceGeneratorTests));
        driver.RunGeneratorsAndUpdateCompilation(compilation, out var newCompilation, out _);

        var generatedFiles = newCompilation.SyntaxTrees
            .Select(t => Path.GetFileName(t.FilePath))
            .ToArray();

        CollectionAssert.Contains(generatedFiles,  "Dagger.SDK.g.cs", "Generated file not found.");
    }
}
