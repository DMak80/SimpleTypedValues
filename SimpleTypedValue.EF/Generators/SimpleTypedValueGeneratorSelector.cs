using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace SimpleTypedValue.EF;

public class SimpleTypedValueGeneratorSelector : ValueGeneratorSelector
{
    private static readonly MethodInfo MethodCreate = typeof(SimpleTypedValueGeneratorSelector)
        .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
        .First(m => m.IsGenericMethod && m.Name.StartsWith(nameof(Create)));

    public SimpleTypedValueGeneratorSelector(ValueGeneratorSelectorDependencies dependencies) : base(dependencies)
    {
    }

    /// <summary>
    ///     Creates a new value generator for the given property.
    /// </summary>
    /// <param name="property">The property to get the value generator for.</param>
    /// <param name="entityType">
    ///     The entity type that the value generator will be used for. When called on inherited properties on derived entity
    ///     types,
    ///     this entity type may be different from the declared entity type on <paramref name="property" />
    /// </param>
    /// <returns>The newly created value generator.</returns>
    public override ValueGenerator Create(IProperty property, IEntityType entityType)
    {
        if (TypedValue.GetInfo(property.ClrType) == null) return base.Create(property, entityType);

        var generator = (ValueGenerator?)MethodCreate.MakeGenericMethod(property.ClrType)
            .Invoke(this, new object?[] { property, entityType })!;
        return generator;
    }

    private ValueGenerator Create<T>(IProperty property, IEntityType entityType)
    {
        return SimpleTypedValueGeneratorFactory<T>.Build(base.Create, property, entityType);
    }
}