using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace SimpleTypedValue.EF;

public class SimpleTypedValueGeneratorProxy<T, TValue> : ValueGenerator<T>
    where T : ITypedValue<TValue>, new()
    where TValue : notnull, IComparable<TValue>, IEquatable<TValue>
{
    private static readonly TypedValueInfo Info = TypedValue.GetInfo<T>()!;
    private readonly ValueGenerator _generator;

    public SimpleTypedValueGeneratorProxy(ValueGenerator generator)
    {
        _generator = generator;
    }

    public override bool GeneratesTemporaryValues => _generator.GeneratesTemporaryValues;
    public override bool GeneratesStableValues => _generator.GeneratesStableValues;

    public override T Next(EntityEntry entry)
    {
        return TypedValueCreator<T, TValue>.Create((TValue)_generator.Next(entry)!);
    }
}