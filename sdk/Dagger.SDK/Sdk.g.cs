using Dagger.SDK;

public class Scalar
{
    public readonly string Value;
    public override string ToString() => Value;
}

public class Object
{
}

public class InputObject
{
}

/// <summary>
/// Key value object that represents a build argument.
/// </summary>
public class BuildArg : InputObject
{
    /// <summary>
    /// The build argument name.
    /// </summary>
    public string Name;
    /// <summary>
    /// The build argument value.
    /// </summary>
    public string Value;
}

/// <summary>
/// Sharing mode of the cache volume.
/// </summary>
public enum CacheSharingMode
{
    SHARED,
    PRIVATE,
    LOCKED
}

/// <summary>
/// A directory whose contents persist across runs.
/// </summary>
public class CacheVolume : Object
{
    /// <summary>
    /// A unique identifier for this CacheVolume.
    /// </summary>
    public CacheVolumeID Id()
    {
    }
}

/// <summary>
/// The `CacheVolumeID` scalar type represents an identifier for an object of type CacheVolume.
/// </summary>
public class CacheVolumeID : Scalar
{
}

/// <summary>
/// An OCI-compatible container, also known as a Docker container.
/// </summary>
public class Container : Object
{
    /// <summary>
    /// Turn the container into a Service.
    /// 
    /// Be sure to set any exposed ports before this conversion.
    /// </summary>
    public Service AsService()
    {
    }

    /// <summary>
    /// Returns a File representing the container serialized to a tarball.
    /// </summary>
    public File AsTarball(ImageLayerCompression forcedCompression = null, ImageMediaTypes mediaTypes = OCIMediaTypes, platformVariants = [])
    {
    }

    /// <summary>
    /// Initializes this container from a Dockerfile build.
    /// </summary>
    public Container Build(DirectoryID context, buildArgs = [], string dockerfile = "Dockerfile", secrets = [], string target = "")
    {
    }

    /// <summary>
    /// Retrieves default arguments for future commands.
    /// </summary>
    public DefaultArgs()
    {
    }

    /// <summary>
    /// Retrieves a directory at the given path.
    /// 
    /// Mounts are included.
    /// </summary>
    public Directory Directory(string path)
    {
    }

    /// <summary>
    /// Retrieves entrypoint to be prepended to the arguments of all commands.
    /// </summary>
    public Entrypoint()
    {
    }

    /// <summary>
    /// Retrieves the value of the specified environment variable.
    /// </summary>
    public string EnvVariable(string name)
    {
    }

    /// <summary>
    /// Retrieves the list of environment variables passed to commands.
    /// </summary>
    public EnvVariables()
    {
    }

    /// <summary>
    /// EXPERIMENTAL API! Subject to change/removal at any time.
    /// 
    /// Configures all available GPUs on the host to be accessible to this container.
    /// 
    /// This currently works for Nvidia devices only.
    /// </summary>
    public Container ExperimentalWithAllGPUs()
    {
    }

    /// <summary>
    /// EXPERIMENTAL API! Subject to change/removal at any time.
    /// 
    /// Configures the provided list of devices to be accessible to this container.
    /// 
    /// This currently works for Nvidia devices only.
    /// </summary>
    public Container ExperimentalWithGPU(devices)
    {
    }

    /// <summary>
    /// Writes the container as an OCI tarball to the destination file path on the host.
    /// 
    /// Return true on success.
    /// 
    /// It can also export platform variants.
    /// </summary>
    public bool Export(string path, ImageLayerCompression forcedCompression = null, ImageMediaTypes mediaTypes = OCIMediaTypes, platformVariants = [])
    {
    }

    /// <summary>
    /// Retrieves the list of exposed ports.
    /// 
    /// This includes ports already exposed by the image, even if not explicitly added with dagger.
    /// </summary>
    public ExposedPorts()
    {
    }

    /// <summary>
    /// Retrieves a file at the given path.
    /// 
    /// Mounts are included.
    /// </summary>
    public File File(string path)
    {
    }

    /// <summary>
    /// Initializes this container from a pulled base image.
    /// </summary>
    public Container From(string address)
    {
    }

    /// <summary>
    /// A unique identifier for this Container.
    /// </summary>
    public ContainerID Id()
    {
    }

    /// <summary>
    /// The unique image reference which can only be retrieved immediately after the 'Container.From' call.
    /// </summary>
    public string ImageRef()
    {
    }

    /// <summary>
    /// Reads the container from an OCI tarball.
    /// </summary>
    public Container Import(FileID source, string tag = "")
    {
    }

    /// <summary>
    /// Retrieves the value of the specified label.
    /// </summary>
    public string Label(string name)
    {
    }

    /// <summary>
    /// Retrieves the list of labels passed to container.
    /// </summary>
    public Labels()
    {
    }

    /// <summary>
    /// Retrieves the list of paths where a directory is mounted.
    /// </summary>
    public Mounts()
    {
    }

    /// <summary>
    /// Creates a named sub-pipeline.
    /// </summary>
    public Container Pipeline(string name, string description = "", labels = [])
    {
    }

    /// <summary>
    /// The platform this container executes and publishes as.
    /// </summary>
    public Platform Platform()
    {
    }

    /// <summary>
    /// Publishes this container as a new image to the specified address.
    /// 
    /// Publish returns a fully qualified ref.
    /// 
    /// It can also publish platform variants.
    /// </summary>
    public string Publish(string address, ImageLayerCompression forcedCompression = null, ImageMediaTypes mediaTypes = OCIMediaTypes, platformVariants = [])
    {
    }

    /// <summary>
    /// Retrieves this container's root filesystem. Mounts are not included.
    /// </summary>
    public Directory Rootfs()
    {
    }

    /// <summary>
    /// The error stream of the last executed command.
    /// 
    /// Will execute default command if none is set, or error if there's no default.
    /// </summary>
    public string Stderr()
    {
    }

    /// <summary>
    /// The output stream of the last executed command.
    /// 
    /// Will execute default command if none is set, or error if there's no default.
    /// </summary>
    public string Stdout()
    {
    }

    /// <summary>
    /// Forces evaluation of the pipeline in the engine.
    /// 
    /// It doesn't run the default command if no exec has been set.
    /// </summary>
    public ContainerID Sync()
    {
    }

    /// <summary>
    /// Return an interactive terminal for this container using its configured default terminal command if not overridden by args (or sh as a fallback default).
    /// </summary>
    public Terminal Terminal(cmd = [], bool experimentalPrivilegedNesting = false, bool insecureRootCapabilities = false)
    {
    }

    /// <summary>
    /// Retrieves the user to be set for all commands.
    /// </summary>
    public string User()
    {
    }

    /// <summary>
    /// Configures default arguments for future commands.
    /// </summary>
    public Container WithDefaultArgs(args)
    {
    }

    /// <summary>
    /// Set the default command to invoke for the container's terminal API.
    /// </summary>
    public Container WithDefaultTerminalCmd(args, bool experimentalPrivilegedNesting = false, bool insecureRootCapabilities = false)
    {
    }

    /// <summary>
    /// Retrieves this container plus a directory written at the given path.
    /// </summary>
    public Container WithDirectory(DirectoryID directory, string path, exclude = [], include = [], string owner = "")
    {
    }

    /// <summary>
    /// Retrieves this container but with a different command entrypoint.
    /// </summary>
    public Container WithEntrypoint(args, bool keepDefaultArgs = false)
    {
    }

    /// <summary>
    /// Retrieves this container plus the given environment variable.
    /// </summary>
    public Container WithEnvVariable(string name, string value, bool expand = false)
    {
    }

    /// <summary>
    /// Retrieves this container after executing the specified command inside it.
    /// </summary>
    public Container WithExec(args, bool experimentalPrivilegedNesting = false, bool insecureRootCapabilities = false, string redirectStderr = "", string redirectStdout = "", bool skipEntrypoint = false, string stdin = "")
    {
    }

