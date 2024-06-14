// A Dagger Module
// 
// TODO: will move to the SDK after design finish.
namespace Potato.DaggerSDK.Module;

/// <summary>
/// Expose the class as a Dagger.ObjectTypeDef.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class Object : Attribute;
