// The DotNet SDK and runtime.
package main

import (
	_ "embed"
)

//go:embed introspection.graphql
var introspectionGraphql string

const introspectionJsonPath = "/introspection.json"

type DotnetSdk struct{}

// Fetch introspection json from the Engine.
//
// This function forked from https://github.com/helderco/daggerverse/blob/main/codegen/main.go but
// didn't modify anything in the data.
//
// It's uses for test for the codegen only.
func (m *DotnetSdk) Introspect() *File {
	return dag.Docker().
		Cli(DockerCliOpts{
			Version: "26",
			Engine:  dag.Docker().Engine(DockerEngineOpts{Version: "26"}),
		}).
		Container().
		WithExec([]string{"apk", "add", "--no-cache", "curl"}).
		WithExec([]string{"sh", "-c", "curl -L https://dl.dagger.io/dagger/install.sh | BIN_DIR=/usr/local/bin sh"}).
		WithNewFile("/introspection.graphql", ContainerWithNewFileOpts{Contents: introspectionGraphql}).
		WithExec([]string{"sh", "-c", "dagger query < /introspection.graphql > " + introspectionJsonPath}, ContainerWithExecOpts{
			ExperimentalPrivilegedNesting: true,
		}).
		File(introspectionJsonPath)
}
