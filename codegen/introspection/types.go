package introspection

import "strings"

// rootObjectName is the name of the root object type (i.e., corresponds to Query)
var rootObjectName = "Client"

// response is the introspection query response
type Response struct {
	Schema *Schema `json:"__schema"`
}

// Result of the introspection
type Schema struct {
	// A list of all types supported by this server
	//
	// Probably better to use the functions that return a list of specific
	// types instead, to render them as groups for the client (tip: use
	// following order: Scalars, Enums, Inputs and Objects).
	Types []*Type `json:"types"`
}

// Returns the root object type (Client)
//
// In the API, this corresponds to the "Query" object type.
func (s *Schema) RootType() *Type {
	return s.GetType(rootObjectName)
}

// Get a type by name
func (s *Schema) GetType(
	// The name of the type
	name string,
) *Type {
	for _, i := range s.Types {
		if i.Name == name {
			return i
		}
	}
	return nil
}

// Get a list of all custom ID scalar types
func (s *Schema) IDs() []*Type {
	var scalars []*Type
	for _, t := range s.Types {
		if t.IsID() {
			scalars = append(scalars, t)
		}
	}
	return scalars
}

// Get a list of all non-ID scalar types
func (s *Schema) NonIdScalars() []*Type {
	var scalars []*Type
	for _, t := range s.Types {
		if t.IsScalar() && !t.IsID() {
			scalars = append(scalars, t)
		}
	}
	return scalars
}

// Get a list of all scalar types
func (s *Schema) Scalars() []*Type {
	var scalars []*Type
	for _, t := range s.Types {
		if t.IsScalar() {
			scalars = append(scalars, t)
		}
	}
	return scalars
}

// Get a list of all enum types
func (s *Schema) Enums() []*Type {
	var enums []*Type
	for _, t := range s.Types {
		if t.IsEnum() {
			enums = append(enums, t)
		}
	}
	return enums
}

// Get a list of all input object types
func (s *Schema) Inputs() []*Type {
	var inputs []*Type
	for _, t := range s.Types {
		if t.IsInputObject() {
			inputs = append(inputs, t)
		}
	}
	return inputs
}

// Get a list of all object types
func (s *Schema) Objects() []*Type {
	var objects []*Type
	for _, t := range s.Types {
		if t.IsObject() {
			objects = append(objects, t)
		}
	}
	return objects
}

// Get a list of all interface types
func (s *Schema) Interfaces() []*Type {
	var objects []*Type
	for _, t := range s.Types {
		if t.IsInterface() {
			objects = append(objects, t)
		}
	}
	return objects
}

type TypeKind string

const (
	TypeKindScalar      = TypeKind("SCALAR")
	TypeKindObject      = TypeKind("OBJECT")
	TypeKindInterface   = TypeKind("INTERFACE")
	TypeKindUnion       = TypeKind("UNION")
	TypeKindEnum        = TypeKind("ENUM")
	TypeKindInputObject = TypeKind("INPUT_OBJECT")
	TypeKindList        = TypeKind("LIST")
	TypeKindNonNull     = TypeKind("NON_NULL")
)

type Scalar string

const (
	ScalarInt     = Scalar("Int")
	ScalarFloat   = Scalar("Float")
	ScalarString  = Scalar("String")
	ScalarBoolean = Scalar("Boolean")
)

// A type in the API
type Type struct {
	// The kind of type, e.g., object, input object, scalar, or enum.
	Kind TypeKind `json:"kind"`
	// The name of the type
	Name string `json:"name"`
	// The description of the type
	Description *string `json:"description,omitempty"`
	// An object type's fields
	Fields []*Field `json:"fields,omitempty"`
	// An input object type's fields
	InputFields []InputValue `json:"inputFields,omitempty"`
	// An enum type's values
	EnumValues []EnumValue `json:"enumValues,omitempty"`
}

// Is it an ID scalar?
func (t *Type) IsID() bool {
	return t.IsScalar() && strings.HasSuffix(t.Name, "ID")
}

// Is the type a scalar?
func (t *Type) IsScalar() bool {
	return t.Kind == TypeKindScalar
}

// Is the type an enum?
func (t *Type) IsEnum() bool {
	return t.Kind == TypeKindEnum
}

// Is the type an object?
func (t *Type) IsObject() bool {
	return t.Kind == TypeKindObject
}

// Is the type an input object?
func (t *Type) IsInputObject() bool {
	return t.Kind == TypeKindInputObject
}

// Is it an interface type?
func (t *Type) IsInterface() bool {
	return t.Kind == TypeKindInterface
}

// Is it the root Client type?
func (t *Type) IsRoot() bool {
	return t.Name == rootObjectName
}

// Get an object type's field by name
func (t *Type) GetField(
	// The name of the field
	name string,
) *Field {
	for _, f := range t.Fields {
		if f.Name == name {
			return f
		}
	}
	return nil
}

// Get an object type's fields that return a leaf type
func (t *Type) Leafs() []*Field {
	var leafs []*Field
	for _, f := range t.Fields {
		if f.Type.IsLeaf() {
			leafs = append(leafs, f)
		}
	}
	return leafs
}

