using System.Text.Json.Serialization;
using Dagger.SDK;
using Dagger.SDK.Mod;

namespace Potato;

// QUESTION: can be done with source generator?
public partial class Potato : IDagSetter
{
    private Query _dag;

    public void SetDag(Query dag)
    {
        _dag = dag;
    }
}