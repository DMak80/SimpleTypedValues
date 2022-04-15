using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace SimpleTypedValue.EF;

public class InMemorySimpleTypedIntegerValueGeneratorFactory : ValueGeneratorFactory
{
    private readonly ConcurrentDictionary<(Type, Type), ValueGenerator> _cache = new();

    public override ValueGenerator Create(IProperty property, IEntityType entityType)
    {
        return _cache.GetOrAdd((entityType.ClrType, property.ClrType), tuple =>
        {
            var info = TypedValue.GetInfo(tuple.Item2) ?? throw new ArgumentOutOfRangeException();
            return (ValueGenerator)Activator.CreateInstance(
                typeof(InMemorySimpleTypedIntegerValueGenerator<>).MakeGenericType(tuple.Item2))!;
        });
    }

    public void Reset()
    {
        foreach (var valueGenerator in _cache.Values)
            if (valueGenerator is IResettableValueGenerator resettable)
                resettable.Reset();
    }
}