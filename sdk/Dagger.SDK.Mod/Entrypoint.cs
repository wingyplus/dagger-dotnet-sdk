using System.Reflection;
using System.Text.Json;

namespace Dagger.SDK.Mod;

public class Entrypoint
{
    public static async Task<Void> Invoke<T>(Query dag) where T : class, IDagSetter
    {
        var rootType = typeof(T);
        var fnCall = dag.CurrentFunctionCall();
        var parentName = await fnCall.ParentName();
        // TODO: Get module name to check root type name match with it.

        var result = parentName switch
        {
            // TODO: Dagger.SDK should automatic serialize into id.
            "" => await Register(dag, dag.Module(), rootType).Id(),
            _ => await DoInvoke<T>(dag, fnCall)
        };

        return await fnCall.ReturnValue(IntoJson(result));
    }

    private static async Task<object> DoInvoke<T>(Query dag, FunctionCall fnCall) where T : class, IDagSetter
    {
        var fnName = await fnCall.Name();
        var parentJson = await fnCall.Parent();
        var fnArgs = await fnCall.InputArgs();
        var parent = JsonSerializer.Deserialize<T>(parentJson.Value);
        parent.SetDag(dag);
        // QUESTION: Can this be in source generator?
        var parentType = parent.GetType();
        var method = parentType.GetMethod(fnName);
        var methodParameters = method.GetParameters();

        var inputArgs = new Dictionary<string, JsonElement>();
        foreach (FunctionCallArgValue arg in fnArgs)
        {
            var name = await arg.Name();
            var value = await arg.Value();
            inputArgs[name] = JsonSerializer.Deserialize<JsonElement>(value.Value);
        }

        IEnumerable<object?> parameters = [];
        foreach (var param in methodParameters)
        {
            if (param.ParameterType.Name == "String")
            {
                parameters = parameters.Append(inputArgs[param.Name].Deserialize<string>());
            }
            else
            {
                // BOOM!
                parameters = parameters.Append(inputArgs[param.Name]);
            }
        }

        var result = method.Invoke(parent, parameters.ToArray())!;
        // QUESTION: can source generator solve this issue?
        if (result.GetType().IsGenericType)
        {
            var task = (Task)result;
            await task.ConfigureAwait(false);
            return task.GetType().GetProperty("Result").GetValue(task);
        }

        return result;
    }

    private static Json IntoJson(object result)
    {
        return new Json { Value = JsonSerializer.Serialize(result) };
    }

    private static Module Register(Query dag, Module module, Type t)
    {
        var td = RegisterFunctions(dag, dag.TypeDef().WithObject(t.Name), t.GetMethods());
        return module.WithObject(td);
    }

    private static TypeDef RegisterFunctions(Query dag, TypeDef obj, MethodInfo[] methods)
    {
        var functions = methods
            .Where(method => method.GetCustomAttribute<Function>() is not null)
            .Select(method =>
            {
                var function = dag.Function(
                    // TODO: we need name convertion.
                    method.Name,
                    ReturnTypeDef(dag, method)
                );

                return method.GetParameters().Aggregate(function,
                    (fn, parameter) =>
                        fn.WithArg(parameter.Name!, ReturnTypeDef(dag, parameter)));
            });

        return functions.Aggregate(obj, (obj, fn) => obj.WithFunction(fn));
    }

    private static TypeDef ReturnTypeDef(Query dag, ParameterInfo parameter)
    {
        return dag.TypeDef().WithKind(TypeDefKind.STRING_KIND);
    }

    private static TypeDef ReturnTypeDef(Query dag, MethodInfo method)
    {
        return dag.TypeDef().WithKind(TypeDefKind.STRING_KIND);
    }
}
