namespace Dagger.SDK.Mod;

/// <summary>
/// Expose the class as a Dagger.ObjectTypeDef.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class Object : Attribute;

/// <summary>
/// Expose the class as a Dagger.Function.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class Function : Attribute;
