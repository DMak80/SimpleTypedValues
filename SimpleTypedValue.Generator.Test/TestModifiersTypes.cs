using System;

namespace SimpleTypedValue.Generator.Test;

[TypedValue]
public readonly partial struct RoTypedIdStruct<T> : ITypedValue<T>
    where T : struct, IComparable<T>, IEquatable<T>
{
    public T Value { get; }
}

public static partial class MyStaticClass
{
    [TypedValue]
    public partial struct SomeId : ITypedValue<long>
    {
        public long Value { get; }
    }
    
    [TypedValue]
    public readonly partial struct RoSomeId : ITypedValue<long>
    {
        public long Value { get; }
    }

    static MyStaticClass()
    {
        var id1 = new SomeId(1);
        var id2 = new RoSomeId(10);
        var id3 = new RoTypedIdStruct<int>(5);
        Console.WriteLine(id1.Value);
        Console.WriteLine(id2.Value);
        Console.WriteLine(id3.Value);
    }
}