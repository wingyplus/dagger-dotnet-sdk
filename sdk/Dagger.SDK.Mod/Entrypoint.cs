using System.Reflection;
using System.Text.Json;

namespace Dagger.SDK.Mod;

public class Entrypoint
{
    public static async Task<Void> Invoke(Query dag, Type rootType)
    {
        FunctionCall fnCall = dag.CurrentFunctionCall();
        string parentName = await fnCall.ParentName();
        // TODO: Get module name to check root type name match with it.

        object result = parentName switch
        {
            // TODO: Dagger.SDK should automatic serialize into id.
            "" => await Register(dag, dag.Module(), rootType).Id(),
            _ => ""
        };

        return await fnCall.ReturnValue(IntoJson(result));
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
