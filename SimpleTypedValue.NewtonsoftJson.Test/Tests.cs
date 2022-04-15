using System;
using Newtonsoft.Json;
using Xunit;

namespace SimpleTypedValue.NewtonsoftJson.Test;

[TypedValue(TypedValueExtensionFlag.NewtonsoftJson)]
public partial struct SomeId : ITypedValue<long>
{
    public long Value { get; }
}

[TypedValue(TypedValueExtensionFlag.NewtonsoftJson)]
public partial struct SomeGuid : ITypedValue<Guid>
{
    public Guid Value { get; }
}

[TypedValue(TypedValueExtensionFlag.NewtonsoftJson)]
public partial struct SomeString : ITypedValue<string>
{
    private static readonly string Default = string.Empty;
    public string Value { get; }
}

[TypedValue(TypedValueExtensionFlag.NewtonsoftJson)]
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
        var json = JsonConvert.SerializeObject(id);
        var newId = JsonConvert.DeserializeObject<SomeId>(json);
        var newValue = JsonConvert.DeserializeObject<long>(json);
        Assert.Equal(10, newId.Value);
        Assert.Equal(10, newValue);
    }

    [Fact]
    public void Test_guid()
    {
        var guid = Guid.NewGuid();
        var id = new SomeGuid(guid);
        var json = JsonConvert.SerializeObject(id);
        var newId = JsonConvert.DeserializeObject<SomeGuid>(json);
        var newValue = JsonConvert.DeserializeObject<Guid>(json);
        Assert.Equal(guid, newId.Value);
        Assert.Equal(guid, newValue);
    }

    [Fact]
    public void Test_string()
    {
        const string str = "1w2e";
        var id = new SomeString(str);
        var json = JsonConvert.SerializeObject(id);
        var newId = JsonConvert.DeserializeObject<SomeString>(json);
        var newValue = JsonConvert.DeserializeObject<string>(json);
        Assert.Equal(str, newId.Value);
        Assert.Equal(str, newValue);
    }

    [Fact]
    public void Test_datetime()
    {
        var dt = DateTime.Now;
        var id = new SomeDateTime(dt);
        var json = JsonConvert.SerializeObject(id);
        var newId = JsonConvert.DeserializeObject<SomeDateTime>(json);
        var newValue = JsonConvert.DeserializeObject<DateTime>(json);
        Assert.Equal(dt, newId.Value);
        Assert.Equal(dt, newValue);
    }
}