namespace SimpleTypedValue.AspNet.Test;

[TypedValue(TypedValueExtensionFlag.Json | TypedValueExtensionFlag.NewtonsoftJson)]
public partial struct LongTypedValue : ITypedValue<long>
{
    public long Value { get; }
}