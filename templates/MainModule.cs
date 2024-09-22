using Mod = Dagger.SDK.Mod;

namespace {{ .Module }};

[Mod.Entrypoint]
[Mod.Object]
public partial class {{ .Module }} 
{
    [Mod.Function]
    public Task<string> Echo(string stringArg) {
        return _dag.Container().From("alpine").WithExec(["echo", $"hello ${stringArg}"]).Stdout();
    }
}
