using Newtonsoft.Json;

namespace SimpleTypedValue.Json;

delegate T? DeserializerMethod<T>(JsonReader reader, JsonSerializer serializer)
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
        return DeserializeMethod<T, TValue>;
    }

    private static T? DeserializeMethod<T, TValue>(JsonReader reader, JsonSerializer serializer)
        where T : ITypedValue, new()
    {
        var value = serializer.Deserialize<TValue>(reader);
        if (value != null)
            return TypedValueCreator<T, TValue>.Create(value);
        return default;
    }
}