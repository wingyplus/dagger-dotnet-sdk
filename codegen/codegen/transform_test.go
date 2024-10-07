package codegen

import (
	"testing"

	"github.com/stretchr/testify/require"
	"google.golang.org/protobuf/proto"

	codegenpb "dagger/codegen/proto/codegen"
)

func TestEnum(t *testing.T) {
	t.Parallel()

	introspectionJson := `
{
  "__schema": {
	"types": [
	  {
	    "description": "Sharing mode of the cache volume.",
	    "enumValues": [
	      {
	    	  "deprecationReason": null,
	    	  "description": "Shares the cache volume amongst many build pipelines",
	    	  "isDeprecated": false,
	    	  "name": "SHARED"
	      },
	      {
	    	  "deprecationReason": null,
	    	  "description": "Keeps a cache volume for a single build pipeline",
	    	  "isDeprecated": false,
	    	  "name": "PRIVATE"
	      },
	      {
	    	  "deprecationReason": null,
	    	  "description": "Shares the cache volume amongst many build pipelines, but will serialize the writes",
	    	  "isDeprecated": false,
	    	  "name": "LOCKED"
	      }
	    ],
	    "fields": [],
	    "inputFields": [],
	    "interfaces": [],
	    "kind": "ENUM",
	    "name": "CacheSharingMode",
	    "possibleTypes": []
	  }
	]
  }
}
`
	pb, err := TransformJson(introspectionJson)
	require.NoError(t, err)

	var schema codegenpb.Schema
	require.NoError(t, proto.Unmarshal(pb, &schema))

	require.Len(t, schema.Types, 1)

	enum := schema.Types[0].Value.(*codegenpb.Type_Enum).Enum
	require.Equal(t, "CacheSharingMode", enum.Name)
	require.Equal(t, "Sharing mode of the cache volume.", enum.Description)

	require.Len(t, enum.Values, 3)

	require.Equal(t, &codegenpb.EnumValue{
		Name:        "SHARED",
		Description: "Shares the cache volume amongst many build pipelines",
		Deprecated:  nil,
	}, enum.Values[0])

}

func TestScalar(t *testing.T) {
	t.Parallel()

	introspectionJson := `
{
  "__schema": {
	"types": [
	  {
        "description": "The 'ContainerID' scalar type represents an identifier for an object of type Container.",
        "enumValues": [],
        "fields": [],
        "inputFields": [],
        "interfaces": [],
        "kind": "SCALAR",
        "name": "ContainerID",
        "possibleTypes": []
      }
	]
  }
}
`

	pb, err := TransformJson(introspectionJson)
	require.NoError(t, err)

	var schema codegenpb.Schema
	require.NoError(t, proto.Unmarshal(pb, &schema))

	scalar := schema.Types[0].Value.(*codegenpb.Type_Scalar).Scalar
	require.Equal(t, "ContainerID", scalar.Name)
	require.Equal(t, "The 'ContainerID' scalar type represents an identifier for an object of type Container.", scalar.Description)
}

func TestInput(t *testing.T) {
	t.Parallel()

	introspectionJson := `
{
  "__schema": {
	"types": [
	  {
        "description": "Key value object that represents a build argument.",
        "enumValues": [],
        "fields": [],
        "inputFields": [
          {
            "defaultValue": null,
            "description": "The build argument name.",
            "name": "name",
            "type": {
              "kind": "NON_NULL",
              "name": null,
              "ofType": {
                "kind": "SCALAR",
                "name": "String",
                "ofType": null
              }
            }
          },
          {
            "defaultValue": null,
            "description": "The build argument value.",
            "name": "value",
            "type": {
              "kind": "NON_NULL",
              "name": null,
              "ofType": {
                "kind": "SCALAR",
                "name": "String",
                "ofType": null
              }
            }
          }
        ],
        "interfaces": [],
        "kind": "INPUT_OBJECT",
        "name": "BuildArg",
        "possibleTypes": []
      }
	]
  }
}
`

	pb, err := TransformJson(introspectionJson)
	require.NoError(t, err)

	var schema codegenpb.Schema
	require.NoError(t, proto.Unmarshal(pb, &schema))

	input := schema.Types[0].Value.(*codegenpb.Type_Input).Input
	require.Equal(t, "BuildArg", input.Name)
	require.Equal(t, "Key value object that represents a build argument.", input.Description)

	require.Len(t, input.Fields, 2)

	require.Equal(t, &codegenpb.Field{
		Name:         "name",
		Description:  "The build argument name.",
		DefaultValue: nil,
		Type: &codegenpb.TypeRef{
			Nullable: false,
			Type: &codegenpb.TypeRef_ScalarType{
				ScalarType: &codegenpb.ScalarTypeRef{Name: "String"},
			},
		},
	}, input.Fields[0])
}

