namespace SimpleTypedValue.EF;

internal static class Types
{
    public static bool IsNumeric(this Type type)
    {
        type = type.UnwrapNullableType();

        return type.IsInteger()
               || type == typeof(decimal)
               || type == typeof(float)
               || type == typeof(double);
    }

    public static bool IsInteger(this Type type)
    {
        type = type.UnwrapNullableType();

        return type == typeof(int)
               || type == typeof(long)
               || type == typeof(short)
               || type == typeof(byte)
               || type == typeof(uint)
               || type == typeof(ulong)
               || type == typeof(ushort)
               || type == typeof(sbyte)
               || type == typeof(char);
    }

    public static Type UnwrapNullableType(this Type type)
    {
        return Nullable.GetUnderlyingType(type) ?? type;
    }
}