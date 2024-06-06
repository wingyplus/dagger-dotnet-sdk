// The DotNet SDK and runtime.
package main

import (
	"context"
	_ "embed"
)

//go:embed introspection.graphql
var introspectionGraphql string

const introspectionJsonPath = "/introspection.json"

type DotnetSdk struct {
}

// Fetch introspection json from the Engine.
//
// This function forked from https://github.com/helderco/daggerverse/blob/main/codegen/main.go but
// didn't modify anything in the data.
//
// It's uses for test for the codegen only.
func (m *DotnetSdk) Introspect() *File {
	return m.Dagger().
		WithNewFile("/introspection.graphql", ContainerWithNewFileOpts{Contents: introspectionGraphql}).
		WithExec([]string{"sh", "-c", "dagger query < /introspection.graphql > " + introspectionJsonPath}, ContainerWithExecOpts{
			ExperimentalPrivilegedNesting: true,
		}).
		File(introspectionJsonPath)
}

func (m *DotnetSdk) Dagger() *Container {

	return dag.Docker().
		Cli(DockerCliOpts{
			Version: "26",
			Engine:  dag.Docker().Engine(DockerEngineOpts{Version: "26"}),
		}).
		Container().
		WithExec([]string{"apk", "add", "--no-cache", "curl"}).
		WithExec([]string{"sh", "-c", "curl -L https://dl.dagger.io/dagger/install.sh | BIN_DIR=/usr/local/bin sh"})
}

// Run the dotnet test command.
func (m *DotnetSdk) Dotnet(ctx context.Context) *Container {

	file := m.Introspect()

	return dag.
		Container().
		From("mcr.microsoft.com/dotnet/sdk:8.0").
		WithDirectory("/app", dag.CurrentModule().Source().Directory("sdk")).
		WithFile("/app/Dagger.SDK/introspection.json", file).
		WithWorkdir("/app")
}
