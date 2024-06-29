// A dev module for dagger-dotnet-sdk.
//
// This module contains functions for developing the SDK such as, running tests,
// generate introspection, etc.
package main

import (
	"context"
	_ "embed"
)

//go:embed introspection.graphql
var introspectionGraphql string

const introspectionJsonPath = "/introspection.json"

type Dev struct {
}

// Fetch introspection json from the Engine.
//
// This function forked from https://github.com/helderco/daggerverse/blob/main/codegen/main.go but
// didn't modify anything in the data.
//
// It's uses for test for the codegen only.
func (m *Dev) Introspect() *File {
	return dag.Container().
		From("alpine:3.20").
		With(installDaggerCli).
		WithNewFile("/introspection.graphql", ContainerWithNewFileOpts{Contents: introspectionGraphql}).
		WithExec([]string{"sh", "-c", "dagger query < /introspection.graphql > " + introspectionJsonPath}, ContainerWithExecOpts{
			ExperimentalPrivilegedNesting: true,
		}).
		File(introspectionJsonPath)
}

func (m *Dev) Test(
	ctx context.Context,
	// SDK Source directory.
	source *Directory,
) error {
	_, err := dag.Container().
		From("mcr.microsoft.com/dotnet/sdk:8.0-alpine3.20").
		WithMountedDirectory("/src", source).
		WithWorkdir("/src/sdk").
		WithFile("Dagger.SDK/introspection.json", m.Introspect()).
		WithExec([]string{"dotnet", "restore"}).
		WithExec([]string{"dotnet", "build", "--no-restore"}).
		WithExec([]string{"dotnet", "test", "--no-build", "--blame-hang", "--blame-hang-timeout", "2m"}, ContainerWithExecOpts{
			ExperimentalPrivilegedNesting: true,
		}).
		Sync(ctx)
	return err
}