    /// <summary>
    /// Expose a network port.
    /// 
    /// Exposed ports serve two purposes:
    /// 
    /// - For health checks and introspection, when running services
    /// 
    /// - For setting the EXPOSE OCI field when publishing the container
    /// </summary>
    public Container WithExposedPort(int port, string description = null, bool experimentalSkipHealthcheck = false, NetworkProtocol protocol = TCP)
    {
    }

    /// <summary>
    /// Retrieves this container plus the contents of the given file copied to the given path.
    /// </summary>
    public Container WithFile(string path, FileID source, string owner = "", int permissions = null)
    {
    }

    /// <summary>
    /// Retrieves this container plus the contents of the given files copied to the given path.
    /// </summary>
    public Container WithFiles(string path, sources, string owner = "", int permissions = null)
    {
    }

    /// <summary>
    /// Indicate that subsequent operations should be featured more prominently in the UI.
    /// </summary>
    public Container WithFocus()
    {
    }

    /// <summary>
    /// Retrieves this container plus the given label.
    /// </summary>
    public Container WithLabel(string name, string value)
    {
    }

    /// <summary>
    /// Retrieves this container plus a cache volume mounted at the given path.
    /// </summary>
    public Container WithMountedCache(CacheVolumeID cache, string path, string owner = "", CacheSharingMode sharing = SHARED, DirectoryID source = null)
    {
    }

    /// <summary>
    /// Retrieves this container plus a directory mounted at the given path.
    /// </summary>
    public Container WithMountedDirectory(string path, DirectoryID source, string owner = "")
    {
    }

    /// <summary>
    /// Retrieves this container plus a file mounted at the given path.
    /// </summary>
    public Container WithMountedFile(string path, FileID source, string owner = "")
    {
    }

    /// <summary>
    /// Retrieves this container plus a secret mounted into a file at the given path.
    /// </summary>
    public Container WithMountedSecret(string path, SecretID source, int mode = 256, string owner = "")
    {
    }

    /// <summary>
    /// Retrieves this container plus a temporary directory mounted at the given path.
    /// </summary>
    public Container WithMountedTemp(string path)
    {
    }

    /// <summary>
    /// Retrieves this container plus a new file written at the given path.
    /// </summary>
    public Container WithNewFile(string path, string contents = "", string owner = "", int permissions = 420)
    {
    }

    /// <summary>
    /// Retrieves this container with a registry authentication for a given address.
    /// </summary>
    public Container WithRegistryAuth(string address, SecretID secret, string username)
    {
    }

    /// <summary>
    /// Retrieves the container with the given directory mounted to /.
    /// </summary>
    public Container WithRootfs(DirectoryID directory)
    {
    }

    /// <summary>
    /// Retrieves this container plus an env variable containing the given secret.
    /// </summary>
    public Container WithSecretVariable(string name, SecretID secret)
    {
    }

    /// <summary>
    /// Establish a runtime dependency on a service.
    /// 
    /// The service will be started automatically when needed and detached when it is no longer needed, executing the default command if none is set.
    /// 
    /// The service will be reachable from the container via the provided hostname alias.
    /// 
    /// The service dependency will also convey to any files or directories produced by the container.
    /// </summary>
    public Container WithServiceBinding(string alias, ServiceID service)
    {
    }

    /// <summary>
    /// Retrieves this container plus a socket forwarded to the given Unix socket path.
    /// </summary>
    public Container WithUnixSocket(string path, SocketID source, string owner = "")
    {
    }

    /// <summary>
    /// Retrieves this container with a different command user.
    /// </summary>
    public Container WithUser(string name)
    {
    }

    /// <summary>
    /// Retrieves this container with a different working directory.
    /// </summary>
    public Container WithWorkdir(string path)
    {
    }

    /// <summary>
    /// Retrieves this container with unset default arguments for future commands.
    /// </summary>
    public Container WithoutDefaultArgs()
    {
    }

    /// <summary>
    /// Retrieves this container with an unset command entrypoint.
    /// </summary>
    public Container WithoutEntrypoint(bool keepDefaultArgs = false)
    {
    }

    /// <summary>
    /// Retrieves this container minus the given environment variable.
    /// </summary>
    public Container WithoutEnvVariable(string name)
    {
    }

    /// <summary>
    /// Unexpose a previously exposed port.
    /// </summary>
    public Container WithoutExposedPort(int port, NetworkProtocol protocol = TCP)
    {
    }

    /// <summary>
    /// Indicate that subsequent operations should not be featured more prominently in the UI.
    /// 
    /// This is the initial state of all containers.
    /// </summary>
    public Container WithoutFocus()
    {
    }

    /// <summary>
    /// Retrieves this container minus the given environment label.
    /// </summary>
    public Container WithoutLabel(string name)
    {
    }

    /// <summary>
    /// Retrieves this container after unmounting everything at the given path.
    /// </summary>
    public Container WithoutMount(string path)
    {
    }

    /// <summary>
    /// Retrieves this container without the registry authentication of a given address.
    /// </summary>
    public Container WithoutRegistryAuth(string address)
    {
    }

    /// <summary>
    /// Retrieves this container with a previously added Unix socket removed.
    /// </summary>
    public Container WithoutUnixSocket(string path)
    {
    }

    /// <summary>
    /// Retrieves this container with an unset command user.
    /// 
    /// Should default to root.
    /// </summary>
    public Container WithoutUser()
    {
    }

    /// <summary>
    /// Retrieves this container with an unset working directory.
    /// 
    /// Should default to "/".
    /// </summary>
    public Container WithoutWorkdir()
    {
    }

    /// <summary>
    /// Retrieves the working directory for all commands.
    /// </summary>
    public string Workdir()
    {
    }
}

/// <summary>
/// The `ContainerID` scalar type represents an identifier for an object of type Container.
/// </summary>
public class ContainerID : Scalar
{
}

/// <summary>
/// Reflective module API provided to functions at runtime.
/// </summary>
public class CurrentModule : Object
{
    /// <summary>
    /// A unique identifier for this CurrentModule.
    /// </summary>
    public CurrentModuleID Id()
    {
    }

    /// <summary>
    /// The name of the module being executed in
    /// </summary>
    public string Name()
    {
    }

    /// <summary>
    /// The directory containing the module's source code loaded into the engine (plus any generated code that may have been created).
    /// </summary>
    public Directory Source()
    {
    }

    /// <summary>
    /// Load a directory from the module's scratch working directory, including any changes that may have been made to it during module function execution.
    /// </summary>
    public Directory Workdir(string path, exclude = [], include = [])
    {
    }

    /// <summary>
    /// Load a file from the module's scratch working directory, including any changes that may have been made to it during module function execution.Load a file from the module's scratch working directory, including any changes that may have been made to it during module function execution.
    /// </summary>
    public File WorkdirFile(string path)
    {
    }
}

/// <summary>
/// The `CurrentModuleID` scalar type represents an identifier for an object of type CurrentModule.
/// </summary>
public class CurrentModuleID : Scalar
{
}

/// <summary>
/// A directory.
/// </summary>
public class Directory : Object
{
    /// <summary>
    /// Load the directory as a Dagger module
    /// </summary>
    public Module AsModule(string sourceRootPath = ".")
    {
    }

    /// <summary>
    /// Gets the difference between this directory and an another directory.
    /// </summary>
    public Directory Diff(DirectoryID other)
    {
    }

    /// <summary>
    /// Retrieves a directory at the given path.
    /// </summary>
    public Directory Directory(string path)
    {
    }

    /// <summary>
    /// Builds a new Docker container from this directory.
    /// </summary>
    public Container DockerBuild(buildArgs = [], string dockerfile = "Dockerfile", Platform platform = null, secrets = [], string target = "")
    {
    }

