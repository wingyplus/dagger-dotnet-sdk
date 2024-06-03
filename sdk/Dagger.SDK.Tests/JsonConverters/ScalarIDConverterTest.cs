using System.Text.Json;
using System.Text.Json.Serialization;

using Dagger.SDK.JsonConverters;

namespace Dagger.SDK.Tests;

public class ScalarIDConverterTest
{
    [JsonConverter(typeof(ScalarIDConverter<DemoID>))]
    public class DemoID : Scalar
    {
    }

    [Fact]
    public void TestJsonSerialization()
    {
        var demoId = JsonSerializer.Deserialize<DemoID>("\"hello\"")!;
        Assert.Equal("hello", demoId.Value);
        Assert.Equal("\"hello\"", JsonSerializer.Serialize(demoId));
    }
}
