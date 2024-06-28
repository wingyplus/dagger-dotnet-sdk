// Functions for managing Dagger.
package main

const daggerVersion = "0.11.9"

func installDaggerCli(ctr *Container) *Container {
	return ctr.
		WithEnvVariable("BIN_DIR", "/usr/local/bin").
		WithEnvVariable("DAGGER_VERSION", daggerVersion).
		WithExec([]string{"apk", "add", "--no-cache", "curl"}).
		WithExec([]string{"sh", "-c", "curl -L https://dl.dagger.io/dagger/install.sh | sh"})
}
