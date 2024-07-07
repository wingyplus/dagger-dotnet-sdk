// Package codegen takes care about how to transfrom GraphQL introspection into
// Protobuf format.
package codegen

import (
	"google.golang.org/protobuf/proto"

	"dagger/codegen/introspection"
	codegenpb "dagger/codegen/proto/codegen"
)

// Transform introspection JSON into Protobuf format.
func Transform(schema *introspection.Schema) ([]byte, error) {
	pb := doTransform(schema)
	return proto.Marshal(pb)
}

func TransformJson(introspectionJson string) ([]byte, error) {
	schema, err := introspection.FromJson(introspectionJson)
	if err != nil {
		return nil, err
	}
	return Transform(schema)
}

func doTransform(schema *introspection.Schema) *codegenpb.Schema {
	schemapb := &codegenpb.Schema{}

	for _, type_ := range schema.Types {
		schemapb.Types = append(schemapb.Types, transformType(type_))
	}

	return schemapb
}

func transformType(type_ *introspection.Type) *codegenpb.Type {
	switch type_.Kind {
	case introspection.TypeKindScalar:
		return transformScalarType(type_)
	case introspection.TypeKindEnum:
		return transformEnumType(type_)
	case introspection.TypeKindInputObject:
		return transformInputObjectType(type_)
	case introspection.TypeKindObject:
		return transformObjectType(type_)
	}
	panic("Type kind " + type_.Kind + " is not supported")
}

func transformScalarType(type_ *introspection.Type) *codegenpb.Type {
	return &codegenpb.Type{
		Value: &codegenpb.Type_Scalar{
			Scalar: &codegenpb.Scalar{
				Name:        normalizeName(type_.Name),
				Description: *type_.Description,
			},
		},
	}
}

func transformEnumType(type_ *introspection.Type) *codegenpb.Type {
	values := make([]*codegenpb.EnumValue, len(type_.EnumValues))

	for i, ev := range type_.EnumValues {
		var deprecation *codegenpb.Deprecation
		if ev.IsDeprecated {
			deprecation = &codegenpb.Deprecation{
				Reason: ev.DeprecationReason,
			}
		}
		values[i] = &codegenpb.EnumValue{
			Name:        ev.Name,
			Description: ev.Description,
			Deprecated:  deprecation,
		}
	}

	return &codegenpb.Type{
		Value: &codegenpb.Type_Enum{
			Enum: &codegenpb.Enum{
				Name:        normalizeName(type_.Name),
				Description: *type_.Description,
				Values:      values,
			},
		},
	}
}

func transformInputObjectType(type_ *introspection.Type) *codegenpb.Type {
	fields := make([]*codegenpb.Field, len(type_.InputFields))

	for i, iv := range type_.InputFields {
		iv := iv

		fields[i] = &codegenpb.Field{
			Name:         iv.Name,
			Description:  iv.Description,
			DefaultValue: iv.DefaultValue,
			Type:         toTypeRef(iv.Type),
		}
	}

	return &codegenpb.Type{
		Value: &codegenpb.Type_Input{
			Input: &codegenpb.Input{
				Name:        normalizeName(type_.Name),
				Description: *type_.Description,
				Fields:      fields,
			},
		},
	}
}

func transformObjectType(type_ *introspection.Type) *codegenpb.Type {
	functions := make([]*codegenpb.Function, len(type_.Fields))

	for i, f := range type_.Fields {
		f := f

		var deprecation *codegenpb.Deprecation
		if f.IsDeprecated {
			deprecation = &codegenpb.Deprecation{
				Reason: f.DeprecationReason,
			}
		}

		functions[i] = &codegenpb.Function{
			Name:        f.Name,
			Description: f.Description,
			ReturnType:  toTypeRef(f.Type),
			Deprecated:  deprecation,
		}
	}

	return &codegenpb.Type{
		Value: &codegenpb.Type_Object{
			Object: &codegenpb.Object{
				Name:        normalizeName(type_.Name),
				Description: *type_.Description,
				Functions:   functions,
			},
		},
	}
}

func toTypeRef(itr *introspection.TypeRef) (ctr *codegenpb.TypeRef) {
	ctr = &codegenpb.TypeRef{
		Nullable: itr.Kind != introspection.TypeKindNonNull,
	}

	if itr.Kind == introspection.TypeKindNonNull {
		itr = itr.OfType
	}

	name := normalizeName(itr.Name)

	switch itr.Kind {
	case introspection.TypeKindEnum:
		ctr.Type = &codegenpb.TypeRef_EnumType{
			EnumType: &codegenpb.EnumTypeRef{Name: name},
		}
		return

	case introspection.TypeKindObject:
		ctr.Type = &codegenpb.TypeRef_ObjectType{
			ObjectType: &codegenpb.ObjectTypeRef{Name: name},
		}
		return

	case introspection.TypeKindScalar:
		ctr.Type = &codegenpb.TypeRef_ScalarType{
			ScalarType: &codegenpb.ScalarTypeRef{Name: name},
		}
		return

	case introspection.TypeKindList:
		ctr.Type = &codegenpb.TypeRef_ListType{
			ListType: &codegenpb.ListTypeRef{
				OfType: toTypeRef(itr.OfType),
			},
		}
		return
	}

	panic("Type kind " + itr.Kind + " is not supported")
}

func normalizeName(name string) string {
	if name == "Query" {
		return "Client"
	}
	return name
}
