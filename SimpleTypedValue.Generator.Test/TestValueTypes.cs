using System;

namespace SimpleTypedValue.Generator.Test;

public struct CustomId: IComparable<CustomId>, IEquatable<CustomId>
{
    public int Id { get; set; }

    public int CompareTo(CustomId other)
    {
        return Id.CompareTo(other.Id);
    }

    public bool Equals(CustomId other)
    {
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is CustomId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id;
    }
}
/// <summary>
/// Type must not be defined
/// </summary>
[TypedValue]
public partial struct SomeIdTypedLong : ITypedValue<long>
{
    public long Value { get; }
}

/// <summary>
/// Type must not be defined
/// </summary>
[TypedValue]
public partial struct SomeIdTypedInt : ITypedValue<Int32>
{
    public int Value { get; }
}

/// <summary>
/// Type must not be defined
/// </summary>
[TypedValue]
public partial struct SomeIdTypedCustomId : ITypedValue<CustomId>
{
    public CustomId Value { get; }
}

/// <summary>
/// Type must not be defined
/// </summary>
[TypedValue]
public partial struct SomeIdTypedDateTime : ITypedValue<DateTime>
{
    public DateTime Value { get; }
}

/// <summary>
/// Type must not be defined
/// </summary>
[TypedValue]
public partial struct SomeIdTypedGuid : ITypedValue<Guid>
{
    public Guid Value { get; }
}

/// <summary>
/// Type must be defined
/// </summary>
[TypedValue]
public partial struct SomeIdBad
{
}

/// <summary>
/// Should be an error because ITypedValue.Value has implementation
/// </summary>
[TypedValue]
public partial struct SomeIdTyped : ITypedValue
{
    public object? Value { get; }
}

public partial class SomeEntity
{
    /// <summary>
    /// Type must not be defined
    /// </summary>
    [TypedValue]
    public partial struct SomeIdTypedLongInner : ITypedValue<long>
    {
        public long Value { get; }
    }
}
