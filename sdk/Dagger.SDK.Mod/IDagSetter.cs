namespace Dagger.SDK.Mod;

public interface IDagSetter
{
    /// <summary>
    /// Set Dagger client instance.
    /// </summary>
    /// <param name="dag">The Dagger client instance.</param>
    void SetDag(Query dag);
}
