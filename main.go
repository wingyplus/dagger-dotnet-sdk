// The DotNet SDK and runtime.
package main

import (
	"context"
	_ "embed"
	"path"
)

//go:embed introspection.graphql
var introspectionGraphql string

const introspectionJsonPath = "/introspection.json"

const (
	ModSourceDirPath = "/src"
)

func New(
	// +optional
	sdkSourceDir *Directory,
) *DotnetSdk {
	if sdkSourceDir == nil {
		sdkSourceDir = dag.CurrentModule().Source()
	}
	return &DotnetSdk{
		SDKSourceDir: sdkSourceDir,
		Container:    dag.Container(),
	}
}

type DotnetSdk struct {
	SDKSourceDir *Directory
	Container    *Container
}

// Fetch introspection json from the Engine.
//
// This function forked from https://github.com/helderco/daggerverse/blob/main/codegen/main.go but
// didn't modify anything in the data.
//
// It's uses for test for the codegen only.
func (m *DotnetSdk) Introspect() *File {
	return dockerCli().
		Container().
		With(installDaggerCli).
		WithNewFile("/introspection.graphql", ContainerWithNewFileOpts{Contents: introspectionGraphql}).
		WithExec([]string{"sh", "-c", "dagger query < /introspection.graphql > " + introspectionJsonPath}, ContainerWithExecOpts{
			ExperimentalPrivilegedNesting: true,
		}).
		File(introspectionJsonPath)
}

// Testing the SDK.
func (m *DotnetSdk) Test(ctx context.Context) error {
	_, err := m.WithBase(m.SDKSourceDir, "sdk").
		Container.
		WithServiceBinding("dockerd", dockerEngine()).
		WithEnvVariable("DOCKER_HOST", "tcp://dockerd:2375").
		With(installDockerCli).
		With(installDaggerCli).
		WithFile("Dagger.SDK/introspection.json", m.Introspect()).
		WithExec([]string{"dagger", "run", "dotnet", "test"}, ContainerWithExecOpts{
			ExperimentalPrivilegedNesting: true,
		}).
		Sync(ctx)
	return err
}

func (m *DotnetSdk) WithBase(contextDir *Directory, subpath string) *DotnetSdk {
	m.Container = m.Container.
		From("mcr.microsoft.com/dotnet/sdk:8.0-alpine3.20").
		WithMountedDirectory(ModSourceDirPath, contextDir).
		WithWorkdir(path.Join(ModSourceDirPath, subpath))
	return m
}
