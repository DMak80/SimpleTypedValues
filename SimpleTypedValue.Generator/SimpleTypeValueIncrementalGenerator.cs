using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SimpleTypedValue.Generator;

[Generator]
public class SimpleTypeValueIncrementalGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider.CreateSyntaxProvider(
                (x, ct) => x is StructDeclarationSyntax,
                (ctx, ct) =>
                {
                    var node = (StructDeclarationSyntax)ctx.Node;
                    var type = (INamedTypeSymbol)ctx.SemanticModel.GetDeclaredSymbol(node)!;
                    return HasAttributeAndInterface(type) ? type : null;
                }
            )
            .Where(x => x != null);

        context.RegisterSourceOutput(provider, FileGeneratorRunner);
    }

    private void FileGeneratorRunner(SourceProductionContext productionContext, INamedTypeSymbol? typeSymbol)
        => new FileGenerator(productionContext, typeSymbol!).Run();

    private static bool HasAttributeAndInterface(INamedTypeSymbol type)
    {
        var hasAttr = type.IsValueType
                      && FileGenerator.GetAttributeData(type) != null
                      && FileGenerator.GetInterface(type) != null;
        return hasAttr;
    }
}