using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleTypedValue.Json;

public class TypedValueJsonConverter<T> : JsonConverter<T>
    where T : ITypedValue, new()
{
    private static readonly Action<Utf8JsonWriter, T, JsonSerializerOptions> Serializer = SerializerFactory.Build<T>();
    private static readonly DeserializerMethod<T> Deserializer = DeserializerFactory.Build<T>();

    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Deserializer(ref reader);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        Serializer(writer, value, options);
    }
}