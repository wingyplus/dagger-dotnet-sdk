using Dagger.SDK;

namespace Potato;

internal class Module
{
    protected readonly Query Dag;

    protected Module()
    {
        Dag = Dagger.SDK.Dagger.Connect();
    }
}

class Potato : Module
{
    public async Task<string> Echo(string name)
    {
        return await Dag.Container()
            .From("alpine")
            .WithExec(["echo", $"Hello, {name}"])
            .Stdout();
    }
}

