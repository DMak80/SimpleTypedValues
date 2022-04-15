namespace SimpleTypedValue.Generator;

public class Templates
{
    private readonly string _namespace;
    private readonly string _className;
    private readonly string _parentClassName;
    private readonly string _valueType;
    private readonly string _constructorName;

    public Templates(string typeNamespace, string typeName, string valueType, string parentClassName)
    {
        _namespace = typeNamespace;
        _parentClassName = parentClassName;
        _className = typeName;
        _valueType = valueType;
        _constructorName = _className.EndsWith($"<{valueType}>")
            ? _className[..^(valueType.Length + 2)]
            : _className;
    }

    private string Formatted(string template)
        => template
            .Replace("$$CLASS$$", _className)
            .Replace("$$NAMESPACE$$", _namespace)
            .Replace("$$VALUECLASS$$", _valueType)
            .Replace("$$PARENTCLASS$$", _parentClassName)
            .Replace("$$CONSTRUCTOR$$", _constructorName);

    public string Namespace => _namespace;
    public string UsingDapperNamespace => _usingDapperNamespace;
    public string Usings => _usings;
    public string StartNamespace => Formatted(_startNamespace);
    public string StartStruct => Formatted(_startStruct);

    public string StartClass => Formatted(_startClass);
    public string End => @"}";

    public string AttributeConverter => Formatted(_attributeConverter);
    public string AttributeNewtonsoftJson => Formatted(_attributeNewtonsoftJson);
    public string AttributeJson => Formatted(_attributeJson);
    public string Funcs => Formatted(_funcs);
    public string StartStaticConstructor => Formatted(_staticConstructor);
    public string StaticConstructorConverter => Formatted(_staticConstructorConverter);
    public string StaticConstructorNewtonsoftJson => Formatted(_staticConstructorNewtonsoftJson);
    public string StaticConstructorJson => Formatted(_staticConstructorJson);
    public string StaticConstructorDapper => Formatted(_staticConstructorDapper);
    public string ConstructorDefault => Formatted(_constructorDefault);
    public string ConstructorValue => Formatted(_constructorValue);
    public string ConstructorDefaultWithDefault => Formatted(_constructorDefaultWithDefault);
    public string ConstructorDefaultWithNew => Formatted(_constructorDefaultWithNew);

    private const string _usingDapperNamespace = @"using SimpleTypedValue.Dapper;";

    private const string _usings = @"
using System;
    ";
    private const string _startNamespace = @"
namespace $$NAMESPACE$$ {";
    private const string _startStruct = @"public partial struct $$CLASS$$ : IComparable<$$CLASS$$>, IEquatable<$$CLASS$$> {";
    private const string _startClass = @"public partial class $$PARENTCLASS$$ {";

    private const string _attributeConverter = @"
[System.ComponentModel.TypeConverter(typeof(SimpleTypedValue.TypedValueConverter<$$CLASS$$>))]
";

    private const string _attributeNewtonsoftJson = @"
[Newtonsoft.Json.JsonConverter(typeof(SimpleTypedValue.NewtonsoftJson.TypedValueJsonConverter<$$CLASS$$>))]
";

    private const string _attributeJson = @"
[System.Text.Json.Serialization.JsonConverter(typeof(SimpleTypedValue.Json.TypedValueJsonConverter<$$CLASS$$>))]
";

    private const string _staticConstructor = @"
    static $$CONSTRUCTOR$$()
    {
";

    private const string _staticConstructorConverter = @"
    System.ComponentModel.TypeDescriptor.AddAttributes(new System.ComponentModel.TypeConverterAttribute(
            typeof(SimpleTypedValue.TypedValueConverter<$$CLASS$$>)
        ));
";

    private const string _staticConstructorNewtonsoftJson = @"
    System.ComponentModel.TypeDescriptor.AddAttributes(new Newtonsoft.Json.JsonConverter(
            typeof(SimpleTypedValue.NewtonsoftJson.TypedValueJsonConverter<$$CLASS$$>)
        ));
";

    private const string _staticConstructorJson = @"
    System.ComponentModel.TypeDescriptor.AddAttributes(new System.Text.Json.Serialization.JsonConverter(
            typeof(SimpleTypedValue.Json.TypedValueJsonConverter<$$CLASS$$>)
        ));
";

    private const string _staticConstructorDapper = @"
        typeof($$CLASS$$).AddDapperTypedValueHandler();
";

    private const string _constructorDefault = @"
    public $$CONSTRUCTOR$$()
    {
        Value = default;
    }
";
    private const string _constructorValue = @"
    public $$CONSTRUCTOR$$($$VALUECLASS$$ value)
    {
        Value = value;
    }
";

    private const string _constructorDefaultWithNew = @"
    public $$CONSTRUCTOR$$()
    {
        Value = new $$VALUECLASS$$();
    }
";

    private const string _constructorDefaultWithDefault = @"
    public $$CONSTRUCTOR$$()
    {
        Value = Default;
    }
";

    private const string _funcs = @"
    public int CompareTo($$CLASS$$ other) => Value.CompareTo(other.Value);

    public bool Equals($$CLASS$$ other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is $$CLASS$$ other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==($$CLASS$$ a, $$CLASS$$ b)
    {
        return a.Equals(b);
    }

    public static bool operator !=($$CLASS$$ a, $$CLASS$$ b)
    {
        return !a.Equals(b);
    }
    ";
}