    /// <summary>
    /// Returns a list of files and directories at the given path.
    /// </summary>
    public Entries(string path = null)
    {
    }

    /// <summary>
    /// Writes the contents of the directory to a path on the host.
    /// </summary>
    public bool Export(string path, bool wipe = false)
    {
    }

    /// <summary>
    /// Retrieves a file at the given path.
    /// </summary>
    public File File(string path)
    {
    }

    /// <summary>
    /// Returns a list of files and directories that matche the given pattern.
    /// </summary>
    public Glob(string pattern)
    {
    }

    /// <summary>
    /// A unique identifier for this Directory.
    /// </summary>
    public DirectoryID Id()
    {
    }

    /// <summary>
    /// Creates a named sub-pipeline.
    /// </summary>
    public Directory Pipeline(string name, string description = "", labels = [])
    {
    }

    /// <summary>
    /// Force evaluation in the engine.
    /// </summary>
    public DirectoryID Sync()
    {
    }

    /// <summary>
    /// Retrieves this directory plus a directory written at the given path.
    /// </summary>
    public Directory WithDirectory(DirectoryID directory, string path, exclude = [], include = [])
    {
    }

    /// <summary>
    /// Retrieves this directory plus the contents of the given file copied to the given path.
    /// </summary>
    public Directory WithFile(string path, FileID source, int permissions = null)
    {
    }

    /// <summary>
    /// Retrieves this directory plus the contents of the given files copied to the given path.
    /// </summary>
    public Directory WithFiles(string path, sources, int permissions = null)
    {
    }

    /// <summary>
    /// Retrieves this directory plus a new directory created at the given path.
    /// </summary>
    public Directory WithNewDirectory(string path, int permissions = 420)
    {
    }

    /// <summary>
    /// Retrieves this directory plus a new file written at the given path.
    /// </summary>
    public Directory WithNewFile(string contents, string path, int permissions = 420)
    {
    }

    /// <summary>
    /// Retrieves this directory with all file/dir timestamps set to the given time.
    /// </summary>
    public Directory WithTimestamps(int timestamp)
    {
    }

    /// <summary>
    /// Retrieves this directory with the directory at the given path removed.
    /// </summary>
    public Directory WithoutDirectory(string path)
    {
    }

    /// <summary>
    /// Retrieves this directory with the file at the given path removed.
    /// </summary>
    public Directory WithoutFile(string path)
    {
    }
}

/// <summary>
/// The `DirectoryID` scalar type represents an identifier for an object of type Directory.
/// </summary>
public class DirectoryID : Scalar
{
}

/// <summary>
/// An environment variable name and value.
/// </summary>
public class EnvVariable : Object
{
    /// <summary>
    /// A unique identifier for this EnvVariable.
    /// </summary>
    public EnvVariableID Id()
    {
    }

    /// <summary>
    /// The environment variable name.
    /// </summary>
    public string Name()
    {
    }

    /// <summary>
    /// The environment variable value.
    /// </summary>
    public string Value()
    {
    }
}

/// <summary>
/// The `EnvVariableID` scalar type represents an identifier for an object of type EnvVariable.
/// </summary>
public class EnvVariableID : Scalar
{
}

/// <summary>
/// A definition of a field on a custom object defined in a Module.
/// 
/// A field on an object has a static value, as opposed to a function on an object whose value is computed by invoking code (and can accept arguments).
/// </summary>
public class FieldTypeDef : Object
{
    /// <summary>
    /// A doc string for the field, if any.
    /// </summary>
    public string Description()
    {
    }

    /// <summary>
    /// A unique identifier for this FieldTypeDef.
    /// </summary>
    public FieldTypeDefID Id()
    {
    }

    /// <summary>
    /// The name of the field in lowerCamelCase format.
    /// </summary>
    public string Name()
    {
    }

    /// <summary>
    /// The type of the field.
    /// </summary>
    public TypeDef TypeDef()
    {
    }
}

/// <summary>
/// The `FieldTypeDefID` scalar type represents an identifier for an object of type FieldTypeDef.
/// </summary>
public class FieldTypeDefID : Scalar
{
}

/// <summary>
/// A file.
/// </summary>
public class File : Object
{
    /// <summary>
    /// Retrieves the contents of the file.
    /// </summary>
    public string Contents()
    {
    }

    /// <summary>
    /// Writes the file to a file path on the host.
    /// </summary>
    public bool Export(string path, bool allowParentDirPath = false)
    {
    }

    /// <summary>
    /// A unique identifier for this File.
    /// </summary>
    public FileID Id()
    {
    }

    /// <summary>
    /// Retrieves the name of the file.
    /// </summary>
    public string Name()
    {
    }

    /// <summary>
    /// Retrieves the size of the file, in bytes.
    /// </summary>
    public int Size()
    {
    }

    /// <summary>
    /// Force evaluation in the engine.
    /// </summary>
    public FileID Sync()
    {
    }

    /// <summary>
    /// Retrieves this file with its created/modified timestamps set to the given time.
    /// </summary>
    public File WithTimestamps(int timestamp)
    {
    }
}

/// <summary>
/// The `FileID` scalar type represents an identifier for an object of type File.
/// </summary>
public class FileID : Scalar
{
}

/// <summary>
/// Function represents a resolver provided by a Module.
/// 
/// A function always evaluates against a parent object and is given a set of named arguments.
/// </summary>
public class Function : Object
{
    /// <summary>
    /// Arguments accepted by the function, if any.
    /// </summary>
    public Args()
    {
    }

    /// <summary>
    /// A doc string for the function, if any.
    /// </summary>
    public string Description()
    {
    }

    /// <summary>
    /// A unique identifier for this Function.
    /// </summary>
    public FunctionID Id()
    {
    }

    /// <summary>
    /// The name of the function.
    /// </summary>
    public string Name()
    {
    }

    /// <summary>
    /// The type returned by the function.
    /// </summary>
    public TypeDef ReturnType()
    {
    }

    /// <summary>
    /// Returns the function with the provided argument
    /// </summary>
    public Function WithArg(string name, TypeDefID typeDef, JSON defaultValue = null, string description = "")
    {
    }

    /// <summary>
    /// Returns the function with the given doc string.
    /// </summary>
    public Function WithDescription(string description)
    {
    }
}

/// <summary>
/// An argument accepted by a function.
/// 
/// This is a specification for an argument at function definition time, not an argument passed at function call time.
/// </summary>
public class FunctionArg : Object
{
    /// <summary>
    /// A default value to use for this argument when not explicitly set by the caller, if any.
    /// </summary>
    public JSON DefaultValue()
    {
    }

    /// <summary>
    /// A doc string for the argument, if any.
    /// </summary>
    public string Description()
    {
    }

    /// <summary>
    /// A unique identifier for this FunctionArg.
    /// </summary>
    public FunctionArgID Id()
    {
    }

    /// <summary>
    /// The name of the argument in lowerCamelCase format.
    /// </summary>
    public string Name()
    {
    }

    /// <summary>
    /// The type of the argument.
    /// </summary>
    public TypeDef TypeDef()
    {
    }
}

/// <summary>
/// The `FunctionArgID` scalar type represents an identifier for an object of type FunctionArg.
/// </summary>
public class FunctionArgID : Scalar
{
}

/// <summary>
/// An active function call.
/// </summary>
public class FunctionCall : Object
{
    /// <summary>
    /// A unique identifier for this FunctionCall.
    /// </summary>
    public FunctionCallID Id()
    {
    }

    /// <summary>
    /// The argument values the function is being invoked with.
    /// </summary>
    public InputArgs()
    {
    }

    /// <summary>
    /// The name of the function being called.
    /// </summary>
    public string Name()
    {
    }

    /// <summary>
    /// The value of the parent object of the function being called. If the function is top-level to the module, this is always an empty object.
    /// </summary>
    public JSON Parent()
    {
    }

