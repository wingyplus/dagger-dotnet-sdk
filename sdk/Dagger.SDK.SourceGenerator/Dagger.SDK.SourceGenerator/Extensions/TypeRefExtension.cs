using Dagger.SDK.SourceGenerator.Code;
namespace  Dagger.SDK.SourceGenerator;

public static class TypeRefExtension
{
    // <summary>
    // Get a type from TypeRef.
    //
    // This method doesn't indicate the type is nullable or not. The caller
    // must detecting it from TypeRef object by themself.
    // </summary>
    public static string GetTypeName(this TypeRef typeRef)
    {
        var tr = typeRef.GetType_();
        if (tr.IsList())
        {
            return $"{tr.OfType.GetTypeName()}[]";
        }
        return Formatter.FormatType(tr.Name);
    }
}
