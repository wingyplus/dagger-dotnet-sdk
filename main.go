// The DotNet SDK and runtime.
package main

import (
	"context"
	"strings"
)

type DotnetSdk struct{}

// Generate C# code.
//
// Returns a generated C# file.
func (m *DotnetSdk) GenerateCode(ctx context.Context) (string, error) {
	introspection := dag.Codegen().Introspect()

	builder := &strings.Builder{}

	enums, err := introspection.Enums(ctx)
	if err != nil {
		return "", err
	}

	enumRenderer := &EnumRenderer{}
	if err := enumRenderer.RenderMany(ctx, builder, enums); err != nil {
		return "", err
	}

	inputs, err := introspection.Inputs(ctx)
	if err != nil {
		return "", err
	}

	inputRenderer := &InputValueRenderer{}
	if err := inputRenderer.RenderMany(ctx, builder, inputs); err != nil {
		return "", err
	}

	return builder.String(), nil
}
