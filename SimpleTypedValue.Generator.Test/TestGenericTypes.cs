using System;

namespace SimpleTypedValue.Generator.Test;

[TypedValue]
public partial struct TypedIDStruct<T> : ITypedValue<T>
    where T : struct, IComparable<T>, IEquatable<T>
{
    public T Value { get; }
}

[TypedValue]
public partial struct TypedIDClass<T> : ITypedValue<T>
    where T : class, IComparable<T>, IEquatable<T>, new()
{
    private static readonly T Default = new();
    public T Value { get; }
}

[TypedValue]
public partial struct TypedID<T> : ITypedValue<T>
    where T : notnull, IComparable<T>, IEquatable<T>, new()
{
    private static readonly T Default = new();
    public T Value { get; }
}

public partial class EntityBase<TKey, T, TValue>
    where T : class, IComparable<T>, IEquatable<T>, new()
{
    [TypedValue]
    public partial struct ID : ITypedValue<T>
    {
        private static readonly T Default = new();
        public T Value { get; }
    }

    [TypedValue]
    public partial struct ID<TT> : ITypedValue<TT>
        where TT : struct, IComparable<TT>, IEquatable<TT>
    {
        public TT Value { get; }
    }
}

public class BaseEntity<TID>
    where TID : notnull, new()
{
    public TID Id { get; set; } = new();
}

public partial class TestEntity2 : BaseEntity<TestEntity2.ID>
{
    [TypedValue]
    public partial struct ID : ITypedValue<long>
    {
        public long Value { get; }
    }

    public string? Text2 { get; set; }
}

public partial class TestEntity3 : BaseEntity<TestEntity3.ID>
{
    [TypedValue]
    public partial struct ID : ITypedValue<long>
    {
        public long Value { get; }
    }
}