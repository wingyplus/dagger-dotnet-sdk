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
                                  """;
        return SourceText.From(sourceText, Encoding.UTF8);
    }

    public static SourceText ModuleAttributesSource()
    {
        const string sourceText = """
                                  using System;

                                  namespace Dagger.SDK.Mod;

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
        return SourceText.From(sourceText, Encoding.UTF8);
    }
}
