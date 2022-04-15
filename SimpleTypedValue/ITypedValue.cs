using System;

namespace SimpleTypedValue;

public interface ITypedValue
{
    object? Value { get; }
}

public interface ITypedValue<out TValue> : ITypedValue
    where TValue : notnull, IComparable<TValue>, IEquatable<TValue>
{
    new TValue Value { get; }
    object? ITypedValue.Value => Value;
}