    /// <summary>
    /// The name of the parent object of the function being called. If the function is top-level to the module, this is the name of the module.
    /// </summary>
    public string ParentName()
    {
    }

    /// <summary>
    /// Set the return value of the function call to the provided value.
    /// </summary>
    public Void ReturnValue(JSON value)
    {
    }
}

/// <summary>
/// A value passed as a named argument to a function call.
/// </summary>
public class FunctionCallArgValue : Object
{
    /// <summary>
    /// A unique identifier for this FunctionCallArgValue.
    /// </summary>
    public FunctionCallArgValueID Id()
    {
    }

    /// <summary>
    /// The name of the argument.
    /// </summary>
    public string Name()
    {
    }

    /// <summary>
    /// The value of the argument represented as a JSON serialized string.
    /// </summary>
    public JSON Value()
    {
    }
}

/// <summary>
/// The `FunctionCallArgValueID` scalar type represents an identifier for an object of type FunctionCallArgValue.
/// </summary>
public class FunctionCallArgValueID : Scalar
{
}

/// <summary>
/// The `FunctionCallID` scalar type represents an identifier for an object of type FunctionCall.
/// </summary>
public class FunctionCallID : Scalar
{
}

/// <summary>
/// The `FunctionID` scalar type represents an identifier for an object of type Function.
/// </summary>
public class FunctionID : Scalar
{
}

/// <summary>
/// The result of running an SDK's codegen.
/// </summary>
public class GeneratedCode : Object
{
    /// <summary>
    /// The directory containing the generated code.
    /// </summary>
    public Directory Code()
    {
    }

    /// <summary>
    /// A unique identifier for this GeneratedCode.
    /// </summary>
    public GeneratedCodeID Id()
    {
    }

    /// <summary>
    /// List of paths to mark generated in version control (i.e. .gitattributes).
    /// </summary>
    public VcsGeneratedPaths()
    {
    }

    /// <summary>
    /// List of paths to ignore in version control (i.e. .gitignore).
    /// </summary>
    public VcsIgnoredPaths()
    {
    }

    /// <summary>
    /// Set the list of paths to mark generated in version control.
    /// </summary>
    public GeneratedCode WithVCSGeneratedPaths(paths)
    {
    }

    /// <summary>
    /// Set the list of paths to ignore in version control.
    /// </summary>
    public GeneratedCode WithVCSIgnoredPaths(paths)
    {
    }
}

/// <summary>
/// The `GeneratedCodeID` scalar type represents an identifier for an object of type GeneratedCode.
/// </summary>
public class GeneratedCodeID : Scalar
{
}

/// <summary>
/// Module source originating from a git repo.
/// </summary>
public class GitModuleSource : Object
{
    /// <summary>
    /// The URL from which the source's git repo can be cloned.
    /// </summary>
    public string CloneURL()
    {
    }

    /// <summary>
    /// The resolved commit of the git repo this source points to.
    /// </summary>
    public string Commit()
    {
    }

    /// <summary>
    /// The directory containing everything needed to load load and use the module.
    /// </summary>
    public Directory ContextDirectory()
    {
    }

    /// <summary>
    /// The URL to the source's git repo in a web browser
    /// </summary>
    public string HtmlURL()
    {
    }

    /// <summary>
    /// A unique identifier for this GitModuleSource.
    /// </summary>
    public GitModuleSourceID Id()
    {
    }

    /// <summary>
    /// The path to the root of the module source under the context directory. This directory contains its configuration file. It also contains its source code (possibly as a subdirectory).
    /// </summary>
    public string RootSubpath()
    {
    }

    /// <summary>
    /// The specified version of the git repo this source points to.
    /// </summary>
    public string Version()
    {
    }
}

/// <summary>
/// The `GitModuleSourceID` scalar type represents an identifier for an object of type GitModuleSource.
/// </summary>
public class GitModuleSourceID : Scalar
{
}

/// <summary>
/// A git ref (tag, branch, or commit).
/// </summary>
public class GitRef : Object
{
    /// <summary>
    /// The resolved commit id at this ref.
    /// </summary>
    public string Commit()
    {
    }

    /// <summary>
    /// A unique identifier for this GitRef.
    /// </summary>
    public GitRefID Id()
    {
    }

    /// <summary>
    /// The filesystem tree at this ref.
    /// </summary>
    public Directory Tree(SocketID sshAuthSocket = null, string sshKnownHosts = null)
    {
    }
}

/// <summary>
/// The `GitRefID` scalar type represents an identifier for an object of type GitRef.
/// </summary>
public class GitRefID : Scalar
{
}

/// <summary>
/// A git repository.
/// </summary>
public class GitRepository : Object
{
    /// <summary>
    /// Returns details of a branch.
    /// </summary>
    public GitRef Branch(string name)
    {
    }

    /// <summary>
    /// Returns details of a commit.
    /// </summary>
    public GitRef Commit(string id)
    {
    }

    /// <summary>
    /// Returns details for HEAD.
    /// </summary>
    public GitRef Head()
    {
    }

    /// <summary>
    /// A unique identifier for this GitRepository.
    /// </summary>
    public GitRepositoryID Id()
    {
    }

    /// <summary>
    /// Returns details of a ref.
    /// </summary>
    public GitRef Ref(string name)
    {
    }

    /// <summary>
    /// Returns details of a tag.
    /// </summary>
    public GitRef Tag(string name)
    {
    }
}

/// <summary>
/// The `GitRepositoryID` scalar type represents an identifier for an object of type GitRepository.
/// </summary>
public class GitRepositoryID : Scalar
{
}

/// <summary>
/// Information about the host environment.
/// </summary>
public class Host : Object
{
    /// <summary>
    /// Accesses a directory on the host.
    /// </summary>
    public Directory Directory(string path, exclude = [], include = [])
    {
    }

    /// <summary>
    /// Accesses a file on the host.
    /// </summary>
    public File File(string path)
    {
    }

    /// <summary>
    /// A unique identifier for this Host.
    /// </summary>
    public HostID Id()
    {
    }

    /// <summary>
    /// Creates a service that forwards traffic to a specified address via the host.
    /// </summary>
    public Service Service(ports, string host = "localhost")
    {
    }

    /// <summary>
    /// Sets a secret given a user-defined name and the file path on the host, and returns the secret.
    /// 
    /// The file is limited to a size of 512000 bytes.
    /// </summary>
    public Secret SetSecretFile(string name, string path)
    {
    }

    /// <summary>
    /// Creates a tunnel that forwards traffic from the host to a service.
    /// </summary>
    public Service Tunnel(ServiceID service, bool native = false, ports = [])
    {
    }

    /// <summary>
    /// Accesses a Unix socket on the host.
    /// </summary>
    public Socket UnixSocket(string path)
    {
    }
}

/// <summary>
/// The `HostID` scalar type represents an identifier for an object of type Host.
/// </summary>
public class HostID : Scalar
{
}

/// <summary>
/// Compression algorithm to use for image layers.
/// </summary>
public enum ImageLayerCompression
{
    Gzip,
    Zstd,
    EStarGZ,
    Uncompressed
}

/// <summary>
/// Mediatypes to use in published or exported image metadata.
/// </summary>
public enum ImageMediaTypes
{
    OCIMediaTypes,
    DockerMediaTypes
}

/// <summary>
/// A graphql input type, which is essentially just a group of named args.
/// This is currently only used to represent pre-existing usage of graphql input types
/// in the core API. It is not used by user modules and shouldn't ever be as user
/// module accept input objects via their id rather than graphql input types.
/// </summary>
public class InputTypeDef : Object
{
    /// <summary>
    /// Static fields defined on this input object, if any.
    /// </summary>
    public Fields()
    {
    }

    /// <summary>
    /// A unique identifier for this InputTypeDef.
    /// </summary>
    public InputTypeDefID Id()
    {
    }

