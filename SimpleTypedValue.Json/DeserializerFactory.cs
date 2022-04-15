using System.Text.Json;

namespace SimpleTypedValue.Json;

delegate T? DeserializerMethod<T>(ref Utf8JsonReader reader)
    where T : ITypedValue, new();

internal static class DeserializerFactory
{
    public static DeserializerMethod<T> Build<T>()
        where T : ITypedValue, new()
    {
        var info = TypedValue.GetInfo<T>()!;

        return Deserialize((dynamic)info.Default, (dynamic)info.DefaultValueInstance);
    }

    private static DeserializerMethod<T> Deserialize<T, TValue>(T defaultObj, TValue defaultValue)
        where T : ITypedValue, new()
    {
        return (ref Utf8JsonReader x) => DeserializeMethod<T, TValue>(ref x);
    }

    private static T? DeserializeMethod<T, TValue>(ref Utf8JsonReader reader)
        where T : ITypedValue, new()
    {
        var value = JsonSerializer.Deserialize<TValue>(ref reader);
        if (value != null)
            return TypedValueCreator<T, TValue>.Create(value);
        return default;
    }
}