using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SimpleTypedValue;

public static class TypedValueCreator<T>
    where T : ITypedValue, new()
{
    private static readonly TypedValueInfo Info = TypedValue.GetInfo<T>()
                                                  ?? throw new ArgumentOutOfRangeException(
                                                      "Argument should implement ITypedValue<T>");

    private static readonly Func<object, T> Func = CreateTypedValueCreator((dynamic)Info.DefaultValueInstance);

    public static T Create(object obj)
        => Func(obj);

    private static Func<object, T> CreateTypedValueCreator<TValue>(TValue val)
        => x => TypedValueCreator<T, TValue>.Create((TValue)x);
}

public static class TypedValueCreator<T, TValue>
    where T : ITypedValue, new()
{
    public delegate TT ObjectActivator<out TT>(TValue args);

    public static readonly ObjectActivator<T> Create = GetActivator(GetConstructorInfo());

    private static ConstructorInfo GetConstructorInfo()
    {
        var info = TypedValue.GetInfo<T>()!;
        var constructor = typeof(T).GetConstructors()
            .FirstOrDefault(c =>
            {
                var ctrParams = c.GetParameters();
                return ctrParams.Length == 1 && ctrParams[0].ParameterType == info.InterfaceArgument;
            });

        return constructor ?? throw new InvalidOperationException(
            $"Constructor ({typeof(TValue).Name}) is not found for {typeof(T)}");
    }

    private static ObjectActivator<T> GetActivator(ConstructorInfo ctor)
    {
        var paramsInfo = ctor.GetParameters();
        var param = Expression.Parameter(typeof(TValue), "args");
        var paramType = paramsInfo[0].ParameterType;
        Expression paramCastExp = Expression.Convert(param, paramType);
        var newExp = Expression.New(ctor, paramCastExp);

        //create a lambda with the New
        //Expression as body and our param object[] as arg
        var lambda = Expression.Lambda(typeof(ObjectActivator<T>), newExp, param);

        //compile it
        var compiled = (ObjectActivator<T>)lambda.Compile();

        return compiled;
    }
}