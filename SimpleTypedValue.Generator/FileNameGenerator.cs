using System.Text;
using Microsoft.CodeAnalysis;

namespace SimpleTypedValue.Generator;

public class FileNameGenerator
{
    public FileNameGenerator(INamedTypeSymbol type)
    {
        FileName = GetFileName(type);
    }

    public string FileName { get; }
    private static string GetFileName(INamedTypeSymbol type)
    {
        var sb = new StringBuilder();
        if (type.ContainingType != null)
        {
            sb
                .Append(type.ContainingType.Name)
                .Append(".");
            if (type.ContainingType.TypeArguments.Length > 0)
            {
                foreach (var argument in type.ContainingType.TypeArguments)
                {
                    sb.Append(argument.Name)
                        .Append(".");
                }
            }
        }

        sb
            .Append(type.Name)
            .Append(".");

        if (type.TypeArguments.Length > 0)
        {
            foreach (var argument in type.TypeArguments)
            {
                sb.Append(argument.Name)
                    .Append(".");
            }
        }

        return sb.Append("Gen.cs").ToString();
    }
}