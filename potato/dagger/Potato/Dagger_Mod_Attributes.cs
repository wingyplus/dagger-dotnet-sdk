namespace Dagger.SDK.Mod;

// TODO: move this to source generator.
/// <summary>
/// An interface for invoking the module class.
/// </summary>
public interface IEntrypoint
{
    /// <summary>
    /// Register an object as the root of module.
    /// </summary>
    /// <returns></returns>
    Module Register(Query dag);
}