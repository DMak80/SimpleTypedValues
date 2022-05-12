using SimpleTypedValue;

namespace ExampleProject;

[TypedValue]
public partial struct SomeId : ITypedValue<string>
{
    public static readonly string Default = string.Empty;
    public string Value { get; }
}

public class BaseEntity<TID>
    where TID : struct
{
    public TID Value { get; }
}

public partial class Organization : BaseEntity<Organization.ID>
{
    [TypedValue]
    public partial struct ID : ITypedValue<long>
    {
        public long Value { get; }
    }
}

public partial class User : BaseEntity<User.ID>
{
    [TypedValue]
    public partial struct ID : ITypedValue<long>
    {
        public long Value { get; }
    }
}