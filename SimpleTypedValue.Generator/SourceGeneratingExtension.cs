using System.Text;

namespace SimpleTypedValue.Generator;

internal static class SourceGeneratingExtension
{
    public static StringBuilder AddIf(this StringBuilder sb, bool predicate, Action<StringBuilder> action)
    {
        if (predicate)
            action(sb);
        return sb;
    }

    public static StringBuilder AppendLineIf(this StringBuilder sb, bool predicate, string value)
    {
        if (predicate)
            sb.AppendLine(value);
        return sb;
    }
}