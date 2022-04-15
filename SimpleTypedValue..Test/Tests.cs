using System.ComponentModel;
using Xunit;

namespace SimpleTypedValue._Test;

[TypedValue]
public partial struct LongTypedValue : ITypedValue<long>
{
    public long Value { get; private init; }
}

[TypedValue]
public partial struct StringTypedValue : ITypedValue<string>
{
    private static string Default = string.Empty;
    public string Value { get; private init; }
}

public class Tests
{
    [Fact]
    public void TestTypedValueCreator_Long()
    {
        Assert.Equal(1L, TypedValueCreator<LongTypedValue, long>.Create(1L).Value);
    }

    [Fact]
    public void TestTypedValueCreator_LongFromInt()
    {
        Assert.Equal(1L, TypedValueCreator<LongTypedValue, int>.Create(1).Value);
    }

    [Fact]
    public void TestTypedValueCreator_String()
    {
        Assert.Equal("Some", TypedValueCreator<StringTypedValue, string>.Create("Some").Value);
    }

    [Fact]
    public void TestFrom_String()
    {
        Assert.Equal("Some", new StringTypedValue("Some").Value);
    }

    [Fact]
    public void TestConverterTo_String()
    {
        var converter = TypeDescriptor.GetConverter(typeof(StringTypedValue));
        var result = converter.CanConvertFrom("Some".GetType())
            ? converter.ConvertFrom("Some")
            : null;
        Assert.Equal(new StringTypedValue("Some"), result);
    }

    [Fact]
    public void TestConverterTo_FromInt_Long()
    {
        var converter = TypeDescriptor.GetConverter(typeof(LongTypedValue));
        var result = converter.CanConvertFrom(123.GetType())
            ? converter.ConvertFrom(123)
            : null;
        Assert.Equal(new LongTypedValue(123), result);
    }

    [Fact]
    public void TestConverterTo_FromString_Long()
    {
        var converter = TypeDescriptor.GetConverter(typeof(LongTypedValue));
        var result = converter.CanConvertFrom("123".GetType())
            ? converter.ConvertFrom("123")
            : null;
        Assert.Equal(new LongTypedValue(123), result);
    }

    [Fact]
    public void TestGetInfo_byType()
    {
        var info = TypedValue.GetInfo(typeof(LongTypedValue));
        Assert.Equal(TypedValue<LongTypedValue>.Info, info);
    }

    [Fact]
    public void TestGetInfo_byTypeGeneric()
    {
        var info = TypedValue.GetInfo<LongTypedValue>();
        Assert.Equal(TypedValue<LongTypedValue>.Info, info);
    }
}