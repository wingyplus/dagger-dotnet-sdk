using System.Text;

namespace Dagger.Codegen;



class CodeGenerator(CodeRenderer renderer)
{
    private readonly CodeRenderer _renderer = renderer;

    private readonly string[] _primitiveTypes = ["ID", "String", "Int", "Float", "Boolean"];

    public string Generate(Introspection instrospection)
    {
        var builder = new StringBuilder(_renderer.RenderPre());
        builder.Append("\n\n");
        var sources = instrospection
            .Schema
            .Types
            .ExceptBy(_primitiveTypes, type => type.Name)
            .Where(NotInternalTypes)
            .Select(Render)
            .Aggregate(builder, (builder, code) => builder.Append(code).Append("\n\n"));
        return _renderer.Format(builder.ToString());
    }

    public bool NotInternalTypes(Type type)
    {
        return !type.Name.StartsWith("__");
    }

    public string Render(Type type)
    {
        switch (type.Kind)
        {
            case "OBJECT": return _renderer.RenderObject(type);
            case "SCALAR": return _renderer.RenderScalar(type);
            case "INPUT_OBJECT": return _renderer.RenderInputObject(type);
            case "ENUM": return _renderer.RenderEnum(type);
            default: throw new Exception($"Type kind {type.Kind} is not supported");
        }
    }
}
