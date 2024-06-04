namespace Dagger.SDK.Tests;

public class ClientTest
{
    private Query _dag = Dagger.Connect();

    [Fact]
    public async void TestSimple()
    {
        var output = await _dag
            .Container()
            .From("debian")
            .WithExec(["echo", "hello"])
            .Stdout();

        Assert.Equal("hello\n", output);
    }

    [Fact]
    public async void TestOptionalArguments()
    {
        var env = await _dag
            .Container()
            .From("debian")
            .WithEnvVariable("A", "a")
            .WithEnvVariable("B", "b")
            .WithEnvVariable("C", "$A:$B", expand: true)
            .EnvVariable("C");

        Assert.Equal("a:b", env);
    }

    [Fact]
    public async void TestScalarIdSerialization()
    {
        var cache = _dag.CacheVolume("hello");
        var id = await cache.Id();
        Assert.True(id.Value.Length > 0);
    }

    [Fact]
    public async void TestInputObject()
    {
        const string dockerfile = """
                                  FROM alpine:3.20.0
                                  ARG SPAM=spam
                                  ENV SPAM=$SPAM
                                  CMD printenv
                                  """;

        var dockerDir = _dag.Directory().WithNewFile("Dockerfile", dockerfile);
        var output = await _dag.Container()
            .Build(await dockerDir.Id(), buildArgs: [new BuildArg("SPAM", "egg")])
            .Stdout();

        Assert.Matches(".*SPAM=egg.*", output);
    }

    [Fact]
    public async void TestStringEscape()
    {
        await _dag
            .Container()
            .From("alpine")
            .WithNewFile("/a.txt", contents:
                """
                  \\  /       Partly cloudy
                _ /\"\".-.     +29(31) °C
                  \\_(   ).   ↑ 13 km/h
                  /(___(__)  10 km
                             0.0 mm
                """)
            .Sync();
    }
}
