using System.Collections.Immutable;
using System.Reflection;

using Basic.Reference.Assemblies;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using VerifyMSTest;

using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace Dagger.SDK.Mod.SourceGenerator.Tests;

[TestClass]
[UsesVerify]
public partial class SourceGeneratorTests
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

        return Verify(driver.GetRunResult());
    }

    [TestMethod]
    public Task TestGenerateParOnlyClassThatHasObjectAttributeAnnotated()
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
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation,
            out ImmutableArray<Diagnostic> _);

        return Verify(driver.GetRunResult());
    }

    [TestMethod]
    public Task TestGenerateToObjectTypeDef_RenderPublicMethodWithFunctionAttribute()
    {
        const string potatoSource = """
                                    using Mod = Dagger.SDK.Mod;

                                    namespace PotatoModule;

                                    [Dagger.SDK.Mod.Object]
                                    public partial class Potato {
                                        [Dagger.SDK.Mod.Function]
                                        public string Hello(string name) {
                                           return $"Hello, {name}";
                                        }

                                        [Mod.Function]
                                        public string Hello2(string name) {
                                           return $"Hello, {name}";
                                        }

                                        public string PublicWithoutAttributeFunction() {
                                            return "Should not add to object type def function";
                                        }

                                        private string PrivateFunction() {
                                           return "Should not add to object type def function";
                                        }
                                    }
                                    """;

        var inputCompilation = CreateCompilation([(potatoSource, "Potato.cs")]);

        var generator = new SourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation,
            out var diagnostics);

        return Verify(driver
            .GetRunResult()
            .Results
            .SelectMany(runResult => runResult.GeneratedSources)
            .Where(sourceResult => sourceResult.SyntaxTree.FilePath.EndsWith("Potato_ObjectTypeDef.g.cs"))
        );
    }

    [TestMethod]
    public Task TestGenerateEntrypoint()
    {
        const string potatoSource = """
                                    using Mod = Dagger.SDK.Mod;

                                    namespace PotatoModule;

                                    [Dagger.SDK.Mod.Object]
                                    [Dagger.SDK.Mod.Entrypoint]
                                    public partial class Potato {
                                    }
                                    """;

        var inputCompilation = CreateCompilation([(potatoSource, "Potato.cs")]);

        var generator = new SourceGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation,
            out var diagnostics);

        return Verify(driver.GetRunResult());
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
