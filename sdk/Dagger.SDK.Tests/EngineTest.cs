using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        var engine = new Engine();

        string id = await engine.Execute<string>(gqlClient, queryBuilder);

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
        var engine = new Engine();

        var ids = await engine.ExecuteList<string>(gqlClient, queryBuilder);

        Assert.NotEmpty(ids);
    }
}
