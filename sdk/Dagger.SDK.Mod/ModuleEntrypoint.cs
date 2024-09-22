using System.Text.Json;

namespace Dagger.SDK.Mod;

public class ModuleEntrypoint
{
    public static async Task<Void> Invoke<T>(Query dag)
        where T : class, IDagSetter, IEntrypoint, new()
    {
        var fnCall = dag.CurrentFunctionCall();
        var parentName = await fnCall.ParentName();
        // TODO: Get module name to check root type name match with it.

        var result = parentName switch
        {
            // TODO: Dagger.SDK should automatic serialize into id.
            "" => await Register<T>(dag).Id(),
            _ => await Invoke<T>(dag, fnCall),
        };

        return await fnCall.ReturnValue(ToJson(result));
    }

    private static Module Register<T>(Query dag)
        where T : class, IEntrypoint, new()
    {
        T root = new();
        return root.Register(dag, dag.Module());
    }

    private static async Task<object> Invoke<T>(Query dag, FunctionCall fnCall)
        where T : class, IDagSetter, IEntrypoint, new()
    {
        var fnName = await fnCall.Name();
        var fnArgs = await fnCall.InputArgs();

        var inputArgs = new Dictionary<string, JsonElement>();
        foreach (FunctionCallArgValue arg in fnArgs)
        {
            var name = await arg.Name();
            var value = await arg.Value();
            inputArgs[name] = JsonSerializer.Deserialize<JsonElement>(value.Value);
        }

        T root = new();
        root.SetDag(dag);
        return root.Invoke(fnName, inputArgs);
    }

    private static Json ToJson(object result)
    {
        return new Json { Value = JsonSerializer.Serialize(result) };
    }
}
