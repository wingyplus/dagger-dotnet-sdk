using System.Text;

using Microsoft.CodeAnalysis.Text;

namespace Dagger.SDK.Mod.SourceGenerator;

public static class GenerateSources
{
    public static SourceText ModuleInterfacesSource()
    {
        const string sourceText = """
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
                                      /// <param name="dag">The Dagger client instance.</param>
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
                                      /// <param name="dag">Dagger client instance.</param>
                                      /// <param name="module">The empty Dagger module.</param>
                                      /// <returns>The Dagger module with registered object.</returns>
                                      Module Register(Query dag, Module module);
                                  }
                                  """;
        return SourceText.From(CSharpSource.Format(sourceText), Encoding.UTF8);
    }

    public static SourceText ModuleAttributesSource()
    {
        const string sourceText = """
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
                                  """;
        return SourceText.From(CSharpSource.Format(sourceText), Encoding.UTF8);
    }
}
