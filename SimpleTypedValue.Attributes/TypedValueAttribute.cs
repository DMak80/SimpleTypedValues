namespace SimpleTypedValue;

[Flags]
public enum TypedValueExtensionFlag
{
    None = 0,
    Converters = 1 << 0,
    Dapper = 1 << 1 | Converters,
    Json = 1 << 2 | Converters,
    NewtonsoftJson = 1 << 3 | Converters,
    EF6 = 1 << 4 | Converters,
}

public class TypedValueAttribute : Attribute
{
    public TypedValueAttribute(TypedValueExtensionFlag extensions = TypedValueExtensionFlag.Converters)
    {
        Extensions = extensions;
    }

    public TypedValueExtensionFlag Extensions { get; }
}