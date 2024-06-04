// The DotNet SDK and runtime.
package main

import (
	"context"
	_ "embed"
)

//go:embed introspection.graphql
var introspectionGraphql string

const introspectionJsonPath = "/introspection.json"

func New(
	// +optional
	sdkSourceDir *Directory,
) *DotnetSdk {
	if sdkSourceDir == nil {
		sdkSourceDir = dag.CurrentModule().Source().Directory("sdk")
	}
	return &DotnetSdk{
		SDKSourceDir:  sdkSourceDir,
		RequiredPaths: []string{},
		Container:     dag.Container(),
	}
}

type DotnetSdk struct {
	SDKSourceDir  *Directory
	RequiredPaths []string
	Container     *Container
}

func (m *DotnetSdk) ModuleRuntime(
	ctx context.Context,
	modSource *ModuleSource,
	introspectionJson string,
) (*Container, error) {
	return nil, nil
}

func (m *DotnetSdk) Codegen(
	ctx context.Context,
	modSource *ModuleSource,
	introspectionJson string,
) (*GeneratedCode, error) {
	return nil, nil
}
