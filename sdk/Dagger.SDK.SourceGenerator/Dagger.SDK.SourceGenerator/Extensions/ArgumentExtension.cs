using Dagger.SDK.SourceGenerator.Code;

namespace Dagger.SDK.SourceGenerator;

public static class ArgumentExtension
{
    // <summary>
    // Convert argument name into C# variable name.
    // </summary>
    public static string GetVarName(this InputValue arg)
    {
        return Formatter.FormatVarName(arg.Name);
    }
}
