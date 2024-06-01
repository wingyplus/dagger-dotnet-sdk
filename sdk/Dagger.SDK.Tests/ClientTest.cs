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
}
