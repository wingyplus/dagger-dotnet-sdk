namespace Dagger.SDK.Mod;

/// <summary>
/// Expose the class as a Dagger.ObjectTypeDef.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class Object : Attribute;

/// <summary>
/// Expose the class as a Dagger.Function.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class Function : Attribute;
