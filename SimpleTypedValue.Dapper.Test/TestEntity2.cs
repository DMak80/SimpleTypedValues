using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleTypedValue.Dapper.Test;

[TypedValue(TypedValueExtensionFlag.Dapper)]
public partial struct TestEntity2Id : ITypedValue<long>
{
    public long Value { get; }
}

public class TestEntity2
{
    public TestEntity2Id Id { get; set; }
    public string? Text2 { get; set; }

    public TestEntityId TestEntityId { get; set; }

    [ForeignKey(nameof(TestEntityId))]
    public virtual TestEntity? TestEntity { get; set; }
}