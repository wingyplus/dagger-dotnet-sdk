namespace Dagger.Codegen;

public abstract class CodeRenderer
{
    abstract public string RenderPre();

    abstract public string RenderObject(Type type);

    abstract public string RenderEnum(Type type);

    abstract public string RenderScalar(Type type);

    abstract public string RenderInputObject(Type type);

    abstract public string Format(string source);
}
