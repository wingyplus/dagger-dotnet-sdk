using Dagger.SDK.GraphQL;
using Dagger.SDK;

namespace Dagger.SDK.Tests;


public class ClientTest
{
    [Fact]
    public async void TestSimple()
    {
        var client = new Query(QueryBuilder.Builder(), new GraphQLClient());
        var output = await client
            .Container()
            .From("debian")
            .WithExec(["echo", "hello"])
            .Stdout();

        Assert.Equal("hello\n", output);
    }

    [Fact]
    public async void TestOptionalArguments()
    {
        var client = new Query(QueryBuilder.Builder(), new GraphQLClient());
        var env = await client
            .Container()
            .From("debian")
            .WithEnvVariable("A", "a")
            .WithEnvVariable("B", "b")
            .WithEnvVariable("C", "$A:$B", expand: true)
            .EnvVariable("C");

        Assert.Equal("a:b", env);
    }
    
    [Fact]
    public async void TestConnect()
    {
        var dag = Dagger.Connect();

        var output = await dag.Container()
            .From("debian")
            .WithExec(["echo", "hello"])
            .Stdout();
        
        Assert.Equal("hello\n", output);
    }
}
