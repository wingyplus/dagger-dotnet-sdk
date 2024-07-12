using System.Collections.Immutable;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;

namespace Dagger.SDK.Mod.SourceGenerator.Tests;

[TestClass]
public partial class SourceGeneratorTests
{
    private readonly (string, SourceText) _modAttributeSource = (
        @"Dagger.SDK.Mod.SourceGenerator\Dagger.SDK.Mod.SourceGenerator.SourceGenerator\Dagger.SDK.Mod_Attributes.g.cs",
        SourceText.From(
            """
            using System;

            namespace Dagger.SDK.Mod;
            /// <summary>
            /// Treat the class as the root of Module.
            /// </summary>
            [AttributeUsage(AttributeTargets.Class)]
            public sealed class EntrypointAttribute : Attribute;
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
            """,
            Encoding.UTF8
        )
    );

    private readonly (string, SourceText) _modInterfaceSource = (
        @"Dagger.SDK.Mod.SourceGenerator\Dagger.SDK.Mod.SourceGenerator.SourceGenerator\Dagger.SDK.Mod_Interfaces.g.cs",
        SourceText.From(
            """
            namespace Dagger.SDK.Mod;
            /// <summary>
            /// An interface for module runtime to inject Dagger client instance to the
            /// object class.
            /// </summary>
            public interface IDagSetter
            {
                /// <summary>
                /// Set Dagger client instance.
                /// </summary>
                /// <param name = "dag">The Dagger client instance.</param>
                void SetDag(Query dag);
            }

            /// <summary>
            /// An interface for invoking the module class.
            /// </summary>
            public interface IEntrypoint
            {
                /// <summary>
                /// Register an object as the root of module.
                /// </summary>
                /// <param name = "dag">Dagger client instance.</param>
                /// <param name = "module">The empty Dagger module.</param>
                /// <returns>The Dagger module with registered object.</returns>
                Module Register(Query dag, Module module);
            }
            """,
            Encoding.UTF8
        )
    );

    private readonly (string, SourceText) _potatoIDagSetterSource = (
        @"Dagger.SDK.Mod.SourceGenerator\Dagger.SDK.Mod.SourceGenerator.SourceGenerator\Potato_IDagSetter.g.cs",
        SourceText.From(
            """
            namespace PotatoModule;
            public partial class Potato : Dagger.SDK.Mod.IDagSetter
            {
                private Dagger.SDK.Query _dag;
                public void SetDag(Dagger.SDK.Query dag)
                {
                    _dag = dag;
                }
            }
            """,
            Encoding.UTF8
        )
    );

    [TestMethod]
    public async Task TestGenerateFiles()
    {
        await new CSharpSourceGeneratorTest<SourceGenerator, DefaultVerifier>
        {
            CompilerDiagnostics = CompilerDiagnostics.None,
            TestState =
            {
                Sources =
                {
                    """
                    namespace PotatoModule;

                    [Dagger.SDK.Mod.Object]
                    public partial class Potato {}

                    public partial class Tomato {}

                    [Serializable]
                    public partial class Carrot {}
                    """
                },
                GeneratedSources =
                {
                    _modAttributeSource,
                    _modInterfaceSource,
                    _potatoIDagSetterSource,
                    (
                        @"Dagger.SDK.Mod.SourceGenerator\Dagger.SDK.Mod.SourceGenerator.SourceGenerator\Potato_ObjectTypeDef.g.cs",
                        SourceText.From(
                            """
                            namespace PotatoModule;
                            public partial class Potato
                            {
                                public Dagger.SDK.TypeDef ToObjectTypeDef(Dagger.SDK.Query dag)
                                {
                                    var objTypeDef = dag.TypeDef().WithObject("Potato");
                                    return objTypeDef;
                                }
                            }
                            """,
                            Encoding.UTF8
                        )
                    )
                }
            }
        }.RunAsync();
    }

