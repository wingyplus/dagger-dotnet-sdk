using System.Diagnostics;
using System.Text.Json;
using Dagger.SDK;
using Client = Dagger.SDK.Query;

internal class Function
{
    public required FunctionCall FnCall;
    public required string ParentName;
    public required JSON ParentJson;
    public required string Name;
    // public required FunctionCallArgValue[] Args;

    public async Task<string> Invoke(Client dag)
    {
        var potato = JsonSerializer.Deserialize<Potato.Potato>(ParentJson.Value);
        Debug.Assert(potato != null, nameof(potato) + " != null");
        return await potato.Echo("C#");
    }

    public async Task Return<TResult>(TResult result)
    {
        var json = new JSON
        {
            Value = JsonSerializer.Serialize(result)
        };
        await FnCall.ReturnValue(json);
    }

    public async Task<ModuleID> InitializeModule(Client dag)
    {
        return await dag.Module()
            // Class comment code.
            .WithDescription("Eat me a potato")
            .WithObject(
                await dag.TypeDef()
                    // Class name.
                    .WithObject("Potato")
                    .WithFunction(
                        await dag.Function(
                                // Function name, convert to Pascal case. C# might not have any problems with this. lol
                                "Echo",
                                await dag.TypeDef().WithKind(TypeDefKind.STRING_KIND).Id()
                            )
                            // Method comment.
                            .WithDescription("Echo me")
                            .WithArg(
                                // arg name.
                                "name",
                                await dag.TypeDef().WithKind(TypeDefKind.STRING_KIND).Id()
                            )
                            .Id()
                    )
                    .Id()
            )
            .Id();
    }

    public bool IsInitializeModule()
    {
        return string.IsNullOrEmpty(ParentName);
    }
}


public class Program
{
    public static async Task Main(string[] args)
    {
        var dag = Dagger.SDK.Dagger.Connect();
        var fn = await LoadFunctionCall(dag);

        if (fn.IsInitializeModule())
        {
            await fn.Return(await fn.InitializeModule(dag));
            return;
        }

        var result = await fn.Invoke(dag);
        Console.WriteLine(result);
        await fn.Return(result);
    }

    private static async Task<Function> LoadFunctionCall(Client dag)
    {
        var fnCall = dag.CurrentFunctionCall();
        var parentName = await fnCall.ParentName();
        var parentJson = await fnCall.Parent();
        var name = await fnCall.Name();
        // var fnArgs = await fnCall.InputArgs();

        return new Function
        {
            FnCall = fnCall,
            ParentName = parentName,
            ParentJson = parentJson,
            Name = name,
            // Args = fnArgs,
        };
    }
}