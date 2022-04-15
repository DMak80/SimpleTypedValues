using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace SimpleTypedValue.EF;

internal static class SimpleTypedValueGeneratorFactory<T>
{
    private static readonly Lazy<Func<ValueGenerator, ValueGenerator>> Constructor = new(() =>
    {
        var method = typeof(SimpleTypedValueGeneratorFactory<T>)
            .GetMethod("Create", BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(typeof(T), Info!.InterfaceArgument);
        return p => ((ValueGenerator?)method.Invoke(null, new object?[] { p }))!;
    });

    private static TypedValueInfo Info => TypedValue.GetInfo(typeof(T))!;

    // ReSharper disable once UnusedMember.Global
    public static ValueGenerator Build(Func<IProperty, IEntityType, ValueGenerator> builder, IProperty property,
        IEntityType entityType)
    {
        var newProperty = new PropertyProxy(property, Info!.InterfaceArgument);
        var generator = Info!.InterfaceArgument.IsNumeric()
            ? new TemporaryNumberValueGeneratorFactory().Create(newProperty, entityType)
            : builder(newProperty, entityType);
        return Constructor.Value(generator);
    }

    // ReSharper disable once UnusedMember.Global
    private static ValueGenerator Create<TT, TValue>(ValueGenerator generator)
        where TT : ITypedValue<TValue>, new()
        where TValue : notnull, IComparable<TValue>, IEquatable<TValue>
    {
        return new SimpleTypedValueGeneratorProxy<TT, TValue>(generator);
    }
}