    [TestMethod]
    public async Task TestGenerateObjectTypeDef()
    {
        await new CSharpSourceGeneratorTest<SourceGenerator, DefaultVerifier>
        {
            CompilerDiagnostics = CompilerDiagnostics.None,
            TestState =
            {
                Sources =
                {
                    """
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
                    """
                },
                GeneratedSources =
                {
                    _modAttributeSource,
                    _modInterfaceSource,
                    _potatoIDagSetterSource,
                    (
                        @"Dagger.SDK.Mod.SourceGenerator\Dagger.SDK.Mod.SourceGenerator.SourceGenerator\Potato_ObjectTypeDef.g.cs",
                        SourceText.From(
                            """
                            namespace PotatoModule;
                            public partial class Potato
                            {
                                public Dagger.SDK.TypeDef ToObjectTypeDef(Dagger.SDK.Query dag)
                                {
                                    var objTypeDef = dag.TypeDef().WithObject("Potato");
                                    return objTypeDef.WithFunction(dag.Function("Hello", dag.TypeDef().WithKind(Dagger.SDK.TypeDefKind.STRING_KIND))).WithFunction(dag.Function("Hello2", dag.TypeDef().WithKind(Dagger.SDK.TypeDefKind.STRING_KIND)));
                                }
                            }
                            """,
                            Encoding.UTF8
                        )
                    )
                }
            }
        }.RunAsync();
    }

    [TestMethod]
    public async Task TestGenerateEntrypoint()
    {
        await new CSharpSourceGeneratorTest<SourceGenerator, DefaultVerifier>
        {
            CompilerDiagnostics = CompilerDiagnostics.None,
            TestState =
            {
                Sources =
                {
                    """
                    using Mod = Dagger.SDK.Mod;

                    namespace PotatoModule;

                    [Dagger.SDK.Mod.Object]
                    [Dagger.SDK.Mod.Entrypoint]
                    public partial class Potato {
                    }
                    """
                },
                GeneratedSources =
                {
                    _modAttributeSource,
                    _modInterfaceSource,
                    (@"Dagger.SDK.Mod.SourceGenerator\Dagger.SDK.Mod.SourceGenerator.SourceGenerator\Potato_IEntrypoint.g.cs",
                        SourceText.From("""
                                        namespace PotatoModule;
                                        public partial class Potato : Dagger.SDK.Mod.IEntrypoint
                                        {
                                            public Dagger.SDK.Module Register(Dagger.SDK.Query dag, Dagger.SDK.Module module)
                                            {
                                                return module.WithObject(ToObjectTypeDef(dag));
                                            }
                                        }
                                        """,
                            Encoding.UTF8
                        )
                    ),
                    (@"Dagger.SDK.Mod.SourceGenerator\Dagger.SDK.Mod.SourceGenerator.SourceGenerator\Entrypoint.g.cs",
                        SourceText.From(
                            """
                            namespace PotatoModule;
                            public static class Entrypoint
                            {
                                public static async Task Invoke(string[] args)
                                {
                                    var dag = Dagger.SDK.Dagger.Connect();
                                    await Invoke<Potato>(dag);
                                }

                                private static async Task Invoke<T>(Dagger.SDK.Query dag)
                                    where T : class, Dagger.SDK.Mod.IDagSetter, Dagger.SDK.Mod.IEntrypoint, new()
                                {
                                    T root = new();
                                    var fnCall = dag.CurrentFunctionCall();
                                    var parentName = await fnCall.ParentName();
                                    // TODO: Get module name to check root type name match with it.
                                    var result = parentName switch
                                    {
                                        // TODO: Dagger.SDK should automatic serialize into id.
                                        "" => await root.Register(dag, dag.Module()).Id(),
                                        _ => throw new Exception($"{parentName} is not supported at the moment.")};
                                    await fnCall.ReturnValue(IntoJson(result));
                                }

                                private static Dagger.SDK.Json IntoJson(object result)
                                {
                                    return new Dagger.SDK.Json
                                    {
                                        Value = System.Text.Json.JsonSerializer.Serialize(result)
                                    };
                                }
                            }
                            """,
                            Encoding.UTF8
                        )
                    ),
                    _potatoIDagSetterSource,
                    (
                        @"Dagger.SDK.Mod.SourceGenerator\Dagger.SDK.Mod.SourceGenerator.SourceGenerator\Potato_ObjectTypeDef.g.cs",
                        SourceText.From(
                            """
                            namespace PotatoModule;
                            public partial class Potato
                            {
                                public Dagger.SDK.TypeDef ToObjectTypeDef(Dagger.SDK.Query dag)
                                {
                                    var objTypeDef = dag.TypeDef().WithObject("Potato");
                                    return objTypeDef;
                                }
                            }
                            """,
                            Encoding.UTF8
                        )
                    )
                }
            }
        }.RunAsync();
    }
}