// Get an object type's leafs that don't require arguments
func (t *Type) SimpleLeafs(
	// Allow optional arguments?
	// +optional
	// +default=false
	optionals,
	// Allow fields that return an ID?
	// +optional
	// +default=false
	ids bool,
) []*Field {
	var fields []*Field
	for _, f := range t.Leafs() {
		if !ids && f.Type.IsID() {
			continue
		}
		if !f.HasArgs() || optionals && !f.HasRequiredArgs() {
			fields = append(fields, f)
		}
	}
	return fields
}

// An object type's field
type Field struct {
	// The name of the field
	Name string `json:"name"`
	// The description of the field
	Description string `json:"description"`
	// The return type of the field
	Type *TypeRef `json:"type"`
	// The field's arguments
	Args []*InputValue `json:"args"`
	// Is the field deprecated?
	IsDeprecated bool `json:"isDeprecated"`
	// The reason for deprecation, if exists
	DeprecationReason string `json:"deprecationReason"`

	// The name of the field, split into words
	//
	// Common initialisms are preserved, e.g., "ID" becomes "ID" and "URL" becomes "URL".
	// This is useful so SDKs don't have to split the name themselves, before
	// applying the language's naming conventions, which can be a source of
	// inconsistency.
	NameWords string `json:"nameWords,omitempty"`
}

// Does the field have any arguments?
func (f *Field) HasArgs() bool {
	return len(f.Args) > 0
}

// Does the field have required arguments?
func (f *Field) HasRequiredArgs() bool {
	for _, a := range f.Args {
		if !a.Type.IsOptional() {
			return true
		}
	}
	return false
}

// Does the field have optional arguments?
func (f *Field) HasOptionalArgs() bool {
	for _, a := range f.Args {
		if a.Type.IsOptional() {
			return true
		}
	}
	return false
}

// The field's required arguments
func (f *Field) RequiredArgs() []*InputValue {
	var args []*InputValue
	for _, a := range f.Args {
		if !a.Type.IsOptional() {
			args = append(args, a)
		}
	}
	return args
}

// The field's optional arguments
func (f *Field) OptionalArgs() []*InputValue {
	var args []*InputValue
	for _, a := range f.Args {
		if a.Type.IsOptional() {
			args = append(args, a)
		}
	}
	return args
}

// Get a field argument by name
func (f *Field) GetArg(
	// The name of the argument
	name string,
) *InputValue {
	for _, a := range f.Args {
		if a.Name == name {
			return a
		}
	}
	return nil
}

// Type information
//
// Can be the return type of a field, the type of a field argument,
// or the underlying type in a list or non-null type.
type TypeRef struct {
	// The kind of type, e.g., object, scalar, etc.
	Kind TypeKind `json:"kind"`
	// The name of a named type.
	Name string `json:"name,omitempty"`
	// The sub-type of a list or non-null type.
	OfType *TypeRef `json:"ofType,omitempty"`
}

// Is this type optional?
func (r TypeRef) IsOptional() bool {
	return r.Kind != TypeKindNonNull
}

// Is it a leaf?
//
// Leaf types as essentially scalar or enum types. They are the last step
// in a pipeline, so must execute the request and return the result.
func (r TypeRef) IsLeaf() bool {
	ref := r
	if r.Kind == TypeKindNonNull {
		ref = *ref.OfType
	}
	if ref.Kind == TypeKindScalar {
		return true
	}
	if ref.Kind == TypeKindEnum {
		return true
	}
	return false
}

// Is it a scalar?
func (r TypeRef) IsScalar() bool {
	ref := r
	if r.Kind == TypeKindNonNull {
		ref = *ref.OfType
	}
	if ref.Kind == TypeKindScalar {
		return true
	}
	return false
}

// Is it an object type?
func (r TypeRef) IsObject() bool {
	ref := r
	if r.Kind == TypeKindNonNull {
		ref = *ref.OfType
	}
	if ref.Kind == TypeKindObject {
		return true
	}
	return false
}

// Is it a list?
func (r TypeRef) IsList() bool {
	ref := r
	if r.Kind == TypeKindNonNull {
		ref = *ref.OfType
	}
	if ref.Kind == TypeKindList {
		return true
	}
	return false
}

// Is it an ID scalar?
func (r TypeRef) IsID() bool {
	ref := r
	if r.Kind == TypeKindNonNull {
		ref = *ref.OfType
	}
	if ref.Kind != TypeKindScalar {
		return false
	}
	return strings.HasSuffix(ref.Name, "ID")
}

// An argument or input object type's field
type InputValue struct {
	// The name of the input
	Name string `json:"name"`
	// The description of the input
	Description string `json:"description"`
	// The default value of the input
	DefaultValue *string `json:"defaultValue"`
	// The type information of the input
	Type *TypeRef `json:"type"`

	// The name of the input, split into words
	//
	// Common initialisms are preserved, e.g., "ID" becomes "ID" and "URL" becomes "URL".
	// This is useful so SDKs don't have to split the name themselves, before
	// applying the language's naming conventions, which can be a source of
	// inconsistency.
	NameWords string `json:"nameWords,omitempty"`
}

// An enum type's value
type EnumValue struct {
	// The name of the enum value
	Name string `json:"name"`
	// The description of the enum value
	Description string `json:"description"`
	// Is the enum value deprecated?
	IsDeprecated bool `json:"isDeprecated"`
	// The reason for deprecation, if exists
	DeprecationReason string `json:"deprecationReason"`
}
