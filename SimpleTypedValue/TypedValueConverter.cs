using System;
using System.ComponentModel;
using System.Globalization;

namespace SimpleTypedValue;

public class TypedValueConverter<T> : TypeConverter
    where T : struct, ITypedValue
{
    private static readonly TypedValueInfo? Info = TypedValue.GetInfo<T>();

    private readonly TypeConverter? _converter = Info == null
        ? null
        : TypeDescriptor.GetConverter(Info.InterfaceArgument);

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value,
        Type destinationType)
    {
        if (value == null)
            return null;

        if (destinationType == Info!.InterfaceArgument)
            return ((T)value).Value;

        return _converter!.CanConvertTo(destinationType)
            ? _converter.ConvertTo(context, culture, ((T)value).Value, destinationType)
            : TypeDescriptor.GetConverter(destinationType).ConvertFrom(context, culture, ((T)value).Value!);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value.GetType() == Info!.InterfaceArgument) 
            return TypedValueCreator<T>.Create(value);

        var newValue = _converter!.CanConvertFrom(context, value.GetType())
            ? _converter.ConvertFrom(context, culture, value)
            : TypeDescriptor.GetConverter(value.GetType()).ConvertTo(context, culture, value, Info.InterfaceArgument);

        if (newValue != null)
            return TypedValueCreator<T>.Create(newValue);

        return null;
    }

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return Info != null
               && (sourceType == Info.InterfaceArgument
                   || _converter!.CanConvertFrom(context, sourceType)
                   || TypeDescriptor.GetConverter(sourceType).CanConvertTo(context, Info.InterfaceArgument));
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return Info != null
               && destinationType != null
               && (destinationType == Info.InterfaceArgument
                   || _converter!.CanConvertTo(context, destinationType)
                   || TypeDescriptor.GetConverter(destinationType).CanConvertTo(context, Info.InterfaceArgument));
    }
}