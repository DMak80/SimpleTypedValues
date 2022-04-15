using System;
using Dapper;

namespace SimpleTypedValue.Dapper;

public static class SimpleTypedValueDapperExtensions
{
    public static void AddDapperTypedValueHandler(this Type type)
    {
        var info = TypedValue.GetInfo(type); 
        if (info != null)
            SqlMapper.AddTypeHandler(type, info.AsDapperTypeHandler());
    }

    private static SqlMapper.ITypeHandler AsDapperTypeHandler(this TypedValueInfo info)
        => Create((dynamic)info.Default, (dynamic)info.DefaultValueInstance);

    private static SqlMapper.ITypeHandler Create<T, TValue>(T val, TValue value)
        where T : ITypedValue<TValue>, new()
        where TValue : notnull, IComparable<TValue>, IEquatable<TValue>
    {
        return new SimpleTypedValueTypeHandler<T, TValue>();
    }
}