func TestObject(t *testing.T) {
	t.Parallel()

	introspectionJson := `
{
  "__schema": {
	"types": [
	  {
        "description": "A directory whose contents persist across runs.",
        "enumValues": [],
        "fields": [
          {
            "args": [],
            "deprecationReason": null,
            "description": "A unique identifier for this CacheVolume.",
            "isDeprecated": false,
            "name": "id",
            "type": {
              "kind": "NON_NULL",
              "name": null,
              "ofType": {
                "kind": "SCALAR",
                "name": "CacheVolumeID",
                "ofType": null
              }
            }
          }
        ],
        "inputFields": [],
        "interfaces": [],
        "kind": "OBJECT",
        "name": "CacheVolume",
        "possibleTypes": []
      }
	]
  }
}
`

	pb, err := TransformJson(introspectionJson)
	require.NoError(t, err)

	var schema codegenpb.Schema
	require.NoError(t, proto.Unmarshal(pb, &schema))

	object := schema.Types[0].Value.(*codegenpb.Type_Object).Object
	require.Equal(t, "CacheVolume", object.Name)
	require.Equal(t, "A directory whose contents persist across runs.", object.Description)

	require.Len(t, object.Functions, 1)

	require.Equal(t, &codegenpb.Function{
		Name:        "id",
		Description: "A unique identifier for this CacheVolume.",
		ReturnType: &codegenpb.TypeRef{
			Nullable: false,
			Type: &codegenpb.TypeRef_ScalarType{
				ScalarType: &codegenpb.ScalarTypeRef{Name: "CacheVolumeID"},
			},
		},
	}, object.Functions[0])
}

func TestObject_QueryRenameToClient(t *testing.T) {
	t.Parallel()

	introspectionJson := `
{
  "__schema": {
	"types": [
	  {
        "description": "The root of the DAG.",
        "enumValues": [],
        "fields": [
          {
            "args": [
              {
                "defaultValue": null,
                "description": "Name of the sub-pipeline.",
                "name": "name",
                "type": {
                  "kind": "NON_NULL",
                  "name": null,
                  "ofType": {
                    "kind": "SCALAR",
                    "name": "String",
                    "ofType": null
                  }
                }
              },
              {
                "defaultValue": "\"\"",
                "description": "Description of the sub-pipeline.",
                "name": "description",
                "type": {
                  "kind": "SCALAR",
                  "name": "String",
                  "ofType": null
                }
              },
              {
                "defaultValue": null,
                "description": "Labels to apply to the sub-pipeline.",
                "name": "labels",
                "type": {
                  "kind": "LIST",
                  "name": null,
                  "ofType": {
                    "kind": "NON_NULL",
                    "name": null,
                    "ofType": {
                      "kind": "INPUT_OBJECT",
                      "name": "PipelineLabel",
                      "ofType": null
                    }
                  }
                }
              }
            ],
            "deprecationReason": null,
            "description": "Creates a named sub-pipeline.",
            "isDeprecated": false,
            "name": "pipeline",
            "type": {
              "kind": "NON_NULL",
              "name": null,
              "ofType": {
                "kind": "OBJECT",
                "name": "Query",
                "ofType": null
              }
            }
          }
        ],
        "inputFields": [],
        "interfaces": [],
        "kind": "OBJECT",
        "name": "Query",
        "possibleTypes": []
      }
	]
  }
}
`

	pb, err := TransformJson(introspectionJson)
	require.NoError(t, err)

	var schema codegenpb.Schema
	require.NoError(t, proto.Unmarshal(pb, &schema))

	object := schema.Types[0].Value.(*codegenpb.Type_Object).Object
	require.Equal(t, "Client", object.Name)

	require.Len(t, object.Functions, 1)

	fn := object.Functions[0]
	require.Equal(t, "Client", fn.ReturnType.Type.(*codegenpb.TypeRef_ObjectType).ObjectType.Name)
}
