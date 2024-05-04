using CaseExtensions;

namespace Dagger.Codegen.CSharp;

internal class Formatter
{
    public static string FormatMethod(string name)
    {
        return name.ToPascalCase();
    }

    public static string FormatProperty(string name)
    {
        return name.ToPascalCase();
    }
}
