using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace SimpleTypedValue.EF;

public class PropertyProxy : IProperty
{
    private readonly IProperty _property;

    public PropertyProxy(IProperty property, Type clrType)
    {
        _property = property;
        ClrType = clrType;
    }

    public IAnnotation? FindAnnotation(string name)
    {
        return _property.FindAnnotation(name);
    }

    public IEnumerable<IAnnotation> GetAnnotations()
    {
        return _property.GetAnnotations();
    }

    public object? this[string name] => _property[name];

    public PropertyAccessMode GetPropertyAccessMode()
    {
        return _property.GetPropertyAccessMode();
    }

    public string Name => _property.Name;

    public IClrPropertyGetter GetGetter()
    {
        return _property.GetGetter();
    }

    public IComparer<IUpdateEntry> GetCurrentValueComparer()
    {
        return _property.GetCurrentValueComparer();
    }

    public IReadOnlyTypeBase DeclaringType => _property.DeclaringType;
    public Type ClrType { get; }

    public PropertyInfo? PropertyInfo => _property.PropertyInfo;
    public FieldInfo? FieldInfo => _property.FieldInfo;

    public CoreTypeMapping? FindTypeMapping()
    {
        return _property.FindTypeMapping();
    }

    public int? GetMaxLength()
    {
        return _property.GetMaxLength();
    }

    public int? GetPrecision()
    {
        return _property.GetPrecision();
    }

    public int? GetScale()
    {
        return _property.GetScale();
    }

    public bool? IsUnicode()
    {
        return _property.IsUnicode();
    }

    public PropertySaveBehavior GetBeforeSaveBehavior()
    {
        return _property.GetBeforeSaveBehavior();
    }

    public PropertySaveBehavior GetAfterSaveBehavior()
    {
        return _property.GetAfterSaveBehavior();
    }

    public Func<IProperty, IEntityType, ValueGenerator>? GetValueGeneratorFactory()
    {
        return _property.GetValueGeneratorFactory();
    }

    public ValueConverter? GetValueConverter()
    {
        return _property.GetValueConverter();
    }

    public Type? GetProviderClrType()
    {
        return _property.GetProviderClrType();
    }


    ValueComparer IProperty.GetValueComparer()
    {
        return _property.GetValueComparer();
    }

    ValueComparer IProperty.GetKeyValueComparer()
    {
        return _property.GetKeyValueComparer();
    }

    public IEntityType DeclaringEntityType => _property.DeclaringEntityType;

    public IEnumerable<IKey> GetContainingKeys()
    {
        return _property.GetContainingKeys();
    }

    ValueComparer? IReadOnlyProperty.GetValueComparer()
    {
        return _property.GetValueComparer();
    }

    ValueComparer? IReadOnlyProperty.GetKeyValueComparer()
    {
        return _property.GetKeyValueComparer();
    }

    public bool IsForeignKey()
    {
        return _property.IsForeignKey();
    }

    public IEnumerable<IForeignKey> GetContainingForeignKeys()
    {
        return _property.GetContainingForeignKeys();
    }

    public IEnumerable<IIndex> GetContainingIndexes()
    {
        return _property.GetContainingIndexes();
    }

    IEnumerable<IReadOnlyForeignKey> IReadOnlyProperty.GetContainingForeignKeys()
    {
        return _property.GetContainingForeignKeys();
    }

    public bool IsIndex()
    {
        return _property.IsIndex();
    }

    IEnumerable<IReadOnlyIndex> IReadOnlyProperty.GetContainingIndexes()
    {
        return _property.GetContainingIndexes();
    }

    public IReadOnlyKey? FindContainingPrimaryKey()
    {
        return _property.FindContainingPrimaryKey();
    }

    public bool IsKey()
    {
        return _property.IsKey();
    }

    IEnumerable<IReadOnlyKey> IReadOnlyProperty.GetContainingKeys()
    {
        return _property.GetContainingKeys();
    }

    IReadOnlyEntityType IReadOnlyProperty.DeclaringEntityType => DeclaringEntityType;

    public bool IsNullable => _property.IsNullable;
    public ValueGenerated ValueGenerated => _property.ValueGenerated;
    public bool IsConcurrencyToken => _property.IsConcurrencyToken;

    public IAnnotation? FindRuntimeAnnotation(string name)
    {
        return _property.FindRuntimeAnnotation(name);
    }

    public IEnumerable<IAnnotation> GetRuntimeAnnotations()
    {
        return _property.GetRuntimeAnnotations();
    }

    public IAnnotation AddRuntimeAnnotation(string name, object? value)
    {
        return _property.AddRuntimeAnnotation(name, value);
    }

    public IAnnotation SetRuntimeAnnotation(string name, object? value)
    {
        return _property.SetRuntimeAnnotation(name, value);
    }

    public IAnnotation? RemoveRuntimeAnnotation(string name)
    {
        return _property.RemoveRuntimeAnnotation(name);
    }

    public TValue GetOrAddRuntimeAnnotationValue<TValue, TArg>(string name, Func<TArg?, TValue> valueFactory,
        TArg? factoryArgument)
    {
        return _property.GetOrAddRuntimeAnnotationValue(name, valueFactory, factoryArgument);
    }
}