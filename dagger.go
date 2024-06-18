package main

func installDaggerCli(ctr *Container) *Container {
	return ctr.
		WithExec([]string{"apk", "add", "--no-cache", "curl"}).
		WithExec([]string{"sh", "-c", "curl -L https://dl.dagger.io/dagger/install.sh | BIN_DIR=/usr/local/bin sh"})
}
