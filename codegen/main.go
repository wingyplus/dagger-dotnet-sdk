// A reusable tool to help construct SDK more faster.

package main

import (
	"context"
	_ "embed"

	"dagger/codegen/codegen"
)

//go:embed introspection.graphql
var introspectionGraphql string

const (
	introspectionJsonPath = "/introspection.json"
	dockerVersion         = "26"
)

type Introspection struct {
	// +private
	F *File
}

// Export raw introspection json.
func (m *Introspection) Json() *File {
	return m.F
}

// Convert introspection JSON file into Protobuf format plus transform into a
// good shape for developing SDK.
func (m *Introspection) Proto(ctx context.Context) (*File, error) {
	introspectionJson, err := m.Json().Contents(ctx)
	if err != nil {
		return nil, err
	}
	pb, err := codegen.TransformJson(introspectionJson)
	if err != nil {
		return nil, err
	}

	return dag.Directory().
		WithNewFile("introspection.pb.bin", string(pb)).
		File("introspection.pb.bin"), nil
}

type Codegen struct{}

// Fetch the introspection from Dagger Engine.
//
// TODO: Support introspection from module.
func (m *Codegen) Introspect() *Introspection {
	f := dag.Docker().
		Cli(DockerCliOpts{
			Version: dockerVersion,
			Engine:  dag.Docker().Engine(DockerEngineOpts{Version: dockerVersion}),
		}).
		Container().
		WithExec([]string{"apk", "add", "--no-cache", "curl"}).
		WithExec([]string{"sh", "-c", "curl -L https://dl.dagger.io/dagger/install.sh | BIN_DIR=/usr/local/bin sh"}).
		WithNewFile("/introspection.graphql", ContainerWithNewFileOpts{Contents: introspectionGraphql}).
		WithExec([]string{"sh", "-c", "dagger query < /introspection.graphql > " + introspectionJsonPath}, ContainerWithExecOpts{
			ExperimentalPrivilegedNesting: true,
		}).
		File(introspectionJsonPath)

	return &Introspection{F: f}
}
