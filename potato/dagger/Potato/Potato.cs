using Dagger.SDK;
using Mod = Dagger.SDK.Mod;

namespace Potato;

[Serializable]
[Mod.Object]
public partial class Potato()
{
    [Mod.Function]
    public async Task<string> Echo(string name)
    {
        return await _dag.Container()
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