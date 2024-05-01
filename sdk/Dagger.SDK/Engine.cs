using Dagger.SDK.GraphQL;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dagger.SDK;

public class Engine
{
    /// <summary>
    /// Execute a GraphQL request and deserialize data into `T`.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="client">A GraphQL client.</param>
    /// <param name="queryBuilder">A QueryBuilder instance.</param>
    /// <returns></returns>
    public static async Task<T> Execute<T>(GraphQLClient client, QueryBuilder queryBuilder)
    {
        var jsonElement = await Request(client, queryBuilder);
        jsonElement = TakeJsonElementUntilLast<T>(jsonElement, queryBuilder.Path);
        return jsonElement.GetProperty(queryBuilder.Path.Last().Name).Deserialize<T>()!;
    }

    /// <summary>
    /// Similar to Execute but return a list of data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="client">A GraphQL client</param>
    /// <param name="queryBuilder">A QueryBuilder instance.</param>
    /// <returns></returns>
    public static async Task<List<T>> ExecuteList<T>(GraphQLClient client, QueryBuilder queryBuilder)
    {
        var jsonElement = await Request(client, queryBuilder);
        jsonElement = TakeJsonElementUntilLast<T>(jsonElement, queryBuilder.Path);
        return jsonElement
            .EnumerateArray()
            .Select(elem => elem.GetProperty(queryBuilder.Path.Last().Name).Deserialize<T>()!)
            .ToList();
    }

    private static async Task<JsonElement> Request(GraphQLClient client, QueryBuilder queryBuilder)
    {
        var query = queryBuilder.Build();
        var response = await client.RequestAsync(query);
        // TODO: handle error here.
        var jsonElement = await response.Content.ReadFromJsonAsync<JsonElement>()!;
        return jsonElement.GetProperty("data");
    }

    // Traverse jsonElement until the last element.
    private static JsonElement TakeJsonElementUntilLast<T>(JsonElement jsonElement, ImmutableList<Field> path)
    {
        var json = jsonElement;
        foreach (var fieldName in path.RemoveAt(path.Count - 1).Select(field => field.Name))
        {
            json = json.GetProperty(fieldName);
        }
        return json;
    }
}
