using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleTypedValue.EF.Test;

public class BaseEntity<TID>
    where TID : struct
{
    public TID Id { get; set; }
}

public partial class TestEntity2 : BaseEntity<TestEntity2.ID>
{
    [TypedValue]
    public partial struct ID : ITypedValue<long>
    {
        public long Value { get; }
    }

    public string? Text2 { get; set; }

    public TestEntityId TestEntityId { get; set; }

    [ForeignKey(nameof(TestEntityId))]
    public virtual TestEntity? TestEntity { get; set; }
}

public partial class TestEntity3 : BaseEntity<TestEntity3.ID>
{
    [TypedValue]
    public partial struct ID : ITypedValue<long>
    {
        public long Value { get; }
    }
}