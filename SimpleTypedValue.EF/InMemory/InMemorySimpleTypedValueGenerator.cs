using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace SimpleTypedValue.EF;

public interface IResettableValueGenerator
{
    public void Reset();
}

public class InMemorySimpleTypedIntegerValueGenerator<T> : ValueGenerator<T>, IResettableValueGenerator
    where T : ITypedValue, new()
{
    private static readonly TypedValueInfo Info = TypedValue.GetInfo<T>()!;
    private long _value;

    public InMemorySimpleTypedIntegerValueGenerator()
    {
        if (!Info.InterfaceArgument.IsInteger())
            throw new ArgumentOutOfRangeException("type should have ITypedValue<_any integer_>");
    }

    public override bool GeneratesTemporaryValues => false;
    public override bool GeneratesStableValues => true;

    public void Reset()
    {
        _value = default;
    }

    public override T Next(EntityEntry entry)
    {
        var newValue = Interlocked.Increment(ref _value);
        var data = Convert.ChangeType(newValue, Info.InterfaceArgument);
        return TypedValueCreator<T>.Create(data);
    }
}