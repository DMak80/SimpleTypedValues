using Microsoft.CodeAnalysis;

namespace SimpleTypedValue.Generator;

public enum DefaultConstructorTypeEnum
{
    SystemDefault,
    UseNew,
    MemberDefault,
}

[Flags]
public enum ConstructorDefinedEnum
{
    None = 0,
    Default = 1,
    Value = 2,
}

public class ConstructorsInfo
{
    public ConstructorsInfo(FileGeneratorContext context, bool isGenericType, ITypeSymbol valueType)
    {
        ConstructorDefined = CheckConstructorsDefined(context, valueType);

        if (!ConstructorDefined.HasFlag(ConstructorDefinedEnum.Default))
            DefaultConstructorType = CheckConstructors(context, isGenericType, valueType);
    }

    public DefaultConstructorTypeEnum DefaultConstructorType { get; }
    public ConstructorDefinedEnum ConstructorDefined { get; }

    private ConstructorDefinedEnum CheckConstructorsDefined(FileGeneratorContext context, ITypeSymbol valueType)
    {
        var result = ConstructorDefinedEnum.None;
        if (context.Type.Constructors.Any(x => !x.IsStatic
                                               && x.Parameters.Length == 0))
            result |= ConstructorDefinedEnum.Default;
        if (context.Type.Constructors.Any(x => !x.IsStatic
                                               && x.Parameters.Length == 1
                                               && x.Parameters[0].Type.ToDisplayString() == valueType.ToDisplayString()))
            result |= ConstructorDefinedEnum.Value;
        return result;
    }

    private DefaultConstructorTypeEnum CheckConstructors(FileGeneratorContext context, bool isGenericType,
        ITypeSymbol valueType)
        => isGenericType
            ? CheckGenericConstraints(context.Type, valueType)
            : HasDefaultValueForClass(context, valueType);

    private DefaultConstructorTypeEnum CheckGenericConstraints(INamedTypeSymbol type, ITypeSymbol typeSymbol)
    {
        if (type.TypeParameters.Length > 0)
            return ConvertGenericConstraint(type.TypeParameters[0]);

        if (type.ContainingType == null)
            return DefaultConstructorTypeEnum.SystemDefault;
        var baseType = type.ContainingType;

        if (!baseType.IsGenericType)
            return DefaultConstructorTypeEnum.SystemDefault;

        var constructorEnum = baseType.TypeParameters
            .Where(x => x.Name == typeSymbol.Name)
            .Select(ConvertGenericConstraint)
            .FirstOrDefault();

        return constructorEnum;
    }

    private DefaultConstructorTypeEnum ConvertGenericConstraint(ITypeParameterSymbol parameterSymbol)
        => parameterSymbol.HasValueTypeConstraint
            ? DefaultConstructorTypeEnum.SystemDefault
            : parameterSymbol.HasConstructorConstraint
                ? DefaultConstructorTypeEnum.UseNew
                : DefaultConstructorTypeEnum.MemberDefault;

    private DefaultConstructorTypeEnum HasDefaultValueForClass(FileGeneratorContext context, ITypeSymbol valueType)
    {
        if (valueType.IsValueType)
            return DefaultConstructorTypeEnum.SystemDefault;
        var members = context.Type.GetMembers("Default");
        var comparer = SymbolEqualityComparer.Default;

        var result = members
            .Any(x => x.IsStatic
                      && (x is IFieldSymbol fieldSymbol && comparer.Equals(fieldSymbol.Type, valueType)
                          || x is IPropertySymbol propSymbol && comparer.Equals(propSymbol.Type, valueType)));

        if (!result)
            context.Error(
                $"Class {context.Type.ToDisplayString()} should have static named 'Default' field or property with type {valueType.ToDisplayString()}");

        return DefaultConstructorTypeEnum.MemberDefault;
    }
}