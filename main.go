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
	enums, err := introspection.Enums(ctx)
	if err != nil {
		return "", err
	}

	enumRenderer := &EnumRenderer{}
	contents, err := enumRenderer.RenderMany(ctx, enums)
	if err != nil {
		return "", err
	}

	return strings.Join(contents, "\n\n"), nil
}
