using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using Dagger.SDK;
using Client = Dagger.SDK.Query;

internal class Function
{
    public required FunctionCall FnCall;
    public required string ParentName;
    public required Json ParentJson;
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
        var json = new Json
        {
            Value = JsonSerializer.Serialize(result)
        };
        await FnCall.ReturnValue(json);
    }

    public async Task<ModuleId> InitializeModule(Client dag, Type t)
    {
        return await dag.Module()
            // Class comment code.
            .WithDescription("Eat me a potato")
            .WithObject(TypeToObjectTypeDef(dag, t))
            .Id();
    }

    private TypeDef TypeToObjectTypeDef(Client dag, Type t)
    {
        var functions = t.GetMethods()
            .Where(method => method.GetCustomAttribute<Dagger.SDK.Mod.Function>() is not null)
            .Select( method =>
            {
                var function = dag.Function(
                    // Function name, convert to Pascal case. C# might not have any problems with this. lol
                    method.Name,
                    dag.TypeDef().WithKind(TypeDefKind.STRING_KIND)
                );

                foreach (var parameter in method.GetParameters())
                {
                    function = function.WithArg(parameter.Name!, dag.TypeDef().WithKind(TypeDefKind.STRING_KIND));
                }

                return function;
            });

        var objTypeDef = dag.TypeDef().WithObject(t.Name);

        foreach (var fn in functions)
        {
            objTypeDef = objTypeDef.WithFunction(fn);
        }

        return objTypeDef;
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
            await fn.Return(await fn.InitializeModule(dag, typeof(Potato.Potato)));
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