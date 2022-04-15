using System;
using System.Data;
using Dapper;

namespace SimpleTypedValue.Dapper;

public class SimpleTypedValueTypeHandler<T, TValue> : SqlMapper.TypeHandler<T>
    where T : ITypedValue<TValue>, new()
    where TValue : notnull, IComparable<TValue>, IEquatable<TValue>
{
    private static readonly TypedValueInfo Info = TypedValue.GetInfo<T>()!;

    public override void SetValue(IDbDataParameter parameter, T value)
    {
        parameter.Value = value.Value;
    }

    public override T Parse(object value)
    {
        return (T)Info.Create(value);
    }
}