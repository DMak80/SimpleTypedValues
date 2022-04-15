using SimpleTypedValue;

[TypedValue]
public partial struct SomeIdTypedStringNullable : ITypedValue<string>
{
    public static readonly string Default = string.Empty;
    public string Value { get; }
}

[TypedValue]
public partial struct SomeIdTypedStringNullableProp : ITypedValue<string>
{
    public static string Default => string.Empty;
    public string Value { get; }
}