    /// <summary>
    /// The name of the input object.
    /// </summary>
    public string Name()
    {
    }
}

/// <summary>
/// The `InputTypeDefID` scalar type represents an identifier for an object of type InputTypeDef.
/// </summary>
public class InputTypeDefID : Scalar
{
}

/// <summary>
/// A definition of a custom interface defined in a Module.
/// </summary>
public class InterfaceTypeDef : Object
{
    /// <summary>
    /// The doc string for the interface, if any.
    /// </summary>
    public string Description()
    {
    }

    /// <summary>
    /// Functions defined on this interface, if any.
    /// </summary>
    public Functions()
    {
    }

    /// <summary>
    /// A unique identifier for this InterfaceTypeDef.
    /// </summary>
    public InterfaceTypeDefID Id()
    {
    }

    /// <summary>
    /// The name of the interface.
    /// </summary>
    public string Name()
    {
    }

    /// <summary>
    /// If this InterfaceTypeDef is associated with a Module, the name of the module. Unset otherwise.
    /// </summary>
    public string SourceModuleName()
    {
    }
}

/// <summary>
/// The `InterfaceTypeDefID` scalar type represents an identifier for an object of type InterfaceTypeDef.
/// </summary>
public class InterfaceTypeDefID : Scalar
{
}

/// <summary>
/// An arbitrary JSON-encoded value.
/// </summary>
public class JSON : Scalar
{
}

/// <summary>
/// A simple key value object that represents a label.
/// </summary>
public class Label : Object
{
    /// <summary>
    /// A unique identifier for this Label.
    /// </summary>
    public LabelID Id()
    {
    }

    /// <summary>
    /// The label name.
    /// </summary>
    public string Name()
    {
    }

    /// <summary>
    /// The label value.
    /// </summary>
    public string Value()
    {
    }
}

/// <summary>
/// The `LabelID` scalar type represents an identifier for an object of type Label.
/// </summary>
public class LabelID : Scalar
{
}

/// <summary>
/// A definition of a list type in a Module.
/// </summary>
public class ListTypeDef : Object
{
    /// <summary>
    /// The type of the elements in the list.
    /// </summary>
    public TypeDef ElementTypeDef()
    {
    }

    /// <summary>
    /// A unique identifier for this ListTypeDef.
    /// </summary>
    public ListTypeDefID Id()
    {
    }
}

/// <summary>
/// The `ListTypeDefID` scalar type represents an identifier for an object of type ListTypeDef.
/// </summary>
public class ListTypeDefID : Scalar
{
}

/// <summary>
/// Module source that that originates from a path locally relative to an arbitrary directory.
/// </summary>
public class LocalModuleSource : Object
{
    /// <summary>
    /// The directory containing everything needed to load load and use the module.
    /// </summary>
    public Directory ContextDirectory()
    {
    }

    /// <summary>
    /// A unique identifier for this LocalModuleSource.
    /// </summary>
    public LocalModuleSourceID Id()
    {
    }

    /// <summary>
    /// The path to the root of the module source under the context directory. This directory contains its configuration file. It also contains its source code (possibly as a subdirectory).
    /// </summary>
    public string RootSubpath()
    {
    }
}

/// <summary>
/// The `LocalModuleSourceID` scalar type represents an identifier for an object of type LocalModuleSource.
/// </summary>
public class LocalModuleSourceID : Scalar
{
}

/// <summary>
/// A Dagger module.
/// </summary>
public class Module : Object
{
    /// <summary>
    /// Modules used by this module.
    /// </summary>
    public Dependencies()
    {
    }

    /// <summary>
    /// The dependencies as configured by the module.
    /// </summary>
    public DependencyConfig()
    {
    }

    /// <summary>
    /// The doc string of the module, if any
    /// </summary>
    public string Description()
    {
    }

    /// <summary>
    /// The generated files and directories made on top of the module source's context directory.
    /// </summary>
    public Directory GeneratedContextDiff()
    {
    }

    /// <summary>
    /// The module source's context plus any configuration and source files created by codegen.
    /// </summary>
    public Directory GeneratedContextDirectory()
    {
    }

    /// <summary>
    /// A unique identifier for this Module.
    /// </summary>
    public ModuleID Id()
    {
    }

    /// <summary>
    /// Retrieves the module with the objects loaded via its SDK.
    /// </summary>
    public Module Initialize()
    {
    }

    /// <summary>
    /// Interfaces served by this module.
    /// </summary>
    public Interfaces()
    {
    }

    /// <summary>
    /// The name of the module
    /// </summary>
    public string Name()
    {
    }

    /// <summary>
    /// Objects served by this module.
    /// </summary>
    public Objects()
    {
    }

    /// <summary>
    /// The container that runs the module's entrypoint. It will fail to execute if the module doesn't compile.
    /// </summary>
    public Container Runtime()
    {
    }

    /// <summary>
    /// The SDK used by this module. Either a name of a builtin SDK or a module source ref string pointing to the SDK's implementation.
    /// </summary>
    public string Sdk()
    {
    }

    /// <summary>
    /// Serve a module's API in the current session.
    /// 
    /// Note: this can only be called once per session. In the future, it could return a stream or service to remove the side effect.
    /// </summary>
    public Void Serve()
    {
    }

    /// <summary>
    /// The source for the module.
    /// </summary>
    public ModuleSource Source()
    {
    }

    /// <summary>
    /// Retrieves the module with the given description
    /// </summary>
    public Module WithDescription(string description)
    {
    }

    /// <summary>
    /// This module plus the given Interface type and associated functions
    /// </summary>
    public Module WithInterface(TypeDefID iface)
    {
    }

    /// <summary>
    /// This module plus the given Object type and associated functions.
    /// </summary>
    public Module WithObject(TypeDefID object)
    {
    }

    /// <summary>
    /// Retrieves the module with basic configuration loaded if present.
    /// </summary>
    public Module WithSource(ModuleSourceID source)
    {
    }
}

/// <summary>
/// The configuration of dependency of a module.
/// </summary>
public class ModuleDependency : Object
{
    /// <summary>
    /// A unique identifier for this ModuleDependency.
    /// </summary>
    public ModuleDependencyID Id()
    {
    }

    /// <summary>
    /// The name of the dependency module.
    /// </summary>
    public string Name()
    {
    }

    /// <summary>
    /// The source for the dependency module.
    /// </summary>
    public ModuleSource Source()
    {
    }
}

/// <summary>
/// The `ModuleDependencyID` scalar type represents an identifier for an object of type ModuleDependency.
/// </summary>
public class ModuleDependencyID : Scalar
{
}

/// <summary>
/// The `ModuleID` scalar type represents an identifier for an object of type Module.
/// </summary>
public class ModuleID : Scalar
{
}

/// <summary>
/// The source needed to load and run a module, along with any metadata about the source such as versions/urls/etc.
/// </summary>
public class ModuleSource : Object
{
    /// <summary>
    /// If the source is a of kind git, the git source representation of it.
    /// </summary>
    public GitModuleSource AsGitSource()
    {
    }

    /// <summary>
    /// If the source is of kind local, the local source representation of it.
    /// </summary>
    public LocalModuleSource AsLocalSource()
    {
    }

    /// <summary>
    /// Load the source as a module. If this is a local source, the parent directory must have been provided during module source creation
    /// </summary>
    public Module AsModule()
    {
    }

    /// <summary>
    /// A human readable ref string representation of this module source.
    /// </summary>
    public string AsString()
    {
    }

    /// <summary>
    /// Returns whether the module source has a configuration file.
    /// </summary>
    public bool ConfigExists()
    {
    }

    /// <summary>
    /// The directory containing everything needed to load load and use the module.
    /// </summary>
    public Directory ContextDirectory()
    {
    }

