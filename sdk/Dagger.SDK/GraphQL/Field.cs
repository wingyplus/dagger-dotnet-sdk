using System.Collections.Immutable;

namespace Dagger.SDK.GraphQL;

public class Argument(string key, Value value)
{
    public Argument(string key, string value) : this(key, new StringValue(value))
    {
    }

    public Argument(string key, int value) : this(key, new IntValue(value))
    {
    }

    public Argument(string key, bool value) : this(key, new BooleanValue(value))
    {
    }

    public Argument(string key, List<Value> value) : this(key, new ListValue(value))
    {
    }

    public Argument(string key, List<KeyValuePair<string, Value>> value) : this(key, new ObjectValue(value))
    {
    }

    public string Key { get; } = key;
    public Value Value { get; } = value;

    public string FormatValue()
    {
        return Value.Format();
    }
}

public class Field(string name, ImmutableList<Argument> args)
{
    public string Name { get; } = name;

    public ImmutableList<Argument> Args { get; } = args;
}
