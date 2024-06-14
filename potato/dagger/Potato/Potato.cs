using Dagger.SDK;
using Module = Potato.DaggerSDK.Module;

namespace Potato;

internal class BaseObject(Query dag)
{
    protected Query Dag { get; } = dag;
}

[Module.Object]
class Potato(Query dag) : BaseObject(dag)
{
    [Module.Function]
    public async Task<string> Echo(string name)
    {
        return await Dag.Container()
            .From("alpine")
            .WithExec(["echo", $"Hello, {name}"])
            .Stdout();
    }

    [Module.Function]
    public Container EchoContainer(string text)
    {
        return Dag.Container()
            .From("alpine")
            .WithExec(["echo", $"Hello, {text}"]);
    }
}