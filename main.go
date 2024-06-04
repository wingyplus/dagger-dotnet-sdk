// The DotNet SDK and runtime.
package main

import (
	"context"
	_ "embed"
	"path"

	"github.com/iancoleman/strcase"
)

//go:embed introspection.graphql
var introspectionGraphql string

const introspectionJsonPath = "/introspection.json"

const (
	ModSourceDirPath = "/src"
)

var (
	IgnorePaths = []string{
		"**/introspection.json",
		"**/bin",
		"**/obj",
	}
)

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
	return m.Container, nil
}

func (m *DotnetSdk) Codegen(
	ctx context.Context,
	modSource *ModuleSource,
	introspectionJson string,
) (*GeneratedCode, error) {
	modName, err := modSource.ModuleName(ctx)
	if err != nil {
		return nil, err
	}
	subpath, err := modSource.SourceSubpath(ctx)
	if err != nil {
		return nil, err
	}

	source := m.
		WithBase(modSource.ContextDirectory(), subpath).
		WithSln(modName).
		WithSdk(subpath, introspectionJson).
		WithProject(subpath, modName).
		Container.
		Directory(ModSourceDirPath)

	return dag.GeneratedCode(source).
		WithVCSGeneratedPaths([]string{"Dagger.SDK*/**"}).
		WithVCSIgnoredPaths([]string{"Dagger.SDK*/**", "obj", "bin"}), nil
}

func (m *DotnetSdk) WithBase(contextDir *Directory, subpath string) *DotnetSdk {
	m.Container = m.Container.
		From("mcr.microsoft.com/dotnet/sdk:8.0").
		WithMountedDirectory(ModSourceDirPath, contextDir).
		WithWorkdir(path.Join(ModSourceDirPath, subpath))
	return m
}

func (m *DotnetSdk) WithSln(modName string) *DotnetSdk {
	name := strcase.ToCamel(modName)
	m.Container = m.Container.WithExec([]string{"dotnet", "new", "sln", "--name", name})
	return m
}

// Installing sdk into subpath.
func (m *DotnetSdk) WithSdk(subpath string, introspectionJson string) *DotnetSdk {
	m.Container = m.Container.
		WithDirectory(
			"Dagger.SDK",
			m.SDKSourceDir.Directory("Dagger.SDK"),
			ContainerWithDirectoryOpts{Exclude: IgnorePaths},
		).
		WithNewFile("Dagger.SDK/introspection.json", ContainerWithNewFileOpts{
			Contents: introspectionJson,
		}).
		WithDirectory(
			"Dagger.SDK.SourceGenerator/Dagger.SDK.SourceGenerator",
			m.SDKSourceDir.Directory("Dagger.SDK.SourceGenerator/Dagger.SDK.SourceGenerator"),
			ContainerWithDirectoryOpts{Exclude: IgnorePaths},
		).
		WithExec([]string{"dotnet", "sln", "add", "Dagger.SDK"}).
		WithExec([]string{"dotnet", "sln", "add", "Dagger.SDK.SourceGenerator/Dagger.SDK.SourceGenerator"})

	return m
}

func (m *DotnetSdk) WithProject(subpath string, modName string) *DotnetSdk {
	name := strcase.ToCamel(modName)
	m.Container = m.Container.
		WithExec([]string{"dotnet", "new", "console", "--framework", "net8.0", "--output", name, "-n", name}).
		WithExec([]string{"dotnet", "sln", "add", name}).
		WithWorkdir(name).
		WithExec([]string{"dotnet", "add", "reference", "../Dagger.SDK"})

	return m
}
