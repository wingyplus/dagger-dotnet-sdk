using System.Collections.Immutable;
using System.Text.Json.Serialization;
using Dagger.SDK.GraphQL;

namespace Dagger.SDK;

public class BaseClient(QueryBuilder queryBuilder, GraphQLClient gqlClient) {
    public QueryBuilder QueryBuilder { get; } = queryBuilder;
    public GraphQLClient GraphQLClient { get; } = gqlClient;
}

/// <summary>
/// Sharing mode of the cache volume.
/// </summary>
public enum CacheSharingMode {
    /// <summary>
    /// Shares the cache volume amongst many build pipelines
    /// </summary>
    SHARED,
    /// <summary>
    /// Keeps a cache volume for a single build pipeline
    /// </summary>
    PRIVATE,
    /// <summary>
    /// Shares the cache volume amongst many build pipelines, but will serialize the writes
    /// </summary>
    LOCKED,

}

/// <summary>
/// Compression algorithm to use for image layers.
/// </summary>
public enum ImageLayerCompression {
    /// <summary>
    /// 
    /// </summary>
    Gzip,
    /// <summary>
    /// 
    /// </summary>
    Zstd,
    /// <summary>
    /// 
    /// </summary>
    EStarGZ,
    /// <summary>
    /// 
    /// </summary>
    Uncompressed,

}

/// <summary>
/// Mediatypes to use in published or exported image metadata.
/// </summary>
public enum ImageMediaTypes {
    /// <summary>
    /// 
    /// </summary>
    OCIMediaTypes,
    /// <summary>
    /// 
    /// </summary>
    DockerMediaTypes,

}

/// <summary>
/// The kind of module source.
/// </summary>
public enum ModuleSourceKind {
    /// <summary>
    /// 
    /// </summary>
    LOCAL_SOURCE,
    /// <summary>
    /// 
    /// </summary>
    GIT_SOURCE,

}

/// <summary>
/// Transport layer network protocol associated to a port.
/// </summary>
public enum NetworkProtocol {
    /// <summary>
    /// 
    /// </summary>
    TCP,
    /// <summary>
    /// 
    /// </summary>
    UDP,

}

/// <summary>
/// Distinguishes the different kinds of TypeDefs.
/// </summary>
public enum TypeDefKind {
    /// <summary>
    /// A string value.
    /// </summary>
    STRING_KIND,
    /// <summary>
    /// An integer value.
    /// </summary>
    INTEGER_KIND,
    /// <summary>
    /// A boolean value.
    /// </summary>
    BOOLEAN_KIND,
    /// <summary>
    /// A scalar value of any basic kind.
    /// </summary>
    SCALAR_KIND,
    /// <summary>
    /// A list of values all having the same type.
    /// 
    /// Always paired with a ListTypeDef.
    /// </summary>
    LIST_KIND,
    /// <summary>
    /// A named type defined in the GraphQL schema, with fields and functions.
    /// 
    /// Always paired with an ObjectTypeDef.
    /// </summary>
    OBJECT_KIND,
    /// <summary>
    /// A named type of functions that can be matched+implemented by other objects+interfaces.
    /// 
    /// Always paired with an InterfaceTypeDef.
    /// </summary>
    INTERFACE_KIND,
    /// <summary>
    /// A graphql input type, used only when representing the core API via TypeDefs.
    /// </summary>
    INPUT_KIND,
    /// <summary>
    /// A special kind used to signify that no value is returned.
    /// 
    /// This is used for functions that have no return value. The outer TypeDef specifying this Kind is always Optional, as the Void is never actually represented.
    /// </summary>
    VOID_KIND,

}

/// <summary>
/// Key value object that represents a build argument.
/// </summary>
public struct BuildArg {
    /// <summary>
    /// The build argument name.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// The build argument value.
    /// </summary>
    public string Value { get; set; }
}

/// <summary>
/// Key value object that represents a pipeline label.
/// </summary>
public struct PipelineLabel {
    /// <summary>
    /// Label name.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Label value.
    /// </summary>
    public string Value { get; set; }
}

/// <summary>
/// Port forwarding rules for tunneling network traffic.
/// </summary>
public struct PortForward {
    /// <summary>
    /// Destination port for traffic.
    /// </summary>
    public int Backend { get; set; }
    /// <summary>
    /// Port to expose to clients. If unspecified, a default will be chosen.
    /// </summary>
    public int? Frontend { get; set; }
    /// <summary>
    /// Transport layer protocol to use for traffic.
    /// </summary>
    public NetworkProtocol? Protocol { get; set; }
}

