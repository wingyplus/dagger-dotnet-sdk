using Dagger.SDK;
using Dagger.SDK.GraphQL;

namespace Dagger.SDK.Tests;


public class EngineTest
{
    [Fact]
    public async void TestExecute()
    {
        var gqlClient = new GraphQLClient();
        var queryBuilder = QueryBuilder
            .Builder()
            .Select("container")
            .Select("from", [new Argument("address", new StringValue("alpine"))])
            .Select("id");

        string id = await Engine.Execute<string>(gqlClient, queryBuilder);

        Assert.NotEmpty(id);
    }

    [Fact]
    public async void TestExecuteList()
    {
        var gqlClient = new GraphQLClient();
        var queryBuilder = QueryBuilder
            .Builder()
            .Select("container")
            .Select("from", [new Argument("address", new StringValue("alpine"))])
            .Select("envVariables")
            .Select("name");

        var ids = await Engine.ExecuteList<string>(gqlClient, queryBuilder);

        Assert.NotEmpty(ids);
    }
}
