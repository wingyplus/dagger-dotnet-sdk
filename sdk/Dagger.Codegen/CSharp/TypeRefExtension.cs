namespace Dagger.Codegen.CSharp;

public static class TypeRefExtension
{
    // <summary>
    // Get a type from TypeRef.
    //
    // This method doesn't indicate the type is nullable or not. The caller
    // must detecting it from TypeRef object by themself.
    // </summary>
    public static string Type(this TypeRef typeRef)
    {
        var tr = typeRef.GetType_();
        if (tr.IsList())
        {
            return $"{tr.OfType.Type()}[]";
        }
        return Formatter.FormatType(tr.Name);
    }
}
