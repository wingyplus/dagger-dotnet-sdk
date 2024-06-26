using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using VerifyMSTest;

using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace Dagger.SDK.Mod.SourceGenerator.Tests;

[TestClass]
public partial class SourceGeneratorTests : VerifyBase
{
    [TestMethod]
    public Task TestGeneratePartialClass()
    {
        const string potatoSource = """
                                   namespace Potato;

                                   [Dagger.SDK.Mod.Object]
                                   public partial class Potato {
                                   }
                                   """;

        var inputCompilation = CreateCompilation([(potatoSource, "Potato.cs")]);

        var generator = new SourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation,
            out var diagnostics);


        var files = outputCompilation.SyntaxTrees.Select(t => Path.GetFileName(t.FilePath)).ToArray();
        CollectionAssert.Contains(collection: files, element: "Potato.g.cs", message: "Generated file not found.");

        Assert.IsTrue(diagnostics.IsEmpty);
        // One from existing source and the one from generator.
        Assert.AreEqual(2, outputCompilation.SyntaxTrees.Count());

        var runResult = driver.GetRunResult();

        Assert.IsTrue(runResult.Diagnostics.IsEmpty);

        var result = runResult.Results[0];
        Assert.AreEqual(1, result.GeneratedSources.Length);
        return Verify(result.GeneratedSources[0].SourceText.ToString());
    }

    private static CSharpCompilation CreateCompilation((string, string)[] sources)
    {
        return CSharpCompilation.Create(
            nameof(SourceGeneratorTests),
            sources.Select(source =>
            {
                var (content, filename) = source;
                return CSharpSyntaxTree.ParseText(content, path: filename);
            }),
            new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
            new CSharpCompilationOptions(OutputKind.ConsoleApplication)
        );
    }
}
