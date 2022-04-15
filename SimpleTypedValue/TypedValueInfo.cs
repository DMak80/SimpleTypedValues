using System;

namespace SimpleTypedValue;

public abstract class TypedValueInfo
{
    private object? _default;

    protected TypedValueInfo(Type @interface, Type interfaceArgument)
    {
        Interface = @interface;
        InterfaceArgument = interfaceArgument;
        DefaultValueInstance = interfaceArgument == typeof(string)
            ? string.Empty
            : Activator.CreateInstance(interfaceArgument)!;
    }

    public Type Interface { get; }
    public Type InterfaceArgument { get; }
    public object Default => _default ??= Create(DefaultValueInstance);
    public object DefaultValueInstance { get; }

    public object Create(object value)
    {
        return Create((dynamic)value);
    }

    public abstract object Create<TValue>(TValue value);
}

internal sealed class TypedValueInfo<T> : TypedValueInfo
    where T : ITypedValue, new()
{
    public TypedValueInfo(Type @interface, Type interfaceArg)
        : base(@interface, interfaceArg)
    {
    }

    public override object Create<TValue>(TValue value)
    {
        return TypedValueCreator<T, TValue>.Create(value);
    }
}