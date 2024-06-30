using Dagger.SDK;
using Dagger.SDK.Mod;

namespace Potato;

// TODO: move this to source generator.
public partial class Potato : IEntrypoint
{
    public Module Register(Query dag)
    {
        return dag.Module().WithObject(ToObjectTypeDef(dag));
    }
}