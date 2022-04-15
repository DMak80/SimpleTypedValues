using System.Text;
using Microsoft.CodeAnalysis;

namespace SimpleTypedValue.Generator;

public class FileGenerator
{
    private readonly FileGeneratorContext _context;
    private readonly bool _hasParentClassName;
    private readonly bool _useAttributes;
    private readonly ConstructorsInfo _constructorsInfo;
    private readonly FileNameGenerator _fileNameGenerator;
    private readonly Templates _templates;

    public FileGenerator(SourceProductionContext sourceProductionContext, INamedTypeSymbol type)
    {
        _context = new FileGeneratorContext(sourceProductionContext, type);

        var isGenericType = (_context.Type.ContainingType?.IsGenericType ?? false) || _context.Type.IsGenericType;
        _useAttributes = !isGenericType;

        var valueType = GetInterface(_context.Type)!.TypeArguments[0];
        var valueTypeName = _context.Type.IsGenericType
            ? valueType.Name
            : $"{valueType.ContainingNamespace.ToDisplayString()}.{valueType.Name}";

        _constructorsInfo = new ConstructorsInfo(_context, isGenericType, valueType);
        _fileNameGenerator = new FileNameGenerator(_context.Type);

        var typeNamespace = _context.Type.ContainingNamespace.ToDisplayString();
        if (typeNamespace == "<global namespace>")
            typeNamespace = string.Empty;

        var parentClassName = _context.Type.ContainingType?.ToDisplayString()[(typeNamespace.Length + 1)..] ??
                              String.Empty;
        var typeName = _context.Type.ToDisplayString();
        typeName = typeName.StartsWith(typeNamespace + ".")
            ? typeName[(typeNamespace.Length + 1)..]
            : typeName;
        typeName = typeName.StartsWith(parentClassName + ".")
            ? typeName[(parentClassName.Length + 1)..]
            : typeName;

        _hasParentClassName = !string.IsNullOrEmpty(parentClassName);
        _templates = new Templates(typeNamespace, typeName, valueTypeName, parentClassName);
    }

    public void Run()
    {
        var source = GetSource();

        _context.AddSource(_fileNameGenerator.FileName, source);

        // var descriptor = new DiagnosticDescriptor("id", "SimpleTypeValueIncrementalGenerator",
        //     $"Type: {typeName} valueType: {valueType}", "CA",
        //     DiagnosticSeverity.Info, true);
        // var diagnostic = Diagnostic.Create(descriptor, Location.None, DiagnosticSeverity.Info);
        // _context.ReportDiagnostic(diagnostic);
    }

    public static INamedTypeSymbol? GetInterface(INamedTypeSymbol typeSymbol)
        => typeSymbol.Interfaces
            .FirstOrDefault(x => x.IsGenericType
                                 && x.MetadataName.StartsWith("ITypedValue`")
                                 && x.ContainingNamespace.MetadataName == "SimpleTypedValue");

    public static AttributeData? GetAttributeData(INamedTypeSymbol typeSymbol)
        => typeSymbol.GetAttributes()
            .FirstOrDefault(x => x.AttributeClass?.Name == "TypedValueAttribute");

    private static TypedValueExtensionFlag GetAttributeValue(INamedTypeSymbol type)
        => (TypedValueExtensionFlag)GetAttributeData(type)!.ConstructorArguments[0].Value!;

    private string GetSource()
    {
        var attributeFlags = GetAttributeValue(_context.Type);
        var source = AddSource(new StringBuilder(), _templates, _hasParentClassName,
                _constructorsInfo, _useAttributes, attributeFlags)
            .ToString();

        return source;
    }

    internal static StringBuilder AddSource(StringBuilder sb, Templates templates, bool hasParentClassName,
        ConstructorsInfo constructorsInfo, bool useAttributes, TypedValueExtensionFlag attributeFlags)
    {
        var hasDapper = attributeFlags.HasFlag(TypedValueExtensionFlag.Dapper);
        return sb
            .AddIf(hasDapper, x => x.AppendLine(templates.UsingDapperNamespace))
            .AppendLine(templates.Usings)
            .AppendLineIf(!string.IsNullOrEmpty(templates.Namespace), templates.StartNamespace)
            .AppendLineIf(hasParentClassName, templates.StartClass)
            .AddIf(useAttributes, x => AddAttributes(x, templates, attributeFlags))
            .AppendLine(templates.StartStruct)
            .AddIf(!useAttributes || hasDapper,
                x => AddStaticConstructor(x, templates, useAttributes, attributeFlags))
            .AppendLineIf(!constructorsInfo.ConstructorDefined.HasFlag(ConstructorDefinedEnum.Default),
                constructorsInfo.DefaultConstructorType switch
                {
                    DefaultConstructorTypeEnum.SystemDefault => templates.ConstructorDefault,
                    DefaultConstructorTypeEnum.MemberDefault => templates.ConstructorDefaultWithDefault,
                    DefaultConstructorTypeEnum.UseNew => templates.ConstructorDefaultWithNew,
                    _ => throw new ArgumentOutOfRangeException(nameof(constructorsInfo.ConstructorDefined),
                        constructorsInfo.ConstructorDefined, null)
                })
            .AppendLineIf(!constructorsInfo.ConstructorDefined.HasFlag(ConstructorDefinedEnum.Value),
                templates.ConstructorValue)
            .AppendLine(templates.Funcs)
            .AppendLine(templates.End) //end struct
            .AppendLineIf(hasParentClassName, templates.End) // end class
            .AppendLineIf(!string.IsNullOrEmpty(templates.Namespace), templates.End); //end namespace
    }

    internal static StringBuilder AddAttributes(StringBuilder sb, Templates templates,
        TypedValueExtensionFlag typedValues)
        => sb
            .AppendLineIf(typedValues.HasFlag(TypedValueExtensionFlag.Converters),
                templates.AttributeConverter)
            .AppendLineIf(typedValues.HasFlag(TypedValueExtensionFlag.Json),
                templates.AttributeJson)
            .AppendLineIf(typedValues.HasFlag(TypedValueExtensionFlag.NewtonsoftJson),
                templates.AttributeNewtonsoftJson);


    internal static StringBuilder AddStaticConstructor(StringBuilder sb, Templates templates, bool useAttributes,
        TypedValueExtensionFlag typedValues)
        => sb
            .AppendLine(templates.StartStaticConstructor)
            .AppendLineIf(useAttributes && typedValues.HasFlag(TypedValueExtensionFlag.Converters),
                templates.StaticConstructorConverter)
            .AppendLineIf(useAttributes && typedValues.HasFlag(TypedValueExtensionFlag.Json),
                templates.StaticConstructorJson)
            .AppendLineIf(useAttributes && typedValues.HasFlag(TypedValueExtensionFlag.NewtonsoftJson),
                templates.StaticConstructorNewtonsoftJson)
            .AppendLineIf(typedValues.HasFlag(TypedValueExtensionFlag.Dapper), templates.StaticConstructorDapper)
            .AppendLine(templates.End);
}