    /// <summary>
    /// The dependencies of the module source. Includes dependencies from the configuration and any extras from withDependencies calls.
    /// </summary>
    public Dependencies()
    {
    }

    /// <summary>
    /// The directory containing the module configuration and source code (source code may be in a subdir).
    /// </summary>
    public Directory Directory(string path)
    {
    }

    /// <summary>
    /// A unique identifier for this ModuleSource.
    /// </summary>
    public ModuleSourceID Id()
    {
    }

    /// <summary>
    /// The kind of source (e.g. local, git, etc.)
    /// </summary>
    public ModuleSourceKind Kind()
    {
    }

    /// <summary>
    /// If set, the name of the module this source references, including any overrides at runtime by callers.
    /// </summary>
    public string ModuleName()
    {
    }

    /// <summary>
    /// The original name of the module this source references, as defined in the module configuration.
    /// </summary>
    public string ModuleOriginalName()
    {
    }

    /// <summary>
    /// The path to the module source's context directory on the caller's filesystem. Only valid for local sources.
    /// </summary>
    public string ResolveContextPathFromCaller()
    {
    }

    /// <summary>
    /// Resolve the provided module source arg as a dependency relative to this module source.
    /// </summary>
    public ModuleSource ResolveDependency(ModuleSourceID dep)
    {
    }

    /// <summary>
    /// Load a directory from the caller optionally with a given view applied.
    /// </summary>
    public Directory ResolveDirectoryFromCaller(string path, string viewName = null)
    {
    }

    /// <summary>
    /// Load the source from its path on the caller's filesystem, including only needed+configured files and directories. Only valid for local sources.
    /// </summary>
    public ModuleSource ResolveFromCaller()
    {
    }

    /// <summary>
    /// The path relative to context of the root of the module source, which contains dagger.json. It also contains the module implementation source code, but that may or may not being a subdir of this root.
    /// </summary>
    public string SourceRootSubpath()
    {
    }

    /// <summary>
    /// The path relative to context of the module implementation source code.
    /// </summary>
    public string SourceSubpath()
    {
    }

    /// <summary>
    /// Retrieve a named view defined for this module source.
    /// </summary>
    public ModuleSourceView View(string name)
    {
    }

    /// <summary>
    /// The named views defined for this module source, which are sets of directory filters that can be applied to directory arguments provided to functions.
    /// </summary>
    public Views()
    {
    }

    /// <summary>
    /// Update the module source with a new context directory. Only valid for local sources.
    /// </summary>
    public ModuleSource WithContextDirectory(DirectoryID dir)
    {
    }

    /// <summary>
    /// Append the provided dependencies to the module source's dependency list.
    /// </summary>
    public ModuleSource WithDependencies(dependencies)
    {
    }

    /// <summary>
    /// Update the module source with a new name.
    /// </summary>
    public ModuleSource WithName(string name)
    {
    }

    /// <summary>
    /// Update the module source with a new SDK.
    /// </summary>
    public ModuleSource WithSDK(string sdk)
    {
    }

    /// <summary>
    /// Update the module source with a new source subpath.
    /// </summary>
    public ModuleSource WithSourceSubpath(string path)
    {
    }

    /// <summary>
    /// Update the module source with a new named view.
    /// </summary>
    public ModuleSource WithView(string name, patterns)
    {
    }
}

/// <summary>
/// The `ModuleSourceID` scalar type represents an identifier for an object of type ModuleSource.
/// </summary>
public class ModuleSourceID : Scalar
{
}

/// <summary>
/// The kind of module source.
/// </summary>
public enum ModuleSourceKind
{
    LOCAL_SOURCE,
    GIT_SOURCE
}

/// <summary>
/// A named set of path filters that can be applied to directory arguments provided to functions.
/// </summary>
public class ModuleSourceView : Object
{
    /// <summary>
    /// A unique identifier for this ModuleSourceView.
    /// </summary>
    public ModuleSourceViewID Id()
    {
    }

    /// <summary>
    /// The name of the view
    /// </summary>
    public string Name()
    {
    }

    /// <summary>
    /// The patterns of the view used to filter paths
    /// </summary>
    public Patterns()
    {
    }
}

/// <summary>
/// The `ModuleSourceViewID` scalar type represents an identifier for an object of type ModuleSourceView.
/// </summary>
public class ModuleSourceViewID : Scalar
{
}

/// <summary>
/// Transport layer network protocol associated to a port.
/// </summary>
public enum NetworkProtocol
{
    TCP,
    UDP
}

/// <summary>
/// A definition of a custom object defined in a Module.
/// </summary>
public class ObjectTypeDef : Object
{
    /// <summary>
    /// The function used to construct new instances of this object, if any
    /// </summary>
    public Function Constructor()
    {
    }

    /// <summary>
    /// The doc string for the object, if any.
    /// </summary>
    public string Description()
    {
    }

    /// <summary>
    /// Static fields defined on this object, if any.
    /// </summary>
    public Fields()
    {
    }

    /// <summary>
    /// Functions defined on this object, if any.
    /// </summary>
    public Functions()
    {
    }

    /// <summary>
    /// A unique identifier for this ObjectTypeDef.
    /// </summary>
    public ObjectTypeDefID Id()
    {
    }

    /// <summary>
    /// The name of the object.
    /// </summary>
    public string Name()
    {
    }

    /// <summary>
    /// If this ObjectTypeDef is associated with a Module, the name of the module. Unset otherwise.
    /// </summary>
    public string SourceModuleName()
    {
    }
}

/// <summary>
/// The `ObjectTypeDefID` scalar type represents an identifier for an object of type ObjectTypeDef.
/// </summary>
public class ObjectTypeDefID : Scalar
{
}

/// <summary>
/// Key value object that represents a pipeline label.
/// </summary>
public class PipelineLabel : InputObject
{
    /// <summary>
    /// Label name.
    /// </summary>
    public string Name;
    /// <summary>
    /// Label value.
    /// </summary>
    public string Value;
}

/// <summary>
/// The platform config OS and architecture in a Container.
/// 
/// The format is [os]/[platform]/[version] (e.g., "darwin/arm64/v7", "windows/amd64", "linux/arm64").
/// </summary>
public class Platform : Scalar
{
}

/// <summary>
/// A port exposed by a container.
/// </summary>
public class Port : Object
{
    /// <summary>
    /// The port description.
    /// </summary>
    public string Description()
    {
    }

    /// <summary>
    /// Skip the health check when run as a service.
    /// </summary>
    public bool ExperimentalSkipHealthcheck()
    {
    }

    /// <summary>
    /// A unique identifier for this Port.
    /// </summary>
    public PortID Id()
    {
    }

    /// <summary>
    /// The port number.
    /// </summary>
    public int Port()
    {
    }

    /// <summary>
    /// The transport layer protocol.
    /// </summary>
    public NetworkProtocol Protocol()
    {
    }
}

/// <summary>
/// Port forwarding rules for tunneling network traffic.
/// </summary>
public class PortForward : InputObject
{
    /// <summary>
    /// Port to expose to clients. If unspecified, a default will be chosen.
    /// </summary>
    public string Frontend;
    /// <summary>
    /// Destination port for traffic.
    /// </summary>
    public string Backend;
    /// <summary>
    /// Transport layer protocol to use for traffic.
    /// </summary>
    public string Protocol;
}

/// <summary>
/// The `PortID` scalar type represents an identifier for an object of type Port.
/// </summary>
public class PortID : Scalar
{
}

/// <summary>
/// The root of the DAG.
/// </summary>
public class Query : Object
{
    /// <summary>
    /// Retrieves a content-addressed blob.
    /// </summary>
    public Directory Blob(string digest, string mediaType, int size, string uncompressed)
    {
    }

    /// <summary>
    /// Retrieves a container builtin to the engine.
    /// </summary>
    public Container BuiltinContainer(string digest)
    {
    }

