using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace SimpleTypedValue.EF;

public static class DbContextExtension
{
    private static readonly ConcurrentDictionary<Type, ValueConverter> Converters = new();

    private static readonly MethodInfo GetConvertersMethodInfo = typeof(DbContextExtension)
        .GetMethod(nameof(GetConverters), BindingFlags.Static | BindingFlags.NonPublic)!;

    private static readonly Lazy<InMemorySimpleTypedIntegerValueGeneratorFactory> _factory =
        new(() => new InMemorySimpleTypedIntegerValueGeneratorFactory());

    private static bool _isInited;

    public static void ResetInMemorySimpleTypedIntegerValueGeneratorFactory()
    {
        _factory.Value.Reset();
    }

    public static void UseTypedValues(this DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ReplaceService<IValueGeneratorSelector, SimpleTypedValueGeneratorSelector>();
    }

    public static void UseTypedValues(this ModelBuilder modelBuilder, bool useInMemory = false)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        foreach (var property in entityType.GetProperties())
        {
            var obj = _isInited && Converters.TryGetValue(property.ClrType, out var tobj)
                ? tobj
                : CacheAndGetValueConverter(property.ClrType);

            if (obj == null)
                continue;

            property.SetValueConverter(obj);

            if (useInMemory) property.SetValueGeneratorFactory((prop, entity) => _factory.Value.Create(prop, entity));
        }

        _isInited = true;
    }

    private static ValueConverter? CacheAndGetValueConverter(Type type)
    {
        var info = TypedValue.GetInfo(type);
        if (info == null)
            return null;

        return Converters.GetOrAdd(type, t =>
        {
            var method = GetConvertersMethodInfo.MakeGenericMethod(t, info.InterfaceArgument);
            return (ValueConverter)method.Invoke(null, null)!;
        });
    }

    private static ValueConverter<TModel, TProvider> GetConverters<TModel, TProvider>()
        where TModel : ITypedValue<TProvider>, new()
        where TProvider : notnull, IComparable<TProvider>, IEquatable<TProvider>
    {
        var info = TypedValue.GetInfo<TModel>()!;
        return new ValueConverter<TModel, TProvider>(
            x => x.Value,
            x => TypedValueCreator<TModel, TProvider>.Create(x)
        );
    }
}