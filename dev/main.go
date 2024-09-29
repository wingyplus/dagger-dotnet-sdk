// A dev module for dagger-dotnet-sdk.
//
// This module contains functions for developing the SDK such as, running tests,
// generate introspection, etc.
package main

import (
	"context"
	_ "embed"

	"github.com/dagger/dagger/sdk/dotnet/dev/internal/dagger"
)

//go:embed introspection.graphql
var introspectionGraphql string

const introspectionJsonPath = "/introspection.json"

func New(
	// Dotnet SDK source.
	//
	// +optional
	// +defaultPath=".."
	// +ignore=["**/*","!sdk/.config","!sdk/Dagger.sln","!sdk/Dagger.sln.DotSettings.user","!sdk/global.json","!sdk/**/*.cs","!sdk/**/*.csproj"]
	source *dagger.Directory) *DotnetSdkDev {
	return &DotnetSdkDev{
		Source: source,
	}
}

type DotnetSdkDev struct {
	Source *dagger.Directory
}

// Fetch introspection json from the Engine.
//
// This function forked from https://github.com/helderco/daggerverse/blob/main/codegen/main.go but
// didn't modify anything in the JSON file.
//
// It's uses only for testing the codegen.
func (m *DotnetSdkDev) Introspect() *dagger.File {
	return dag.Container().
		From("alpine:3.20").
		With(installDaggerCli).
		WithNewFile("/introspection.graphql", introspectionGraphql).
		WithExec([]string{"sh", "-c", "dagger query < /introspection.graphql > " + introspectionJsonPath}, dagger.ContainerWithExecOpts{
			ExperimentalPrivilegedNesting: true,
		}).
		File(introspectionJsonPath)
}

// Testing the SDK.
func (m *DotnetSdkDev) Test(ctx context.Context) error {
	_, err := m.Base().
		WithFile("Dagger.SDK/introspection.json", m.Introspect()).
		WithExec([]string{"dotnet", "restore"}).
		WithExec([]string{"dotnet", "build", "--no-restore"}).
		WithExec([]string{"dotnet", "test", "--no-build"}, dagger.ContainerWithExecOpts{
			ExperimentalPrivilegedNesting: true,
		}).
		Sync(ctx)
	return err
}

// Lint all CSharp source files in the SDK.
func (m *DotnetSdkDev) Lint(ctx context.Context) error {
	_, err := m.Base().
		WithExec([]string{"dotnet", "tool", "restore"}).
		WithExec([]string{"dotnet", "csharpier", "--check", "."}).
		Sync(ctx)
	return err
}

// Format all CSharp source files.
func (m *DotnetSdkDev) Format() *dagger.Directory {
	return m.Base().
		WithExec([]string{"dotnet", "csharpier", "."}).
		Directory(".")
}

func (m *DotnetSdkDev) Base() *dagger.Container {
	return dag.Container().
		From("mcr.microsoft.com/dotnet/sdk:8.0-alpine3.20").
		WithMountedDirectory("/src", m.Source).
		WithWorkdir("/src/sdk").
		WithExec([]string{"dotnet", "tool", "restore"})
}
