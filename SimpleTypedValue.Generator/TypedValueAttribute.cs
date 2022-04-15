namespace SimpleTypedValue.Generator;

[Flags]
internal enum TypedValueExtensionFlag
{
    None = 0,
    Converters = 1 << 0,
    Dapper = 1 << 1 | Converters,
    Json = 1 << 2 | Converters,
    NewtonsoftJson = 1 << 3 | Converters,
    EF6 = 1 << 4 | Converters,
}

internal class TypedValueAttribute : Attribute
{
}