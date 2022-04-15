namespace SimpleTypedValue.EF.Test;

[TypedValue]
public readonly partial struct TestEntityId : ITypedValue<long>
{
    public long Value { get; }
}

public class TestEntity
{
    public TestEntityId Id { get; set; }
    public string? Text { get; set; }
}