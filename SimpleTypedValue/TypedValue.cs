using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using SimpleTypedValue.Utils;

namespace SimpleTypedValue;

public static class TypedValue<T>
    where T : ITypedValue, new()
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly TypedValueInfo Info = FromType();

    private static TypedValueInfo<T> FromType()
    {
        return typeof(T).GetInterfaces()
                   .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITypedValue<>))
                   .Select(i => new TypedValueInfo<T>(i, i.GenericTypeArguments[0]))
                   .FirstOrDefault()
               ?? typeof(T).GetInterfaces()
                   .Where(i => i == typeof(ITypedValue))
                   .Select(i => new TypedValueInfo<T>(i, typeof(object)))
                   .First();
    }
}

public class TypedValue
{
    private static readonly StaticCache<Type, TypedValueInfo?> StaticCache = new();

    private static readonly MethodInfo FromMethod = typeof(TypedValue)
        .GetMethods(BindingFlags.Static | BindingFlags.Public)
        .First(m => m.IsGenericMethod && m.Name.StartsWith(nameof(GetInfo)));

    public static TypedValueInfo? GetInfo(Type type)
    {
        return StaticCache.GetOrAdd(type, t => typeof(ITypedValue).IsAssignableFrom(type)
            ? (TypedValueInfo?)FromMethod.MakeGenericMethod(t).Invoke(null, null)
            : null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypedValueInfo? GetInfo<T>()
        where T : ITypedValue, new()
    {
        return TypedValue<T>.Info;
    }
}