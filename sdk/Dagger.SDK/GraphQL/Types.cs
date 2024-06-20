using System.Globalization;
using System.Text;

namespace Dagger.SDK.GraphQL;

public abstract class Value
{
    public abstract string Format();
}

public class StringValue(string value) : Value
{
    public override string Format()
    {
        var s = value
            .Replace("\\", @"\\")
            .Replace("\r", "\\r")
            .Replace("\n", "\\n")
            .Replace("\t", "\\t")
            .Replace("\"", "\\\"");
        return $"\"{s}\"";
    }
}

public class IntValue(int n) : Value
{
    public override string Format()
    {
        return n.ToString();
    }
}

public class FloatValue(float f) : Value
{
    public override string Format()
    {
        return f.ToString(CultureInfo.CurrentCulture);
    }
}

public class BooleanValue(bool b) : Value
{
    public override string Format()
    {
        return b ? "true" : "false";
    }
}

public class ListValue(List<Value> list) : Value
{
    public override string Format()
    {
        var builder = new StringBuilder();
        builder.Append('[');
        builder.Append(string.Join(",", list.Select(element => element.Format())));
        builder.Append(']');
        return builder.ToString();
    }
}

public class ObjectValue(List<KeyValuePair<string, Value>> obj) : Value
{
    public override string Format()
    {
        var builder = new StringBuilder();
        builder.Append('{');
        builder.Append(string.Join(",", obj.Select(kv => $"{kv.Key}:{kv.Value.Format()}")));
        builder.Append('}');
        return builder.ToString();
    }
}
