namespace Dagger.SDK.Tests;

[TestClass]
public class ClientTest
{
    private static Query _dag = Dagger.Connect();

    [TestMethod]
    public async Task TestSimple()
    {
        var output = await _dag
            .Container()
            .From("debian")
            .WithExec(["echo", "hello"])
            .Stdout();

        Assert.AreEqual("hello\n", output);
    }

    [TestMethod]
    public async Task TestOptionalArguments()
    {
        var env = await _dag
            .Container()
            .From("debian")
            .WithEnvVariable("A", "a")
            .WithEnvVariable("B", "b")
            .WithEnvVariable("C", "$A:$B", expand: true)
            .EnvVariable("C");

        Assert.AreEqual("a:b", env);
    }

    [TestMethod]
    public async Task TestScalarIdSerialization()
    {
        var cache = _dag.CacheVolume("hello");
        var id = await cache.Id();
        Assert.IsTrue(id.Value.Length > 0);
    }

    [TestMethod]
    public async Task TestInputObject()
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
        
        StringAssert.Contains(output, "SPAM=egg");
    }

    [TestMethod]
    public async Task TestStringEscape()
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
