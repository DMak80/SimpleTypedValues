using System;
using System.Text.Json;
using Xunit;

namespace SimpleTypedValue.Json.Test;

[TypedValue(TypedValueExtensionFlag.Json)]
public partial struct SomeId : ITypedValue<long>
{
    public long Value { get; }
}

[TypedValue(TypedValueExtensionFlag.Json)]
public partial struct SomeGuid : ITypedValue<Guid>
{
    public Guid Value { get; }
}

[TypedValue(TypedValueExtensionFlag.Json)]
public partial struct SomeString : ITypedValue<string>
{
    private static readonly string Default = string.Empty;
    public string Value { get; }
}

[TypedValue(TypedValueExtensionFlag.Json)]
public partial struct SomeDateTime : ITypedValue<DateTime>
{
    public DateTime Value { get; }
}

public class Tests
{
    [Fact]
    public void Test_long()
    {
        var id = new SomeId(10);
        var json = JsonSerializer.Serialize(id);
        var newId = JsonSerializer.Deserialize<SomeId>(json);
        var newValue = JsonSerializer.Deserialize<long>(json);
        Assert.Equal(10, newId.Value);
        Assert.Equal(10, newValue);
    }

    [Fact]
    public void Test_guid()
    {
        var guid = Guid.NewGuid();
        var id = new SomeGuid(guid);
        var json = JsonSerializer.Serialize(id);
        var newId = JsonSerializer.Deserialize<SomeGuid>(json);
        var newValue = JsonSerializer.Deserialize<Guid>(json);
        Assert.Equal(guid, newId.Value);
        Assert.Equal(guid, newValue);
    }

    [Fact]
    public void Test_string()
    {
        const string str = "1w2e";
        var id = new SomeString(str);
        var json = JsonSerializer.Serialize(id);
        var newId = JsonSerializer.Deserialize<SomeString>(json);
        var newValue = JsonSerializer.Deserialize<string>(json);
        Assert.Equal(str, newId.Value);
        Assert.Equal(str, newValue);
    }

    [Fact]
    public void Test_datetime()
    {
        var dt = DateTime.Now;
        var id = new SomeDateTime(dt);
        var json = JsonSerializer.Serialize(id);
        var newId = JsonSerializer.Deserialize<SomeDateTime>(json);
        var newValue = JsonSerializer.Deserialize<DateTime>(json);
        Assert.Equal(dt, newId.Value);
        Assert.Equal(dt, newValue);
    }
}