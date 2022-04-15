using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace SimpleTypedValue.Json;

internal static class SerializerFactory
{
    public static Action<Utf8JsonWriter, T, JsonSerializerOptions> Build<T>()
        where T : ITypedValue, new()
    {
        var info = TypedValue.GetInfo<T>()!;
        return GetMethodA((dynamic)info.Default, (dynamic)info.DefaultValueInstance,
            (dynamic)info.DefaultValueInstance);
    }

    private static Action<Utf8JsonWriter, T, JsonSerializerOptions> GetMethodA<T, TT>(T defaultObj, TT defaultValue,
        TT defaultValue2)
        where T : ITypedValue<TT>, new()
        where TT : notnull, IComparable<TT>, IEquatable<TT>
    {
        return (Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            => JsonSerializer.Serialize(writer, value.Value, options);
    }

    private static Action<Utf8JsonWriter, T, JsonSerializerOptions> GetMethodA<T, TT>(T defaultObj, TT defaultValue,
        object defaultValue2)
        where T : ITypedValue, new()
        where TT : notnull
    {
        return (Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            => JsonSerializer.Serialize(writer, (TT?)value.Value, options);
    }
}