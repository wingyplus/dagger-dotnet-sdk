package main

const DockerEngineVersion = "26"

func dockerEngine() *Service {
	return dag.Docker().
		Engine(DockerEngineOpts{Version: DockerEngineVersion})
}

func dockerCli() *DockerCli {
	return dag.Docker().
		Cli(DockerCliOpts{
			Version: DockerEngineVersion,
		})
}

func installDockerCli(ctr *Container) *Container {
	return ctr.WithFile(
		"/usr/local/bin/docker",
		dockerCli().Container().File("/usr/local/bin/docker"),
	)
}
