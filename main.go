// The DotNet SDK and runtime.
package main

import (
	"bytes"
	"context"
	_ "embed"
	"path"
	"text/template"

	"dagger/dotnet-sdk/internal/dagger"

	"github.com/iancoleman/strcase"
)

//go:embed templates/Program.cs
var mainProg string

//go:embed templates/MainModule.cs
var mainModule string

const (
	ModSourceDirPath = "/src"
)

func New(
	// +optional
	sdkSourceDir *dagger.Directory,
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
	SDKSourceDir  *dagger.Directory
	RequiredPaths []string
	Container     *dagger.Container
}

func (m *DotnetSdk) ModuleRuntime(
	ctx context.Context,
	modSource *dagger.ModuleSource,
	introspectionJson *dagger.File,
) (*dagger.Container, error) {
	subpath, err := modSource.SourceSubpath(ctx)
	if err != nil {
		return nil, err
	}

	modName, err := modSource.ModuleName(ctx)
	if err != nil {
		return nil, err
	}
	name := strcase.ToCamel(modName)

	m, err = m.codegenBase(ctx, modSource, introspectionJson)
	if err != nil {
		return nil, err
	}

	return m.Container.WithEntrypoint([]string{"dotnet", "run", "--project", path.Join(ModSourceDirPath, subpath, name)}), nil
}

func (m *DotnetSdk) Codegen(
	ctx context.Context,
	modSource *dagger.ModuleSource,
	introspectionJson *dagger.File,
) (*dagger.GeneratedCode, error) {
	m, err := m.codegenBase(ctx, modSource, introspectionJson)
	if err != nil {
		return nil, err
	}

	return dag.GeneratedCode(m.Container.Directory(ModSourceDirPath)).
		WithVCSGeneratedPaths([]string{"Dagger.SDK*/**", "**/Program.cs"}).
		WithVCSIgnoredPaths([]string{"Dagger.SDK*/**", "**/obj", "**/bin", "**/.idea", "**/Program.cs"}), nil
}

func (m *DotnetSdk) codegenBase(ctx context.Context, modSource *dagger.ModuleSource, introspectionJson *dagger.File) (*DotnetSdk, error) {
	modName, err := modSource.ModuleName(ctx)
	if err != nil {
		return nil, err
	}
	subpath, err := modSource.SourceSubpath(ctx)
	if err != nil {
		return nil, err
	}

	m, err = m.
		WithBase(modSource.ContextDirectory(), subpath).
		WithSln(modName).
		WithIntrospection(introspectionJson).
		WithProject(ctx, subpath, modName)
	if err != nil {
		return nil, err
	}

	return m.WithSdk(subpath, modName), nil
}

func (m *DotnetSdk) WithBase(contextDir *dagger.Directory, subpath string) *DotnetSdk {
	m.Container = m.Container.
		From("mcr.microsoft.com/dotnet/sdk:8.0-alpine3.20").
		WithMountedDirectory(ModSourceDirPath, contextDir).
		WithWorkdir(path.Join(ModSourceDirPath, subpath))
	return m
}

func (m *DotnetSdk) WithSln(modName string) *DotnetSdk {
	name := strcase.ToCamel(modName)
	m.Container = m.Container.WithExec([]string{"dotnet", "new", "sln", "--name", name, "--force"})
	return m
}

// Installing sdk into subpath.
func (m *DotnetSdk) WithSdk(subpath string, modName string) *DotnetSdk {
	name := strcase.ToCamel(modName)
	includePatterns := []string{"**/*.cs", "**/*.csproj"}
	projects := []string{
		"Dagger.SDK",
		"Dagger.SDK.Mod",
		"Dagger.SDK.Mod.SourceGenerator",
		"Dagger.SDK.SourceGenerator/Dagger.SDK.SourceGenerator",
	}

	// Copy the SDK and link it to the project.
	for _, project := range projects {
		m.Container = m.Container.
			WithDirectory(
				project,
				m.SDKSourceDir.Directory(project),
				dagger.ContainerWithDirectoryOpts{Include: includePatterns},
			).
			WithExec([]string{"dotnet", "sln", "add", project}).
			WithExec([]string{"dotnet", "add", name, "reference", project})
	}

	return m
}

func (m *DotnetSdk) WithIntrospection(introspectionJson *dagger.File) *DotnetSdk {
	m.Container = m.Container.
		WithFile("Dagger.SDK/introspection.json", introspectionJson)

	return m
}

func (m *DotnetSdk) WithProject(ctx context.Context, subpath string, modName string) (*DotnetSdk, error) {
	ctr := m.Container
	name := strcase.ToCamel(modName)

	ents, err := m.Container.Directory(".").Entries(ctx)
	if err != nil {
		return nil, err
	}
	created := false
	for _, ent := range ents {
		if ent == name {
			created = true
			break
		}
	}

	if !created {
		var buf bytes.Buffer
		err = template.Must(template.New("MainModule.cs").Parse(mainModule)).Execute(&buf, struct{ Module string }{Module: name})
		if err != nil {
			return nil, err
		}

		mainMod := buf.String()
		ctr = ctr.
			WithExec([]string{"dotnet", "new", "console", "--framework", "net8.0", "--output", name, "-n", name}).
			WithNewFile(name+"/"+name+".cs", mainMod)
	}

	var buf bytes.Buffer
	err = template.Must(template.New("Program.cs").Parse(mainProg)).Execute(&buf, struct{ Module string }{Module: name})
	if err != nil {
		return nil, err
	}

	prog := buf.String()

	m.Container = ctr.
		WithExec([]string{"dotnet", "sln", "add", name}).
		WithNewFile(name+"/Program.cs", prog)

	return m, nil
}
