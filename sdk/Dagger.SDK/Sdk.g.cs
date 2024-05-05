using System.Collections.Immutable;
using Dagger.SDK.GraphQL;

namespace Dagger.SDK;
public class Scalar
{
    public readonly string Value;
    public override string ToString() => Value;
}

public class Object(QueryBuilder queryBuilder, GraphQLClient gqlClient)
{
    public QueryBuilder QueryBuilder { get; } = queryBuilder;
    public GraphQLClient GraphQLClient { get; } = gqlClient;
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
public class CacheVolume(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// A unique identifier for this CacheVolume.
    /// </summary>
    public async Task<CacheVolumeID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<CacheVolumeID>(GraphQLClient, QueryBuilder);
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
public class Container(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// Turn the container into a Service.
    /// 
    /// Be sure to set any exposed ports before this conversion.
    /// </summary>
    public Service AsService()
    {
        var queryBuilder = QueryBuilder.Select("asService");
        return new Service(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Returns a File representing the container serialized to a tarball.
    /// </summary>
    public File AsTarball(ImageLayerCompression? forcedCompression = null, ImageMediaTypes mediaTypes = ImageMediaTypes.OCIMediaTypes, ContainerID[] platformVariants = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("asTarball");
        return new File(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Initializes this container from a Dockerfile build.
    /// </summary>
    public Container Build(DirectoryID context, BuildArg[] buildArgs = null, string dockerfile = "Dockerfile", SecretID[] secrets = null, string target = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("build");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves default arguments for future commands.
    /// </summary>
    public async Task<string[]> DefaultArgs()
    {
        var queryBuilder = QueryBuilder.Select("defaultArgs");
        return await Engine.Execute<string[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves a directory at the given path.
    /// 
    /// Mounts are included.
    /// </summary>
    public Directory Directory(string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("directory");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves entrypoint to be prepended to the arguments of all commands.
    /// </summary>
    public async Task<string[]> Entrypoint()
    {
        var queryBuilder = QueryBuilder.Select("entrypoint");
        return await Engine.Execute<string[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves the value of the specified environment variable.
    /// </summary>
    public async Task<string> EnvVariable(string name)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("envVariable");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves the list of environment variables passed to commands.
    /// </summary>
    public async Task<EnvVariable[]> EnvVariables()
    {
        var queryBuilder = QueryBuilder.Select("envVariables");
        return await Engine.Execute<EnvVariable[]>(GraphQLClient, QueryBuilder);
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
        var queryBuilder = QueryBuilder.Select("experimentalWithAllGPUs");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// EXPERIMENTAL API! Subject to change/removal at any time.
    /// 
    /// Configures the provided list of devices to be accessible to this container.
    /// 
    /// This currently works for Nvidia devices only.
    /// </summary>
    public Container ExperimentalWithGPU(string[] devices)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("experimentalWithGPU");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Writes the container as an OCI tarball to the destination file path on the host.
    /// 
    /// Return true on success.
    /// 
    /// It can also export platform variants.
    /// </summary>
    public async Task<bool> Export(string path, ImageLayerCompression? forcedCompression = null, ImageMediaTypes mediaTypes = ImageMediaTypes.OCIMediaTypes, ContainerID[] platformVariants = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("export");
        return await Engine.Execute<bool>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves the list of exposed ports.
    /// 
    /// This includes ports already exposed by the image, even if not explicitly added with dagger.
    /// </summary>
    public async Task<Port[]> ExposedPorts()
    {
        var queryBuilder = QueryBuilder.Select("exposedPorts");
        return await Engine.Execute<Port[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves a file at the given path.
    /// 
    /// Mounts are included.
    /// </summary>
    public File File(string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("file");
        return new File(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Initializes this container from a pulled base image.
    /// </summary>
    public Container From(string address)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("from");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// A unique identifier for this Container.
    /// </summary>
    public async Task<ContainerID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<ContainerID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The unique image reference which can only be retrieved immediately after the 'Container.From' call.
    /// </summary>
    public async Task<string> ImageRef()
    {
        var queryBuilder = QueryBuilder.Select("imageRef");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Reads the container from an OCI tarball.
    /// </summary>
    public Container Import(FileID source, string tag = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("import");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves the value of the specified label.
    /// </summary>
    public async Task<string> Label(string name)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("label");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves the list of labels passed to container.
    /// </summary>
    public async Task<Label[]> Labels()
    {
        var queryBuilder = QueryBuilder.Select("labels");
        return await Engine.Execute<Label[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves the list of paths where a directory is mounted.
    /// </summary>
    public async Task<string[]> Mounts()
    {
        var queryBuilder = QueryBuilder.Select("mounts");
        return await Engine.Execute<string[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Creates a named sub-pipeline.
    /// </summary>
    public Container Pipeline(string name, string description = "", PipelineLabel[] labels = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("pipeline");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// The platform this container executes and publishes as.
    /// </summary>
    public async Task<Platform> Platform()
    {
        var queryBuilder = QueryBuilder.Select("platform");
        return await Engine.Execute<Platform>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Publishes this container as a new image to the specified address.
    /// 
    /// Publish returns a fully qualified ref.
    /// 
    /// It can also publish platform variants.
    /// </summary>
    public async Task<string> Publish(string address, ImageLayerCompression? forcedCompression = null, ImageMediaTypes mediaTypes = ImageMediaTypes.OCIMediaTypes, ContainerID[] platformVariants = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("publish");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves this container's root filesystem. Mounts are not included.
    /// </summary>
    public Directory Rootfs()
    {
        var queryBuilder = QueryBuilder.Select("rootfs");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// The error stream of the last executed command.
    /// 
    /// Will execute default command if none is set, or error if there's no default.
    /// </summary>
    public async Task<string> Stderr()
    {
        var queryBuilder = QueryBuilder.Select("stderr");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The output stream of the last executed command.
    /// 
    /// Will execute default command if none is set, or error if there's no default.
    /// </summary>
    public async Task<string> Stdout()
    {
        var queryBuilder = QueryBuilder.Select("stdout");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Forces evaluation of the pipeline in the engine.
    /// 
    /// It doesn't run the default command if no exec has been set.
    /// </summary>
    public async Task<ContainerID> Sync()
    {
        var queryBuilder = QueryBuilder.Select("sync");
        return await Engine.Execute<ContainerID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Return an interactive terminal for this container using its configured default terminal command if not overridden by args (or sh as a fallback default).
    /// </summary>
    public Terminal Terminal(string[] cmd = null, bool experimentalPrivilegedNesting = false, bool insecureRootCapabilities = false)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("terminal");
        return new Terminal(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves the user to be set for all commands.
    /// </summary>
    public async Task<string> User()
    {
        var queryBuilder = QueryBuilder.Select("user");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Configures default arguments for future commands.
    /// </summary>
    public Container WithDefaultArgs(string[] args)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withDefaultArgs");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Set the default command to invoke for the container's terminal API.
    /// </summary>
    public Container WithDefaultTerminalCmd(string[] args, bool experimentalPrivilegedNesting = false, bool insecureRootCapabilities = false)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withDefaultTerminalCmd");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container plus a directory written at the given path.
    /// </summary>
    public Container WithDirectory(DirectoryID directory, string path, string[] exclude = null, string[] include = null, string owner = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withDirectory");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container but with a different command entrypoint.
    /// </summary>
    public Container WithEntrypoint(string[] args, bool keepDefaultArgs = false)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withEntrypoint");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container plus the given environment variable.
    /// </summary>
    public Container WithEnvVariable(string name, string value, bool expand = false)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withEnvVariable");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container after executing the specified command inside it.
    /// </summary>
    public Container WithExec(string[] args, bool experimentalPrivilegedNesting = false, bool insecureRootCapabilities = false, string redirectStderr = "", string redirectStdout = "", bool skipEntrypoint = false, string stdin = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withExec");
        return new Container(QueryBuilder, GraphQLClient);
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
    public Container WithExposedPort(int port, string? description = null, bool experimentalSkipHealthcheck = false, NetworkProtocol protocol = NetworkProtocol.TCP)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withExposedPort");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container plus the contents of the given file copied to the given path.
    /// </summary>
    public Container WithFile(string path, FileID source, string owner = "", int? permissions = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withFile");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container plus the contents of the given files copied to the given path.
    /// </summary>
    public Container WithFiles(string path, FileID[] sources, string owner = "", int? permissions = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withFiles");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Indicate that subsequent operations should be featured more prominently in the UI.
    /// </summary>
    public Container WithFocus()
    {
        var queryBuilder = QueryBuilder.Select("withFocus");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container plus the given label.
    /// </summary>
    public Container WithLabel(string name, string value)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withLabel");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container plus a cache volume mounted at the given path.
    /// </summary>
    public Container WithMountedCache(CacheVolumeID cache, string path, string owner = "", CacheSharingMode sharing = CacheSharingMode.SHARED, DirectoryID? source = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withMountedCache");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container plus a directory mounted at the given path.
    /// </summary>
    public Container WithMountedDirectory(string path, DirectoryID source, string owner = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withMountedDirectory");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container plus a file mounted at the given path.
    /// </summary>
    public Container WithMountedFile(string path, FileID source, string owner = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withMountedFile");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container plus a secret mounted into a file at the given path.
    /// </summary>
    public Container WithMountedSecret(string path, SecretID source, int mode = 256, string owner = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withMountedSecret");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container plus a temporary directory mounted at the given path.
    /// </summary>
    public Container WithMountedTemp(string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withMountedTemp");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container plus a new file written at the given path.
    /// </summary>
    public Container WithNewFile(string path, string contents = "", string owner = "", int permissions = 420)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withNewFile");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container with a registry authentication for a given address.
    /// </summary>
    public Container WithRegistryAuth(string address, SecretID secret, string username)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withRegistryAuth");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves the container with the given directory mounted to /.
    /// </summary>
    public Container WithRootfs(DirectoryID directory)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withRootfs");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container plus an env variable containing the given secret.
    /// </summary>
    public Container WithSecretVariable(string name, SecretID secret)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withSecretVariable");
        return new Container(QueryBuilder, GraphQLClient);
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
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withServiceBinding");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container plus a socket forwarded to the given Unix socket path.
    /// </summary>
    public Container WithUnixSocket(string path, SocketID source, string owner = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withUnixSocket");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container with a different command user.
    /// </summary>
    public Container WithUser(string name)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withUser");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container with a different working directory.
    /// </summary>
    public Container WithWorkdir(string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withWorkdir");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container with unset default arguments for future commands.
    /// </summary>
    public Container WithoutDefaultArgs()
    {
        var queryBuilder = QueryBuilder.Select("withoutDefaultArgs");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container with an unset command entrypoint.
    /// </summary>
    public Container WithoutEntrypoint(bool keepDefaultArgs = false)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withoutEntrypoint");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container minus the given environment variable.
    /// </summary>
    public Container WithoutEnvVariable(string name)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withoutEnvVariable");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Unexpose a previously exposed port.
    /// </summary>
    public Container WithoutExposedPort(int port, NetworkProtocol protocol = NetworkProtocol.TCP)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withoutExposedPort");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Indicate that subsequent operations should not be featured more prominently in the UI.
    /// 
    /// This is the initial state of all containers.
    /// </summary>
    public Container WithoutFocus()
    {
        var queryBuilder = QueryBuilder.Select("withoutFocus");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container minus the given environment label.
    /// </summary>
    public Container WithoutLabel(string name)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withoutLabel");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container after unmounting everything at the given path.
    /// </summary>
    public Container WithoutMount(string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withoutMount");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container without the registry authentication of a given address.
    /// </summary>
    public Container WithoutRegistryAuth(string address)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withoutRegistryAuth");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container with a previously added Unix socket removed.
    /// </summary>
    public Container WithoutUnixSocket(string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withoutUnixSocket");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container with an unset command user.
    /// 
    /// Should default to root.
    /// </summary>
    public Container WithoutUser()
    {
        var queryBuilder = QueryBuilder.Select("withoutUser");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this container with an unset working directory.
    /// 
    /// Should default to "/".
    /// </summary>
    public Container WithoutWorkdir()
    {
        var queryBuilder = QueryBuilder.Select("withoutWorkdir");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves the working directory for all commands.
    /// </summary>
    public async Task<string> Workdir()
    {
        var queryBuilder = QueryBuilder.Select("workdir");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
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
public class CurrentModule(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// A unique identifier for this CurrentModule.
    /// </summary>
    public async Task<CurrentModuleID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<CurrentModuleID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The name of the module being executed in
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The directory containing the module's source code loaded into the engine (plus any generated code that may have been created).
    /// </summary>
    public Directory Source()
    {
        var queryBuilder = QueryBuilder.Select("source");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a directory from the module's scratch working directory, including any changes that may have been made to it during module function execution.
    /// </summary>
    public Directory Workdir(string path, string[] exclude = null, string[] include = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("workdir");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a file from the module's scratch working directory, including any changes that may have been made to it during module function execution.Load a file from the module's scratch working directory, including any changes that may have been made to it during module function execution.
    /// </summary>
    public File WorkdirFile(string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("workdirFile");
        return new File(QueryBuilder, GraphQLClient);
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
public class Directory(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// Load the directory as a Dagger module
    /// </summary>
    public Module AsModule(string sourceRootPath = ".")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("asModule");
        return new Module(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Gets the difference between this directory and an another directory.
    /// </summary>
    public Directory Diff(DirectoryID other)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("diff");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves a directory at the given path.
    /// </summary>
    public Directory Directory_(string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("directory");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Builds a new Docker container from this directory.
    /// </summary>
    public Container DockerBuild(BuildArg[] buildArgs = null, string dockerfile = "Dockerfile", Platform? platform = null, SecretID[] secrets = null, string target = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("dockerBuild");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Returns a list of files and directories at the given path.
    /// </summary>
    public async Task<string[]> Entries(string? path = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("entries");
        return await Engine.Execute<string[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Writes the contents of the directory to a path on the host.
    /// </summary>
    public async Task<bool> Export(string path, bool wipe = false)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("export");
        return await Engine.Execute<bool>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves a file at the given path.
    /// </summary>
    public File File(string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("file");
        return new File(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Returns a list of files and directories that matche the given pattern.
    /// </summary>
    public async Task<string[]> Glob(string pattern)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("glob");
        return await Engine.Execute<string[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// A unique identifier for this Directory.
    /// </summary>
    public async Task<DirectoryID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<DirectoryID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Creates a named sub-pipeline.
    /// </summary>
    public Directory Pipeline(string name, string description = "", PipelineLabel[] labels = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("pipeline");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Force evaluation in the engine.
    /// </summary>
    public async Task<DirectoryID> Sync()
    {
        var queryBuilder = QueryBuilder.Select("sync");
        return await Engine.Execute<DirectoryID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves this directory plus a directory written at the given path.
    /// </summary>
    public Directory WithDirectory(DirectoryID directory, string path, string[] exclude = null, string[] include = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withDirectory");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this directory plus the contents of the given file copied to the given path.
    /// </summary>
    public Directory WithFile(string path, FileID source, int? permissions = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withFile");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this directory plus the contents of the given files copied to the given path.
    /// </summary>
    public Directory WithFiles(string path, FileID[] sources, int? permissions = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withFiles");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this directory plus a new directory created at the given path.
    /// </summary>
    public Directory WithNewDirectory(string path, int permissions = 420)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withNewDirectory");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this directory plus a new file written at the given path.
    /// </summary>
    public Directory WithNewFile(string contents, string path, int permissions = 420)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withNewFile");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this directory with all file/dir timestamps set to the given time.
    /// </summary>
    public Directory WithTimestamps(int timestamp)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withTimestamps");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this directory with the directory at the given path removed.
    /// </summary>
    public Directory WithoutDirectory(string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withoutDirectory");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves this directory with the file at the given path removed.
    /// </summary>
    public Directory WithoutFile(string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withoutFile");
        return new Directory(QueryBuilder, GraphQLClient);
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
public class EnvVariable(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// A unique identifier for this EnvVariable.
    /// </summary>
    public async Task<EnvVariableID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<EnvVariableID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The environment variable name.
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The environment variable value.
    /// </summary>
    public async Task<string> Value()
    {
        var queryBuilder = QueryBuilder.Select("value");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
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
public class FieldTypeDef(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// A doc string for the field, if any.
    /// </summary>
    public async Task<string> Description()
    {
        var queryBuilder = QueryBuilder.Select("description");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// A unique identifier for this FieldTypeDef.
    /// </summary>
    public async Task<FieldTypeDefID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<FieldTypeDefID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The name of the field in lowerCamelCase format.
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The type of the field.
    /// </summary>
    public TypeDef TypeDef()
    {
        var queryBuilder = QueryBuilder.Select("typeDef");
        return new TypeDef(QueryBuilder, GraphQLClient);
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
public class File(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// Retrieves the contents of the file.
    /// </summary>
    public async Task<string> Contents()
    {
        var queryBuilder = QueryBuilder.Select("contents");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Writes the file to a file path on the host.
    /// </summary>
    public async Task<bool> Export(string path, bool allowParentDirPath = false)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("export");
        return await Engine.Execute<bool>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// A unique identifier for this File.
    /// </summary>
    public async Task<FileID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<FileID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves the name of the file.
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves the size of the file, in bytes.
    /// </summary>
    public async Task<int> Size()
    {
        var queryBuilder = QueryBuilder.Select("size");
        return await Engine.Execute<int>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Force evaluation in the engine.
    /// </summary>
    public async Task<FileID> Sync()
    {
        var queryBuilder = QueryBuilder.Select("sync");
        return await Engine.Execute<FileID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves this file with its created/modified timestamps set to the given time.
    /// </summary>
    public File WithTimestamps(int timestamp)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withTimestamps");
        return new File(QueryBuilder, GraphQLClient);
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
public class Function(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// Arguments accepted by the function, if any.
    /// </summary>
    public async Task<FunctionArg[]> Args()
    {
        var queryBuilder = QueryBuilder.Select("args");
        return await Engine.Execute<FunctionArg[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// A doc string for the function, if any.
    /// </summary>
    public async Task<string> Description()
    {
        var queryBuilder = QueryBuilder.Select("description");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// A unique identifier for this Function.
    /// </summary>
    public async Task<FunctionID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<FunctionID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The name of the function.
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The type returned by the function.
    /// </summary>
    public TypeDef ReturnType()
    {
        var queryBuilder = QueryBuilder.Select("returnType");
        return new TypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Returns the function with the provided argument
    /// </summary>
    public Function WithArg(string name, TypeDefID typeDef, JSON? defaultValue = null, string description = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withArg");
        return new Function(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Returns the function with the given doc string.
    /// </summary>
    public Function WithDescription(string description)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withDescription");
        return new Function(QueryBuilder, GraphQLClient);
    }
}

/// <summary>
/// An argument accepted by a function.
/// 
/// This is a specification for an argument at function definition time, not an argument passed at function call time.
/// </summary>
public class FunctionArg(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// A default value to use for this argument when not explicitly set by the caller, if any.
    /// </summary>
    public async Task<JSON> DefaultValue()
    {
        var queryBuilder = QueryBuilder.Select("defaultValue");
        return await Engine.Execute<JSON>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// A doc string for the argument, if any.
    /// </summary>
    public async Task<string> Description()
    {
        var queryBuilder = QueryBuilder.Select("description");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// A unique identifier for this FunctionArg.
    /// </summary>
    public async Task<FunctionArgID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<FunctionArgID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The name of the argument in lowerCamelCase format.
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The type of the argument.
    /// </summary>
    public TypeDef TypeDef()
    {
        var queryBuilder = QueryBuilder.Select("typeDef");
        return new TypeDef(QueryBuilder, GraphQLClient);
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
public class FunctionCall(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// A unique identifier for this FunctionCall.
    /// </summary>
    public async Task<FunctionCallID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<FunctionCallID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The argument values the function is being invoked with.
    /// </summary>
    public async Task<FunctionCallArgValue[]> InputArgs()
    {
        var queryBuilder = QueryBuilder.Select("inputArgs");
        return await Engine.Execute<FunctionCallArgValue[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The name of the function being called.
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The value of the parent object of the function being called. If the function is top-level to the module, this is always an empty object.
    /// </summary>
    public async Task<JSON> Parent()
    {
        var queryBuilder = QueryBuilder.Select("parent");
        return await Engine.Execute<JSON>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The name of the parent object of the function being called. If the function is top-level to the module, this is the name of the module.
    /// </summary>
    public async Task<string> ParentName()
    {
        var queryBuilder = QueryBuilder.Select("parentName");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Set the return value of the function call to the provided value.
    /// </summary>
    public async Task<Void> ReturnValue(JSON value)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("returnValue");
        return await Engine.Execute<Void>(GraphQLClient, QueryBuilder);
    }
}

/// <summary>
/// A value passed as a named argument to a function call.
/// </summary>
public class FunctionCallArgValue(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// A unique identifier for this FunctionCallArgValue.
    /// </summary>
    public async Task<FunctionCallArgValueID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<FunctionCallArgValueID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The name of the argument.
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The value of the argument represented as a JSON serialized string.
    /// </summary>
    public async Task<JSON> Value()
    {
        var queryBuilder = QueryBuilder.Select("value");
        return await Engine.Execute<JSON>(GraphQLClient, QueryBuilder);
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
public class GeneratedCode(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// The directory containing the generated code.
    /// </summary>
    public Directory Code()
    {
        var queryBuilder = QueryBuilder.Select("code");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// A unique identifier for this GeneratedCode.
    /// </summary>
    public async Task<GeneratedCodeID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<GeneratedCodeID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// List of paths to mark generated in version control (i.e. .gitattributes).
    /// </summary>
    public async Task<string[]> VcsGeneratedPaths()
    {
        var queryBuilder = QueryBuilder.Select("vcsGeneratedPaths");
        return await Engine.Execute<string[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// List of paths to ignore in version control (i.e. .gitignore).
    /// </summary>
    public async Task<string[]> VcsIgnoredPaths()
    {
        var queryBuilder = QueryBuilder.Select("vcsIgnoredPaths");
        return await Engine.Execute<string[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Set the list of paths to mark generated in version control.
    /// </summary>
    public GeneratedCode WithVCSGeneratedPaths(string[] paths)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withVCSGeneratedPaths");
        return new GeneratedCode(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Set the list of paths to ignore in version control.
    /// </summary>
    public GeneratedCode WithVCSIgnoredPaths(string[] paths)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withVCSIgnoredPaths");
        return new GeneratedCode(QueryBuilder, GraphQLClient);
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
public class GitModuleSource(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// The URL from which the source's git repo can be cloned.
    /// </summary>
    public async Task<string> CloneURL()
    {
        var queryBuilder = QueryBuilder.Select("cloneURL");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The resolved commit of the git repo this source points to.
    /// </summary>
    public async Task<string> Commit()
    {
        var queryBuilder = QueryBuilder.Select("commit");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The directory containing everything needed to load load and use the module.
    /// </summary>
    public Directory ContextDirectory()
    {
        var queryBuilder = QueryBuilder.Select("contextDirectory");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// The URL to the source's git repo in a web browser
    /// </summary>
    public async Task<string> HtmlURL()
    {
        var queryBuilder = QueryBuilder.Select("htmlURL");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// A unique identifier for this GitModuleSource.
    /// </summary>
    public async Task<GitModuleSourceID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<GitModuleSourceID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The path to the root of the module source under the context directory. This directory contains its configuration file. It also contains its source code (possibly as a subdirectory).
    /// </summary>
    public async Task<string> RootSubpath()
    {
        var queryBuilder = QueryBuilder.Select("rootSubpath");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The specified version of the git repo this source points to.
    /// </summary>
    public async Task<string> Version()
    {
        var queryBuilder = QueryBuilder.Select("version");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
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
public class GitRef(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// The resolved commit id at this ref.
    /// </summary>
    public async Task<string> Commit()
    {
        var queryBuilder = QueryBuilder.Select("commit");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// A unique identifier for this GitRef.
    /// </summary>
    public async Task<GitRefID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<GitRefID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The filesystem tree at this ref.
    /// </summary>
    public Directory Tree(SocketID? sshAuthSocket = null, string? sshKnownHosts = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("tree");
        return new Directory(QueryBuilder, GraphQLClient);
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
public class GitRepository(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// Returns details of a branch.
    /// </summary>
    public GitRef Branch(string name)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("branch");
        return new GitRef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Returns details of a commit.
    /// </summary>
    public GitRef Commit(string id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("commit");
        return new GitRef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Returns details for HEAD.
    /// </summary>
    public GitRef Head()
    {
        var queryBuilder = QueryBuilder.Select("head");
        return new GitRef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// A unique identifier for this GitRepository.
    /// </summary>
    public async Task<GitRepositoryID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<GitRepositoryID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Returns details of a ref.
    /// </summary>
    public GitRef Ref(string name)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("ref");
        return new GitRef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Returns details of a tag.
    /// </summary>
    public GitRef Tag(string name)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("tag");
        return new GitRef(QueryBuilder, GraphQLClient);
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
public class Host(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// Accesses a directory on the host.
    /// </summary>
    public Directory Directory(string path, string[] exclude = null, string[] include = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("directory");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Accesses a file on the host.
    /// </summary>
    public File File(string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("file");
        return new File(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// A unique identifier for this Host.
    /// </summary>
    public async Task<HostID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<HostID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Creates a service that forwards traffic to a specified address via the host.
    /// </summary>
    public Service Service(PortForward[] ports, string host = "localhost")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("service");
        return new Service(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Sets a secret given a user-defined name and the file path on the host, and returns the secret.
    /// 
    /// The file is limited to a size of 512000 bytes.
    /// </summary>
    public Secret SetSecretFile(string name, string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("setSecretFile");
        return new Secret(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Creates a tunnel that forwards traffic from the host to a service.
    /// </summary>
    public Service Tunnel(ServiceID service, bool native = false, PortForward[] ports = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("tunnel");
        return new Service(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Accesses a Unix socket on the host.
    /// </summary>
    public Socket UnixSocket(string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("unixSocket");
        return new Socket(QueryBuilder, GraphQLClient);
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
public class InputTypeDef(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// Static fields defined on this input object, if any.
    /// </summary>
    public async Task<FieldTypeDef[]> Fields()
    {
        var queryBuilder = QueryBuilder.Select("fields");
        return await Engine.Execute<FieldTypeDef[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// A unique identifier for this InputTypeDef.
    /// </summary>
    public async Task<InputTypeDefID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<InputTypeDefID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The name of the input object.
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
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
public class InterfaceTypeDef(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// The doc string for the interface, if any.
    /// </summary>
    public async Task<string> Description()
    {
        var queryBuilder = QueryBuilder.Select("description");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Functions defined on this interface, if any.
    /// </summary>
    public async Task<Function[]> Functions()
    {
        var queryBuilder = QueryBuilder.Select("functions");
        return await Engine.Execute<Function[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// A unique identifier for this InterfaceTypeDef.
    /// </summary>
    public async Task<InterfaceTypeDefID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<InterfaceTypeDefID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The name of the interface.
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// If this InterfaceTypeDef is associated with a Module, the name of the module. Unset otherwise.
    /// </summary>
    public async Task<string> SourceModuleName()
    {
        var queryBuilder = QueryBuilder.Select("sourceModuleName");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
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
public class Label(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// A unique identifier for this Label.
    /// </summary>
    public async Task<LabelID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<LabelID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The label name.
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The label value.
    /// </summary>
    public async Task<string> Value()
    {
        var queryBuilder = QueryBuilder.Select("value");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
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
public class ListTypeDef(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// The type of the elements in the list.
    /// </summary>
    public TypeDef ElementTypeDef()
    {
        var queryBuilder = QueryBuilder.Select("elementTypeDef");
        return new TypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// A unique identifier for this ListTypeDef.
    /// </summary>
    public async Task<ListTypeDefID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<ListTypeDefID>(GraphQLClient, QueryBuilder);
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
public class LocalModuleSource(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// The directory containing everything needed to load load and use the module.
    /// </summary>
    public Directory ContextDirectory()
    {
        var queryBuilder = QueryBuilder.Select("contextDirectory");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// A unique identifier for this LocalModuleSource.
    /// </summary>
    public async Task<LocalModuleSourceID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<LocalModuleSourceID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The path to the root of the module source under the context directory. This directory contains its configuration file. It also contains its source code (possibly as a subdirectory).
    /// </summary>
    public async Task<string> RootSubpath()
    {
        var queryBuilder = QueryBuilder.Select("rootSubpath");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
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
public class Module(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// Modules used by this module.
    /// </summary>
    public async Task<Module[]> Dependencies()
    {
        var queryBuilder = QueryBuilder.Select("dependencies");
        return await Engine.Execute<Module[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The dependencies as configured by the module.
    /// </summary>
    public async Task<ModuleDependency[]> DependencyConfig()
    {
        var queryBuilder = QueryBuilder.Select("dependencyConfig");
        return await Engine.Execute<ModuleDependency[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The doc string of the module, if any
    /// </summary>
    public async Task<string> Description()
    {
        var queryBuilder = QueryBuilder.Select("description");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The generated files and directories made on top of the module source's context directory.
    /// </summary>
    public Directory GeneratedContextDiff()
    {
        var queryBuilder = QueryBuilder.Select("generatedContextDiff");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// The module source's context plus any configuration and source files created by codegen.
    /// </summary>
    public Directory GeneratedContextDirectory()
    {
        var queryBuilder = QueryBuilder.Select("generatedContextDirectory");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// A unique identifier for this Module.
    /// </summary>
    public async Task<ModuleID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<ModuleID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves the module with the objects loaded via its SDK.
    /// </summary>
    public Module Initialize()
    {
        var queryBuilder = QueryBuilder.Select("initialize");
        return new Module(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Interfaces served by this module.
    /// </summary>
    public async Task<TypeDef[]> Interfaces()
    {
        var queryBuilder = QueryBuilder.Select("interfaces");
        return await Engine.Execute<TypeDef[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The name of the module
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Objects served by this module.
    /// </summary>
    public async Task<TypeDef[]> Objects()
    {
        var queryBuilder = QueryBuilder.Select("objects");
        return await Engine.Execute<TypeDef[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The container that runs the module's entrypoint. It will fail to execute if the module doesn't compile.
    /// </summary>
    public Container Runtime()
    {
        var queryBuilder = QueryBuilder.Select("runtime");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// The SDK used by this module. Either a name of a builtin SDK or a module source ref string pointing to the SDK's implementation.
    /// </summary>
    public async Task<string> Sdk()
    {
        var queryBuilder = QueryBuilder.Select("sdk");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Serve a module's API in the current session.
    /// 
    /// Note: this can only be called once per session. In the future, it could return a stream or service to remove the side effect.
    /// </summary>
    public async Task<Void> Serve()
    {
        var queryBuilder = QueryBuilder.Select("serve");
        return await Engine.Execute<Void>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The source for the module.
    /// </summary>
    public ModuleSource Source()
    {
        var queryBuilder = QueryBuilder.Select("source");
        return new ModuleSource(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves the module with the given description
    /// </summary>
    public Module WithDescription(string description)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withDescription");
        return new Module(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// This module plus the given Interface type and associated functions
    /// </summary>
    public Module WithInterface(TypeDefID iface)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withInterface");
        return new Module(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// This module plus the given Object type and associated functions.
    /// </summary>
    public Module WithObject(TypeDefID object_)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withObject");
        return new Module(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves the module with basic configuration loaded if present.
    /// </summary>
    public Module WithSource(ModuleSourceID source)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withSource");
        return new Module(QueryBuilder, GraphQLClient);
    }
}

/// <summary>
/// The configuration of dependency of a module.
/// </summary>
public class ModuleDependency(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// A unique identifier for this ModuleDependency.
    /// </summary>
    public async Task<ModuleDependencyID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<ModuleDependencyID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The name of the dependency module.
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The source for the dependency module.
    /// </summary>
    public ModuleSource Source()
    {
        var queryBuilder = QueryBuilder.Select("source");
        return new ModuleSource(QueryBuilder, GraphQLClient);
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
public class ModuleSource(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// If the source is a of kind git, the git source representation of it.
    /// </summary>
    public GitModuleSource AsGitSource()
    {
        var queryBuilder = QueryBuilder.Select("asGitSource");
        return new GitModuleSource(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// If the source is of kind local, the local source representation of it.
    /// </summary>
    public LocalModuleSource AsLocalSource()
    {
        var queryBuilder = QueryBuilder.Select("asLocalSource");
        return new LocalModuleSource(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load the source as a module. If this is a local source, the parent directory must have been provided during module source creation
    /// </summary>
    public Module AsModule()
    {
        var queryBuilder = QueryBuilder.Select("asModule");
        return new Module(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// A human readable ref string representation of this module source.
    /// </summary>
    public async Task<string> AsString()
    {
        var queryBuilder = QueryBuilder.Select("asString");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Returns whether the module source has a configuration file.
    /// </summary>
    public async Task<bool> ConfigExists()
    {
        var queryBuilder = QueryBuilder.Select("configExists");
        return await Engine.Execute<bool>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The directory containing everything needed to load load and use the module.
    /// </summary>
    public Directory ContextDirectory()
    {
        var queryBuilder = QueryBuilder.Select("contextDirectory");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// The dependencies of the module source. Includes dependencies from the configuration and any extras from withDependencies calls.
    /// </summary>
    public async Task<ModuleDependency[]> Dependencies()
    {
        var queryBuilder = QueryBuilder.Select("dependencies");
        return await Engine.Execute<ModuleDependency[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The directory containing the module configuration and source code (source code may be in a subdir).
    /// </summary>
    public Directory Directory(string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("directory");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// A unique identifier for this ModuleSource.
    /// </summary>
    public async Task<ModuleSourceID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<ModuleSourceID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The kind of source (e.g. local, git, etc.)
    /// </summary>
    public async Task<ModuleSourceKind> Kind()
    {
        var queryBuilder = QueryBuilder.Select("kind");
        return await Engine.Execute<ModuleSourceKind>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// If set, the name of the module this source references, including any overrides at runtime by callers.
    /// </summary>
    public async Task<string> ModuleName()
    {
        var queryBuilder = QueryBuilder.Select("moduleName");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The original name of the module this source references, as defined in the module configuration.
    /// </summary>
    public async Task<string> ModuleOriginalName()
    {
        var queryBuilder = QueryBuilder.Select("moduleOriginalName");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The path to the module source's context directory on the caller's filesystem. Only valid for local sources.
    /// </summary>
    public async Task<string> ResolveContextPathFromCaller()
    {
        var queryBuilder = QueryBuilder.Select("resolveContextPathFromCaller");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Resolve the provided module source arg as a dependency relative to this module source.
    /// </summary>
    public ModuleSource ResolveDependency(ModuleSourceID dep)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("resolveDependency");
        return new ModuleSource(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a directory from the caller optionally with a given view applied.
    /// </summary>
    public Directory ResolveDirectoryFromCaller(string path, string? viewName = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("resolveDirectoryFromCaller");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load the source from its path on the caller's filesystem, including only needed+configured files and directories. Only valid for local sources.
    /// </summary>
    public ModuleSource ResolveFromCaller()
    {
        var queryBuilder = QueryBuilder.Select("resolveFromCaller");
        return new ModuleSource(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// The path relative to context of the root of the module source, which contains dagger.json. It also contains the module implementation source code, but that may or may not being a subdir of this root.
    /// </summary>
    public async Task<string> SourceRootSubpath()
    {
        var queryBuilder = QueryBuilder.Select("sourceRootSubpath");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The path relative to context of the module implementation source code.
    /// </summary>
    public async Task<string> SourceSubpath()
    {
        var queryBuilder = QueryBuilder.Select("sourceSubpath");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieve a named view defined for this module source.
    /// </summary>
    public ModuleSourceView View(string name)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("view");
        return new ModuleSourceView(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// The named views defined for this module source, which are sets of directory filters that can be applied to directory arguments provided to functions.
    /// </summary>
    public async Task<ModuleSourceView[]> Views()
    {
        var queryBuilder = QueryBuilder.Select("views");
        return await Engine.Execute<ModuleSourceView[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Update the module source with a new context directory. Only valid for local sources.
    /// </summary>
    public ModuleSource WithContextDirectory(DirectoryID dir)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withContextDirectory");
        return new ModuleSource(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Append the provided dependencies to the module source's dependency list.
    /// </summary>
    public ModuleSource WithDependencies(ModuleDependencyID[] dependencies)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withDependencies");
        return new ModuleSource(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Update the module source with a new name.
    /// </summary>
    public ModuleSource WithName(string name)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withName");
        return new ModuleSource(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Update the module source with a new SDK.
    /// </summary>
    public ModuleSource WithSDK(string sdk)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withSDK");
        return new ModuleSource(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Update the module source with a new source subpath.
    /// </summary>
    public ModuleSource WithSourceSubpath(string path)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withSourceSubpath");
        return new ModuleSource(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Update the module source with a new named view.
    /// </summary>
    public ModuleSource WithView(string name, string[] patterns)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withView");
        return new ModuleSource(QueryBuilder, GraphQLClient);
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
public class ModuleSourceView(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// A unique identifier for this ModuleSourceView.
    /// </summary>
    public async Task<ModuleSourceViewID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<ModuleSourceViewID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The name of the view
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The patterns of the view used to filter paths
    /// </summary>
    public async Task<string[]> Patterns()
    {
        var queryBuilder = QueryBuilder.Select("patterns");
        return await Engine.Execute<string[]>(GraphQLClient, QueryBuilder);
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
public class ObjectTypeDef(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// The function used to construct new instances of this object, if any
    /// </summary>
    public Function Constructor()
    {
        var queryBuilder = QueryBuilder.Select("constructor");
        return new Function(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// The doc string for the object, if any.
    /// </summary>
    public async Task<string> Description()
    {
        var queryBuilder = QueryBuilder.Select("description");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Static fields defined on this object, if any.
    /// </summary>
    public async Task<FieldTypeDef[]> Fields()
    {
        var queryBuilder = QueryBuilder.Select("fields");
        return await Engine.Execute<FieldTypeDef[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Functions defined on this object, if any.
    /// </summary>
    public async Task<Function[]> Functions()
    {
        var queryBuilder = QueryBuilder.Select("functions");
        return await Engine.Execute<Function[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// A unique identifier for this ObjectTypeDef.
    /// </summary>
    public async Task<ObjectTypeDefID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<ObjectTypeDefID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The name of the object.
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// If this ObjectTypeDef is associated with a Module, the name of the module. Unset otherwise.
    /// </summary>
    public async Task<string> SourceModuleName()
    {
        var queryBuilder = QueryBuilder.Select("sourceModuleName");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
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
public class Port(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// The port description.
    /// </summary>
    public async Task<string> Description()
    {
        var queryBuilder = QueryBuilder.Select("description");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Skip the health check when run as a service.
    /// </summary>
    public async Task<bool> ExperimentalSkipHealthcheck()
    {
        var queryBuilder = QueryBuilder.Select("experimentalSkipHealthcheck");
        return await Engine.Execute<bool>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// A unique identifier for this Port.
    /// </summary>
    public async Task<PortID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<PortID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The port number.
    /// </summary>
    public async Task<int> Port_()
    {
        var queryBuilder = QueryBuilder.Select("port");
        return await Engine.Execute<int>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The transport layer protocol.
    /// </summary>
    public async Task<NetworkProtocol> Protocol()
    {
        var queryBuilder = QueryBuilder.Select("protocol");
        return await Engine.Execute<NetworkProtocol>(GraphQLClient, QueryBuilder);
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
public class Query(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// Retrieves a content-addressed blob.
    /// </summary>
    public Directory Blob(string digest, string mediaType, int size, string uncompressed)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("blob");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Retrieves a container builtin to the engine.
    /// </summary>
    public Container BuiltinContainer(string digest)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("builtinContainer");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Constructs a cache volume for a given cache key.
    /// </summary>
    public CacheVolume CacheVolume(string key)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("cacheVolume");
        return new CacheVolume(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Checks if the current Dagger Engine is compatible with an SDK's required version.
    /// </summary>
    public async Task<bool> CheckVersionCompatibility(string version)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("checkVersionCompatibility");
        return await Engine.Execute<bool>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Creates a scratch container.
    /// 
    /// Optional platform argument initializes new containers to execute and publish as that platform. Platform defaults to that of the builder's host.
    /// </summary>
    public Container Container(ContainerID? id = null, Platform? platform = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("container");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// The FunctionCall context that the SDK caller is currently executing in.
    /// 
    /// If the caller is not currently executing in a function, this will return an error.
    /// </summary>
    public FunctionCall CurrentFunctionCall()
    {
        var queryBuilder = QueryBuilder.Select("currentFunctionCall");
        return new FunctionCall(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// The module currently being served in the session, if any.
    /// </summary>
    public CurrentModule CurrentModule()
    {
        var queryBuilder = QueryBuilder.Select("currentModule");
        return new CurrentModule(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// The TypeDef representations of the objects currently being served in the session.
    /// </summary>
    public async Task<TypeDef[]> CurrentTypeDefs()
    {
        var queryBuilder = QueryBuilder.Select("currentTypeDefs");
        return await Engine.Execute<TypeDef[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The default platform of the engine.
    /// </summary>
    public async Task<Platform> DefaultPlatform()
    {
        var queryBuilder = QueryBuilder.Select("defaultPlatform");
        return await Engine.Execute<Platform>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Creates an empty directory.
    /// </summary>
    public Directory Directory(DirectoryID? id = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("directory");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// 
    /// </summary>
    public File File(FileID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("file");
        return new File(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Creates a function.
    /// </summary>
    public Function Function(string name, TypeDefID returnType)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("function");
        return new Function(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Create a code generation result, given a directory containing the generated code.
    /// </summary>
    public GeneratedCode GeneratedCode(DirectoryID code)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("generatedCode");
        return new GeneratedCode(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Queries a Git repository.
    /// </summary>
    public GitRepository Git(string url, ServiceID? experimentalServiceHost = null, bool keepGitDir = false, SocketID? sshAuthSocket = null, string sshKnownHosts = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("git");
        return new GitRepository(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Queries the host environment.
    /// </summary>
    public Host Host()
    {
        var queryBuilder = QueryBuilder.Select("host");
        return new Host(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Returns a file containing an http remote url content.
    /// </summary>
    public File Http(string url, ServiceID? experimentalServiceHost = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("http");
        return new File(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a CacheVolume from its ID.
    /// </summary>
    public CacheVolume LoadCacheVolumeFromID(CacheVolumeID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadCacheVolumeFromID");
        return new CacheVolume(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a Container from its ID.
    /// </summary>
    public Container LoadContainerFromID(ContainerID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadContainerFromID");
        return new Container(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a CurrentModule from its ID.
    /// </summary>
    public CurrentModule LoadCurrentModuleFromID(CurrentModuleID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadCurrentModuleFromID");
        return new CurrentModule(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a Directory from its ID.
    /// </summary>
    public Directory LoadDirectoryFromID(DirectoryID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadDirectoryFromID");
        return new Directory(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a EnvVariable from its ID.
    /// </summary>
    public EnvVariable LoadEnvVariableFromID(EnvVariableID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadEnvVariableFromID");
        return new EnvVariable(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a FieldTypeDef from its ID.
    /// </summary>
    public FieldTypeDef LoadFieldTypeDefFromID(FieldTypeDefID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadFieldTypeDefFromID");
        return new FieldTypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a File from its ID.
    /// </summary>
    public File LoadFileFromID(FileID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadFileFromID");
        return new File(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a FunctionArg from its ID.
    /// </summary>
    public FunctionArg LoadFunctionArgFromID(FunctionArgID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadFunctionArgFromID");
        return new FunctionArg(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a FunctionCallArgValue from its ID.
    /// </summary>
    public FunctionCallArgValue LoadFunctionCallArgValueFromID(FunctionCallArgValueID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadFunctionCallArgValueFromID");
        return new FunctionCallArgValue(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a FunctionCall from its ID.
    /// </summary>
    public FunctionCall LoadFunctionCallFromID(FunctionCallID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadFunctionCallFromID");
        return new FunctionCall(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a Function from its ID.
    /// </summary>
    public Function LoadFunctionFromID(FunctionID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadFunctionFromID");
        return new Function(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a GeneratedCode from its ID.
    /// </summary>
    public GeneratedCode LoadGeneratedCodeFromID(GeneratedCodeID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadGeneratedCodeFromID");
        return new GeneratedCode(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a GitModuleSource from its ID.
    /// </summary>
    public GitModuleSource LoadGitModuleSourceFromID(GitModuleSourceID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadGitModuleSourceFromID");
        return new GitModuleSource(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a GitRef from its ID.
    /// </summary>
    public GitRef LoadGitRefFromID(GitRefID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadGitRefFromID");
        return new GitRef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a GitRepository from its ID.
    /// </summary>
    public GitRepository LoadGitRepositoryFromID(GitRepositoryID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadGitRepositoryFromID");
        return new GitRepository(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a Host from its ID.
    /// </summary>
    public Host LoadHostFromID(HostID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadHostFromID");
        return new Host(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a InputTypeDef from its ID.
    /// </summary>
    public InputTypeDef LoadInputTypeDefFromID(InputTypeDefID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadInputTypeDefFromID");
        return new InputTypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a InterfaceTypeDef from its ID.
    /// </summary>
    public InterfaceTypeDef LoadInterfaceTypeDefFromID(InterfaceTypeDefID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadInterfaceTypeDefFromID");
        return new InterfaceTypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a Label from its ID.
    /// </summary>
    public Label LoadLabelFromID(LabelID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadLabelFromID");
        return new Label(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a ListTypeDef from its ID.
    /// </summary>
    public ListTypeDef LoadListTypeDefFromID(ListTypeDefID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadListTypeDefFromID");
        return new ListTypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a LocalModuleSource from its ID.
    /// </summary>
    public LocalModuleSource LoadLocalModuleSourceFromID(LocalModuleSourceID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadLocalModuleSourceFromID");
        return new LocalModuleSource(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a ModuleDependency from its ID.
    /// </summary>
    public ModuleDependency LoadModuleDependencyFromID(ModuleDependencyID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadModuleDependencyFromID");
        return new ModuleDependency(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a Module from its ID.
    /// </summary>
    public Module LoadModuleFromID(ModuleID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadModuleFromID");
        return new Module(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a ModuleSource from its ID.
    /// </summary>
    public ModuleSource LoadModuleSourceFromID(ModuleSourceID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadModuleSourceFromID");
        return new ModuleSource(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a ModuleSourceView from its ID.
    /// </summary>
    public ModuleSourceView LoadModuleSourceViewFromID(ModuleSourceViewID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadModuleSourceViewFromID");
        return new ModuleSourceView(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a ObjectTypeDef from its ID.
    /// </summary>
    public ObjectTypeDef LoadObjectTypeDefFromID(ObjectTypeDefID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadObjectTypeDefFromID");
        return new ObjectTypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a Port from its ID.
    /// </summary>
    public Port LoadPortFromID(PortID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadPortFromID");
        return new Port(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a Secret from its ID.
    /// </summary>
    public Secret LoadSecretFromID(SecretID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadSecretFromID");
        return new Secret(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a Service from its ID.
    /// </summary>
    public Service LoadServiceFromID(ServiceID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadServiceFromID");
        return new Service(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a Socket from its ID.
    /// </summary>
    public Socket LoadSocketFromID(SocketID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadSocketFromID");
        return new Socket(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a Terminal from its ID.
    /// </summary>
    public Terminal LoadTerminalFromID(TerminalID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadTerminalFromID");
        return new Terminal(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Load a TypeDef from its ID.
    /// </summary>
    public TypeDef LoadTypeDefFromID(TypeDefID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("loadTypeDefFromID");
        return new TypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Create a new module.
    /// </summary>
    public Module Module()
    {
        var queryBuilder = QueryBuilder.Select("module");
        return new Module(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Create a new module dependency configuration from a module source and name
    /// </summary>
    public ModuleDependency ModuleDependency(ModuleSourceID source, string name = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("moduleDependency");
        return new ModuleDependency(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Create a new module source instance from a source ref string.
    /// </summary>
    public ModuleSource ModuleSource(string refString, bool stable = false)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("moduleSource");
        return new ModuleSource(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Creates a named sub-pipeline.
    /// </summary>
    public Query Pipeline(string name, string description = "", PipelineLabel[]? labels = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("pipeline");
        return new Query(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Reference a secret by name.
    /// </summary>
    public Secret Secret(string name, string? accessor = null)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("secret");
        return new Secret(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Sets a secret given a user defined name to its plaintext and returns the secret.
    /// 
    /// The plaintext value is limited to a size of 128000 bytes.
    /// </summary>
    public Secret SetSecret(string name, string plaintext)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("setSecret");
        return new Secret(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Loads a socket by its ID.
    /// </summary>
    public Socket Socket(SocketID id)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("socket");
        return new Socket(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Create a new TypeDef.
    /// </summary>
    public TypeDef TypeDef()
    {
        var queryBuilder = QueryBuilder.Select("typeDef");
        return new TypeDef(QueryBuilder, GraphQLClient);
    }
}

/// <summary>
/// A reference to a secret value, which can be handled more safely than the value itself.
/// </summary>
public class Secret(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// A unique identifier for this Secret.
    /// </summary>
    public async Task<SecretID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<SecretID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The name of this secret.
    /// </summary>
    public async Task<string> Name()
    {
        var queryBuilder = QueryBuilder.Select("name");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The value of this secret.
    /// </summary>
    public async Task<string> Plaintext()
    {
        var queryBuilder = QueryBuilder.Select("plaintext");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
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
public class Service(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// Retrieves an endpoint that clients can use to reach this container.
    /// 
    /// If no port is specified, the first exposed port is used. If none exist an error is returned.
    /// 
    /// If a scheme is specified, a URL is returned. Otherwise, a host:port pair is returned.
    /// </summary>
    public async Task<string> Endpoint(int? port = null, string scheme = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("endpoint");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves a hostname which can be used by clients to reach this container.
    /// </summary>
    public async Task<string> Hostname()
    {
        var queryBuilder = QueryBuilder.Select("hostname");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// A unique identifier for this Service.
    /// </summary>
    public async Task<ServiceID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<ServiceID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Retrieves the list of ports provided by the service.
    /// </summary>
    public async Task<Port[]> Ports()
    {
        var queryBuilder = QueryBuilder.Select("ports");
        return await Engine.Execute<Port[]>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Start the service and wait for its health checks to succeed.
    /// 
    /// Services bound to a Container do not need to be manually started.
    /// </summary>
    public async Task<ServiceID> Start()
    {
        var queryBuilder = QueryBuilder.Select("start");
        return await Engine.Execute<ServiceID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Stop the service.
    /// </summary>
    public async Task<ServiceID> Stop(bool kill = false)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("stop");
        return await Engine.Execute<ServiceID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Creates a tunnel that forwards traffic from the caller's network to this service.
    /// </summary>
    public async Task<Void> Up(PortForward[] ports = null, bool random = false)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("up");
        return await Engine.Execute<Void>(GraphQLClient, QueryBuilder);
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
public class Socket(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// A unique identifier for this Socket.
    /// </summary>
    public async Task<SocketID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<SocketID>(GraphQLClient, QueryBuilder);
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
public class Terminal(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// A unique identifier for this Terminal.
    /// </summary>
    public async Task<TerminalID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<TerminalID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// An http endpoint at which this terminal can be connected to over a websocket.
    /// </summary>
    public async Task<string> WebsocketEndpoint()
    {
        var queryBuilder = QueryBuilder.Select("websocketEndpoint");
        return await Engine.Execute<string>(GraphQLClient, QueryBuilder);
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
public class TypeDef(QueryBuilder queryBuilder, GraphQLClient gqlClient) : Object(queryBuilder, gqlClient)
{
    /// <summary>
    /// If kind is INPUT, the input-specific type definition. If kind is not INPUT, this will be null.
    /// </summary>
    public InputTypeDef AsInput()
    {
        var queryBuilder = QueryBuilder.Select("asInput");
        return new InputTypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// If kind is INTERFACE, the interface-specific type definition. If kind is not INTERFACE, this will be null.
    /// </summary>
    public InterfaceTypeDef AsInterface()
    {
        var queryBuilder = QueryBuilder.Select("asInterface");
        return new InterfaceTypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// If kind is LIST, the list-specific type definition. If kind is not LIST, this will be null.
    /// </summary>
    public ListTypeDef AsList()
    {
        var queryBuilder = QueryBuilder.Select("asList");
        return new ListTypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// If kind is OBJECT, the object-specific type definition. If kind is not OBJECT, this will be null.
    /// </summary>
    public ObjectTypeDef AsObject()
    {
        var queryBuilder = QueryBuilder.Select("asObject");
        return new ObjectTypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// A unique identifier for this TypeDef.
    /// </summary>
    public async Task<TypeDefID> Id()
    {
        var queryBuilder = QueryBuilder.Select("id");
        return await Engine.Execute<TypeDefID>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// The kind of type this is (e.g. primitive, list, object).
    /// </summary>
    public async Task<TypeDefKind> Kind()
    {
        var queryBuilder = QueryBuilder.Select("kind");
        return await Engine.Execute<TypeDefKind>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Whether this type can be set to null. Defaults to false.
    /// </summary>
    public async Task<bool> Optional()
    {
        var queryBuilder = QueryBuilder.Select("optional");
        return await Engine.Execute<bool>(GraphQLClient, QueryBuilder);
    }

    /// <summary>
    /// Adds a function for constructing a new instance of an Object TypeDef, failing if the type is not an object.
    /// </summary>
    public TypeDef WithConstructor(FunctionID function)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withConstructor");
        return new TypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Adds a static field for an Object TypeDef, failing if the type is not an object.
    /// </summary>
    public TypeDef WithField(string name, TypeDefID typeDef, string description = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withField");
        return new TypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Adds a function for an Object or Interface TypeDef, failing if the type is not one of those kinds.
    /// </summary>
    public TypeDef WithFunction(FunctionID function)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withFunction");
        return new TypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Returns a TypeDef of kind Interface with the provided name.
    /// </summary>
    public TypeDef WithInterface(string name, string description = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withInterface");
        return new TypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Sets the kind of the type.
    /// </summary>
    public TypeDef WithKind(TypeDefKind kind)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withKind");
        return new TypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Returns a TypeDef of kind List with the provided type for its elements.
    /// </summary>
    public TypeDef WithListOf(TypeDefID elementType)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withListOf");
        return new TypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Returns a TypeDef of kind Object with the provided name.
    /// 
    /// Note that an object's fields and functions may be omitted if the intent is only to refer to an object. This is how functions are able to return their own object, or any other circular reference.
    /// </summary>
    public TypeDef WithObject(string name, string description = "")
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withObject");
        return new TypeDef(QueryBuilder, GraphQLClient);
    }

    /// <summary>
    /// Sets whether this type can be set to null.
    /// </summary>
    public TypeDef WithOptional(bool optional)
    {
        var arguments = ImmutableList<Argument>.Empty;
        var queryBuilder = QueryBuilder.Select("withOptional");
        return new TypeDef(QueryBuilder, GraphQLClient);
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
