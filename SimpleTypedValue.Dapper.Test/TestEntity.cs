namespace SimpleTypedValue.Dapper.Test;

[TypedValue(TypedValueExtensionFlag.Dapper)]
public partial struct TestEntityId : ITypedValue<long>
{
    public long Value { get; }
}

public class TestEntity
{
    public TestEntityId Id { get; set; }
    public string? Text { get; set; }
}