    /// <summary>
    /// Constructs a cache volume for a given cache key.
    /// </summary>
    public CacheVolume CacheVolume(string key)
    {
    }

    /// <summary>
    /// Checks if the current Dagger Engine is compatible with an SDK's required version.
    /// </summary>
    public bool CheckVersionCompatibility(string version)
    {
    }

    /// <summary>
    /// Creates a scratch container.
    /// 
    /// Optional platform argument initializes new containers to execute and publish as that platform. Platform defaults to that of the builder's host.
    /// </summary>
    public Container Container(ContainerID id = null, Platform platform = null)
    {
    }

    /// <summary>
    /// The FunctionCall context that the SDK caller is currently executing in.
    /// 
    /// If the caller is not currently executing in a function, this will return an error.
    /// </summary>
    public FunctionCall CurrentFunctionCall()
    {
    }

    /// <summary>
    /// The module currently being served in the session, if any.
    /// </summary>
    public CurrentModule CurrentModule()
    {
    }

    /// <summary>
    /// The TypeDef representations of the objects currently being served in the session.
    /// </summary>
    public CurrentTypeDefs()
    {
    }

    /// <summary>
    /// The default platform of the engine.
    /// </summary>
    public Platform DefaultPlatform()
    {
    }

    /// <summary>
    /// Creates an empty directory.
    /// </summary>
    public Directory Directory(DirectoryID id = null)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public File File(FileID id)
    {
    }

    /// <summary>
    /// Creates a function.
    /// </summary>
    public Function Function(string name, TypeDefID returnType)
    {
    }

    /// <summary>
    /// Create a code generation result, given a directory containing the generated code.
    /// </summary>
    public GeneratedCode GeneratedCode(DirectoryID code)
    {
    }

    /// <summary>
    /// Queries a Git repository.
    /// </summary>
    public GitRepository Git(string url, ServiceID experimentalServiceHost = null, bool keepGitDir = false, SocketID sshAuthSocket = null, string sshKnownHosts = "")
    {
    }

    /// <summary>
    /// Queries the host environment.
    /// </summary>
    public Host Host()
    {
    }

    /// <summary>
    /// Returns a file containing an http remote url content.
    /// </summary>
    public File Http(string url, ServiceID experimentalServiceHost = null)
    {
    }

    /// <summary>
    /// Load a CacheVolume from its ID.
    /// </summary>
    public CacheVolume LoadCacheVolumeFromID(CacheVolumeID id)
    {
    }

    /// <summary>
    /// Load a Container from its ID.
    /// </summary>
    public Container LoadContainerFromID(ContainerID id)
    {
    }

    /// <summary>
    /// Load a CurrentModule from its ID.
    /// </summary>
    public CurrentModule LoadCurrentModuleFromID(CurrentModuleID id)
    {
    }

    /// <summary>
    /// Load a Directory from its ID.
    /// </summary>
    public Directory LoadDirectoryFromID(DirectoryID id)
    {
    }

    /// <summary>
    /// Load a EnvVariable from its ID.
    /// </summary>
    public EnvVariable LoadEnvVariableFromID(EnvVariableID id)
    {
    }

    /// <summary>
    /// Load a FieldTypeDef from its ID.
    /// </summary>
    public FieldTypeDef LoadFieldTypeDefFromID(FieldTypeDefID id)
    {
    }

    /// <summary>
    /// Load a File from its ID.
    /// </summary>
    public File LoadFileFromID(FileID id)
    {
    }

    /// <summary>
    /// Load a FunctionArg from its ID.
    /// </summary>
    public FunctionArg LoadFunctionArgFromID(FunctionArgID id)
    {
    }

    /// <summary>
    /// Load a FunctionCallArgValue from its ID.
    /// </summary>
    public FunctionCallArgValue LoadFunctionCallArgValueFromID(FunctionCallArgValueID id)
    {
    }

    /// <summary>
    /// Load a FunctionCall from its ID.
    /// </summary>
    public FunctionCall LoadFunctionCallFromID(FunctionCallID id)
    {
    }

    /// <summary>
    /// Load a Function from its ID.
    /// </summary>
    public Function LoadFunctionFromID(FunctionID id)
    {
    }

    /// <summary>
    /// Load a GeneratedCode from its ID.
    /// </summary>
    public GeneratedCode LoadGeneratedCodeFromID(GeneratedCodeID id)
    {
    }

    /// <summary>
    /// Load a GitModuleSource from its ID.
    /// </summary>
    public GitModuleSource LoadGitModuleSourceFromID(GitModuleSourceID id)
    {
    }

    /// <summary>
    /// Load a GitRef from its ID.
    /// </summary>
    public GitRef LoadGitRefFromID(GitRefID id)
    {
    }

    /// <summary>
    /// Load a GitRepository from its ID.
    /// </summary>
    public GitRepository LoadGitRepositoryFromID(GitRepositoryID id)
    {
    }

    /// <summary>
    /// Load a Host from its ID.
    /// </summary>
    public Host LoadHostFromID(HostID id)
    {
    }

    /// <summary>
    /// Load a InputTypeDef from its ID.
    /// </summary>
    public InputTypeDef LoadInputTypeDefFromID(InputTypeDefID id)
    {
    }

    /// <summary>
    /// Load a InterfaceTypeDef from its ID.
    /// </summary>
    public InterfaceTypeDef LoadInterfaceTypeDefFromID(InterfaceTypeDefID id)
    {
    }

    /// <summary>
    /// Load a Label from its ID.
    /// </summary>
    public Label LoadLabelFromID(LabelID id)
    {
    }

    /// <summary>
    /// Load a ListTypeDef from its ID.
    /// </summary>
    public ListTypeDef LoadListTypeDefFromID(ListTypeDefID id)
    {
    }

    /// <summary>
    /// Load a LocalModuleSource from its ID.
    /// </summary>
    public LocalModuleSource LoadLocalModuleSourceFromID(LocalModuleSourceID id)
    {
    }

    /// <summary>
    /// Load a ModuleDependency from its ID.
    /// </summary>
    public ModuleDependency LoadModuleDependencyFromID(ModuleDependencyID id)
    {
    }

    /// <summary>
    /// Load a Module from its ID.
    /// </summary>
    public Module LoadModuleFromID(ModuleID id)
    {
    }

    /// <summary>
    /// Load a ModuleSource from its ID.
    /// </summary>
    public ModuleSource LoadModuleSourceFromID(ModuleSourceID id)
    {
    }

    /// <summary>
    /// Load a ModuleSourceView from its ID.
    /// </summary>
    public ModuleSourceView LoadModuleSourceViewFromID(ModuleSourceViewID id)
    {
    }

    /// <summary>
    /// Load a ObjectTypeDef from its ID.
    /// </summary>
    public ObjectTypeDef LoadObjectTypeDefFromID(ObjectTypeDefID id)
    {
    }

    /// <summary>
    /// Load a Port from its ID.
    /// </summary>
    public Port LoadPortFromID(PortID id)
    {
    }

    /// <summary>
    /// Load a Secret from its ID.
    /// </summary>
    public Secret LoadSecretFromID(SecretID id)
    {
    }

    /// <summary>
    /// Load a Service from its ID.
    /// </summary>
    public Service LoadServiceFromID(ServiceID id)
    {
    }

    /// <summary>
    /// Load a Socket from its ID.
    /// </summary>
    public Socket LoadSocketFromID(SocketID id)
    {
    }

    /// <summary>
    /// Load a Terminal from its ID.
    /// </summary>
    public Terminal LoadTerminalFromID(TerminalID id)
    {
    }

    /// <summary>
    /// Load a TypeDef from its ID.
    /// </summary>
    public TypeDef LoadTypeDefFromID(TypeDefID id)
    {
    }

    /// <summary>
    /// Create a new module.
    /// </summary>
    public Module Module()
    {
    }

