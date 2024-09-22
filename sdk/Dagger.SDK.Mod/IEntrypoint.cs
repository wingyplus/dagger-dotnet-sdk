using System.Text.Json;

namespace Dagger.SDK.Mod;

/// <summary>
/// An interface for invoking the module class.
/// </summary>
public interface IEntrypoint
{
    /// <summary>
    /// Register an object as the root of module.
    /// </summary>
    /// <param name="dag">Dagger client instance.</param>
    /// <param name="module">The empty Dagger module.</param>
    /// <returns>The Dagger module with registered object.</returns>
    Module Register(Query dag, Module module);

    /// <summary>
    /// Invoke a function on the given root type.
    /// </summary>
    /// <param name="dag">The Dagger client instance.</param>
    object Invoke(string name, Dictionary<string, JsonElement> args);
}
