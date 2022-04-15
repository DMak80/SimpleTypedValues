using System;
using Xunit;

namespace SimpleTypedValue.Generator.Test;

[TypedValue]
public partial struct TypedIDDefaultConstructor : ITypedValue<long>
{
    public TypedIDDefaultConstructor()
    {
        Value = 5;
    }

    public long Value { get; }
}

[TypedValue]
public partial struct TypedStringDefaultConstructor : ITypedValue<string>
{
    public TypedStringDefaultConstructor()
    {
        Value = "asd";
    }

    public string Value { get; }
}

[TypedValue]
public partial struct TypedIDValueConstructor : ITypedValue<long>
{
    public TypedIDValueConstructor(long newValue)
    {
        Value = Math.Max(newValue, 0);
    }

    public long Value { get; }
}

public class Tests
{
    [Fact]
    public void Test_long_DefaultConstructor()
    {
        Assert.Equal(5, new TypedIDDefaultConstructor().Value);
    }

    [Fact]
    public void Test_string_DefaultConstructor()
    {
        Assert.Equal("asd", new TypedStringDefaultConstructor().Value);
    }

    [Fact]
    public void Test_long_ValueConstructor()
    {
        Assert.Equal(0, new TypedIDValueConstructor(-10).Value);
    }
}