    /// <summary>
    /// Create a new module dependency configuration from a module source and name
    /// </summary>
    public ModuleDependency ModuleDependency(ModuleSourceID source, string name = "")
    {
    }

    /// <summary>
    /// Create a new module source instance from a source ref string.
    /// </summary>
    public ModuleSource ModuleSource(string refString, bool stable = false)
    {
    }

    /// <summary>
    /// Creates a named sub-pipeline.
    /// </summary>
    public Query Pipeline(string name, string description = "", labels = null)
    {
    }

    /// <summary>
    /// Reference a secret by name.
    /// </summary>
    public Secret Secret(string name, string accessor = null)
    {
    }

    /// <summary>
    /// Sets a secret given a user defined name to its plaintext and returns the secret.
    /// 
    /// The plaintext value is limited to a size of 128000 bytes.
    /// </summary>
    public Secret SetSecret(string name, string plaintext)
    {
    }

    /// <summary>
    /// Loads a socket by its ID.
    /// </summary>
    public Socket Socket(SocketID id)
    {
    }

    /// <summary>
    /// Create a new TypeDef.
    /// </summary>
    public TypeDef TypeDef()
    {
    }
}

/// <summary>
/// A reference to a secret value, which can be handled more safely than the value itself.
/// </summary>
public class Secret : Object
{
    /// <summary>
    /// A unique identifier for this Secret.
    /// </summary>
    public SecretID Id()
    {
    }

    /// <summary>
    /// The name of this secret.
    /// </summary>
    public string Name()
    {
    }

    /// <summary>
    /// The value of this secret.
    /// </summary>
    public string Plaintext()
    {
    }
}

/// <summary>
/// The `SecretID` scalar type represents an identifier for an object of type Secret.
/// </summary>
public class SecretID : Scalar
{
}

/// <summary>
/// A content-addressed service providing TCP connectivity.
/// </summary>
public class Service : Object
{
    /// <summary>
    /// Retrieves an endpoint that clients can use to reach this container.
    /// 
    /// If no port is specified, the first exposed port is used. If none exist an error is returned.
    /// 
    /// If a scheme is specified, a URL is returned. Otherwise, a host:port pair is returned.
    /// </summary>
    public string Endpoint(int port = null, string scheme = "")
    {
    }

    /// <summary>
    /// Retrieves a hostname which can be used by clients to reach this container.
    /// </summary>
    public string Hostname()
    {
    }

    /// <summary>
    /// A unique identifier for this Service.
    /// </summary>
    public ServiceID Id()
    {
    }

    /// <summary>
    /// Retrieves the list of ports provided by the service.
    /// </summary>
    public Ports()
    {
    }

    /// <summary>
    /// Start the service and wait for its health checks to succeed.
    /// 
    /// Services bound to a Container do not need to be manually started.
    /// </summary>
    public ServiceID Start()
    {
    }

    /// <summary>
    /// Stop the service.
    /// </summary>
    public ServiceID Stop(bool kill = false)
    {
    }

    /// <summary>
    /// Creates a tunnel that forwards traffic from the caller's network to this service.
    /// </summary>
    public Void Up(ports = [], bool random = false)
    {
    }
}

/// <summary>
/// The `ServiceID` scalar type represents an identifier for an object of type Service.
/// </summary>
public class ServiceID : Scalar
{
}

/// <summary>
/// A Unix or TCP/IP socket that can be mounted into a container.
/// </summary>
public class Socket : Object
{
    /// <summary>
    /// A unique identifier for this Socket.
    /// </summary>
    public SocketID Id()
    {
    }
}

/// <summary>
/// The `SocketID` scalar type represents an identifier for an object of type Socket.
/// </summary>
public class SocketID : Scalar
{
}

/// <summary>
/// An interactive terminal that clients can connect to.
/// </summary>
public class Terminal : Object
{
    /// <summary>
    /// A unique identifier for this Terminal.
    /// </summary>
    public TerminalID Id()
    {
    }

    /// <summary>
    /// An http endpoint at which this terminal can be connected to over a websocket.
    /// </summary>
    public string WebsocketEndpoint()
    {
    }
}

/// <summary>
/// The `TerminalID` scalar type represents an identifier for an object of type Terminal.
/// </summary>
public class TerminalID : Scalar
{
}

/// <summary>
/// A definition of a parameter or return type in a Module.
/// </summary>
public class TypeDef : Object
{
    /// <summary>
    /// If kind is INPUT, the input-specific type definition. If kind is not INPUT, this will be null.
    /// </summary>
    public InputTypeDef AsInput()
    {
    }

    /// <summary>
    /// If kind is INTERFACE, the interface-specific type definition. If kind is not INTERFACE, this will be null.
    /// </summary>
    public InterfaceTypeDef AsInterface()
    {
    }

    /// <summary>
    /// If kind is LIST, the list-specific type definition. If kind is not LIST, this will be null.
    /// </summary>
    public ListTypeDef AsList()
    {
    }

    /// <summary>
    /// If kind is OBJECT, the object-specific type definition. If kind is not OBJECT, this will be null.
    /// </summary>
    public ObjectTypeDef AsObject()
    {
    }

    /// <summary>
    /// A unique identifier for this TypeDef.
    /// </summary>
    public TypeDefID Id()
    {
    }

    /// <summary>
    /// The kind of type this is (e.g. primitive, list, object).
    /// </summary>
    public TypeDefKind Kind()
    {
    }

    /// <summary>
    /// Whether this type can be set to null. Defaults to false.
    /// </summary>
    public bool Optional()
    {
    }

    /// <summary>
    /// Adds a function for constructing a new instance of an Object TypeDef, failing if the type is not an object.
    /// </summary>
    public TypeDef WithConstructor(FunctionID function)
    {
    }

    /// <summary>
    /// Adds a static field for an Object TypeDef, failing if the type is not an object.
    /// </summary>
    public TypeDef WithField(string name, TypeDefID typeDef, string description = "")
    {
    }

    /// <summary>
    /// Adds a function for an Object or Interface TypeDef, failing if the type is not one of those kinds.
    /// </summary>
    public TypeDef WithFunction(FunctionID function)
    {
    }

    /// <summary>
    /// Returns a TypeDef of kind Interface with the provided name.
    /// </summary>
    public TypeDef WithInterface(string name, string description = "")
    {
    }

    /// <summary>
    /// Sets the kind of the type.
    /// </summary>
    public TypeDef WithKind(TypeDefKind kind)
    {
    }

    /// <summary>
    /// Returns a TypeDef of kind List with the provided type for its elements.
    /// </summary>
    public TypeDef WithListOf(TypeDefID elementType)
    {
    }

    /// <summary>
    /// Returns a TypeDef of kind Object with the provided name.
    /// 
    /// Note that an object's fields and functions may be omitted if the intent is only to refer to an object. This is how functions are able to return their own object, or any other circular reference.
    /// </summary>
    public TypeDef WithObject(string name, string description = "")
    {
    }

    /// <summary>
    /// Sets whether this type can be set to null.
    /// </summary>
    public TypeDef WithOptional(bool optional)
    {
    }
}

/// <summary>
/// The `TypeDefID` scalar type represents an identifier for an object of type TypeDef.
/// </summary>
public class TypeDefID : Scalar
{
}

/// <summary>
/// Distinguishes the different kinds of TypeDefs.
/// </summary>
public enum TypeDefKind
{
    STRING_KIND,
    INTEGER_KIND,
    BOOLEAN_KIND,
    LIST_KIND,
    OBJECT_KIND,
    INTERFACE_KIND,
    INPUT_KIND,
    VOID_KIND
}

/// <summary>
/// The absence of a value.
/// 
/// A Null Void is used as a placeholder for resolvers that do not return anything.
/// </summary>
public class Void : Scalar
{
}
