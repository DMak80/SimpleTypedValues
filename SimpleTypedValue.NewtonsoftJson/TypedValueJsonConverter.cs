using System.ComponentModel;
using Newtonsoft.Json;
using SimpleTypedValue.Json;

namespace SimpleTypedValue.NewtonsoftJson;

public class TypedValueJsonConverter<T> : JsonConverter<T>
    where T : ITypedValue, new()
{
    private static readonly DeserializerMethod<T> Deserializer = DeserializerFactory.Build<T>();

    public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value?.Value);
    }

    public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (hasExistingValue)
            return existingValue;
        return Deserializer(reader, serializer);
    }
}