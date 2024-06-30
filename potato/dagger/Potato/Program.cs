// QUESTION: is it possible to get generate by source generator?

using System.Text.Json;
using Dagger.SDK;
using Dagger.SDK.Mod;

public class Program
{
    public static async Task Main(string[] args)
    {
        var dag = Dagger.SDK.Dagger.Connect();
        await Invoke<Potato.Potato>(dag);
    }

    private static async Task Invoke<T>(Query dag) where T : class, IDagSetter, IEntrypoint, new()
    {
        T root = new();
        var fnCall = dag.CurrentFunctionCall();
        var parentName = await fnCall.ParentName();
        // TODO: Get module name to check root type name match with it.

        var result = parentName switch
        {
            // TODO: Dagger.SDK should automatic serialize into id.
            "" => await root.Register(dag).Id(),
            _ => throw new Exception($"{parentName} is not supported at the moment.")
        };

        await fnCall.ReturnValue(IntoJson(result));
    }

    private static Json IntoJson(object result)
    {
        return new Json { Value = JsonSerializer.Serialize(result) };
    }
}