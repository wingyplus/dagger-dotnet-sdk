using System.Collections.Immutable;
using System.Reflection;

using Basic.Reference.Assemblies;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using VerifyMSTest;

using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace Dagger.SDK.Mod.SourceGenerator.Tests;

[TestClass]
public class SourceGeneratorTests : VerifyBase
{
    [TestMethod]
    public Task TestGeneratePartialClass()
    {
        const string potatoSource = """
                                    namespace PotatoModule;

                                    [Dagger.SDK.Mod.Object]
                                    public partial class Potato {
                                    }
                                    """;

        var inputCompilation = CreateCompilation([(potatoSource, "Potato.cs")]);

        var generator = new SourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation,
            out var diagnostics);

        var outputDiagnostics = outputCompilation.GetDiagnostics();

        var files = outputCompilation.SyntaxTrees.Select(t => Path.GetFileName(t.FilePath)).ToArray();
        CollectionAssert.Contains(collection: files, element: "Potato.g.cs");

        // Assert.IsTrue(diagnostics.IsEmpty);
        // One from existing source and the one from generator.
        // Assert.AreEqual(2, outputCompilation.SyntaxTrees.Count());

        var runResult = driver.GetRunResult();

        Assert.IsTrue(runResult.Diagnostics.IsEmpty);

        var result = runResult.Results[0];
        Assert.AreEqual(2, result.GeneratedSources.Length);
        // Assert.IsTrue(result.Diagnostics.IsEmpty);

        return Verify(result.GeneratedSources[1].SourceText.ToString());
    }

    [TestMethod]
    public void TestGenerateOnlyClassThatHasObjectAttributeAnnotated()
    {
        const string potatoSource = """
                                    namespace PotatoModule;

                                    [Dagger.SDK.Mod.Object]
                                    public partial class Potato {}

                                    public partial class Tomato {}

                                    [Serializable]
                                    public partial class Carrot {}
                                    """;

        var inputCompilation = CreateCompilation([(potatoSource, "Potato.cs")]);

        var generator = new SourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation,
            out ImmutableArray<Diagnostic> _);

        var files = outputCompilation.SyntaxTrees.Select(t => Path.GetFileName(t.FilePath)).ToArray();
        Assert.AreEqual(2, files.Length);
        CollectionAssert.Contains(collection: files, element: "Potato.g.cs");
        CollectionAssert.DoesNotContain(collection: files, element: "Tomato.g.cs");
        CollectionAssert.DoesNotContain(collection: files, element: "Carrot.g.cs");
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
            ReferenceAssemblies.NetStandard20,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );
    }
}
