using Microsoft.CodeAnalysis;

namespace SimpleTypedValue.Generator;

public class FileGeneratorContext
{
    private readonly SourceProductionContext _context;
    public INamedTypeSymbol Type { get; }

    public FileGeneratorContext(SourceProductionContext context, INamedTypeSymbol type)
    {
        _context = context;
        Type = type;
    }

    public void Error(string message)
    {
        var descriptor = new DiagnosticDescriptor("none", "SimpleTypeValueIncrementalGenerator", message, "None",
            DiagnosticSeverity.Error, true);
        var diagnostic = Diagnostic.Create(descriptor, Location.None, DiagnosticSeverity.Error);
        _context.ReportDiagnostic(diagnostic);
    }

    public void AddSource(string fileName, string source)
        => _context.AddSource(fileName, source);
}