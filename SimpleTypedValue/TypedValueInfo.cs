using System;

namespace SimpleTypedValue;

public abstract class TypedValueInfo
{
    protected TypedValueInfo(Type @interface, Type interfaceArgument, ITypedValue obj)
    {
        Interface = @interface;
        InterfaceArgument = interfaceArgument;
        Default = obj; 
        DefaultValueInstance = obj.Value ?? throw new ArgumentException();
    }

    public Type Interface { get; }
    public Type InterfaceArgument { get; }
    public object Default { get; }
    public object DefaultValueInstance { get; }
}

internal sealed class TypedValueInfo<T> : TypedValueInfo
    where T : ITypedValue, new()
{
    public TypedValueInfo(Type @interface, Type interfaceArg)
        : base(@interface, interfaceArg, new T())
    {
    }
}