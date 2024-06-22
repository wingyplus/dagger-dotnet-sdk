using Dagger.SDK;
using Mod = Dagger.SDK.Mod;

namespace Potato;

public class BaseObject(Query dag)
{
    protected Query Dag { get; } = dag;
}

[Mod.Object]
public class Potato(Query dag) : BaseObject(dag)
{
    [Mod.Function]
    public async Task<string> Echo(string name)
    {
        return await Dag.Container()
            .From("alpine")
            .WithExec(["echo", $"Hello, {name}"])
            .Stdout();
    }

    // [Mod.Function]
    // public Container EchoContainer(string text)
    // {
    //     return Dag.Container()
    //         .From("alpine")
    //         .WithExec(["echo", $"Hello, {text}"]);
    // }
}