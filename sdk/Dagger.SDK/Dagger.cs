using Dagger.SDK.GraphQL;

namespace Dagger.SDK;

public static class Dagger
{
    static readonly Lazy<Query> Query = new(() => new Query(QueryBuilder.Builder(), new GraphQLClient()));
    
    public static Query Connect() => Query.Value;
}
