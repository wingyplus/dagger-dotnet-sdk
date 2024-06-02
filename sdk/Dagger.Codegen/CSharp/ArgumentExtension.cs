namespace Dagger.Codegen.CSharp;

public static class ArgumentExtension
{
    // <summary>
    // Convert argument name into C# variable name.
    // </summary>
    public static string VarName(this InputValue arg)
    {
        return Formatter.FormatVarName(arg.Name);
    }
}
