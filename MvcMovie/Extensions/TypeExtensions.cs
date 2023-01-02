#region usings

using System.ComponentModel;
using System.Reflection;

#endregion

namespace MvcMovie.Extensions;
#nullable disable
public static class TypeExtensions
{
    public static readonly Type ObjectType = typeof(object);
    public static readonly Type StringType = typeof(string);
    public static readonly Type CharType = typeof(char);
    public static readonly Type NullableCharType = typeof(char?);
    public static readonly Type DateTimeType = typeof(DateTime);
    public static readonly Type NullableDateTimeType = typeof(DateTime?);
    public static readonly Type BoolType = typeof(bool);
    public static readonly Type NullableBoolType = typeof(bool?);
    public static readonly Type ByteArrayType = typeof(byte[]);
    public static readonly Type ByteType = typeof(byte);
    public static readonly Type SByteType = typeof(sbyte);
    public static readonly Type SingleType = typeof(float);
    public static readonly Type DecimalType = typeof(decimal);
    public static readonly Type Int16Type = typeof(short);
    public static readonly Type UInt16Type = typeof(ushort);
    public static readonly Type Int32Type = typeof(int);
    public static readonly Type UInt32Type = typeof(uint);
    public static readonly Type Int64Type = typeof(long);
    public static readonly Type UInt64Type = typeof(ulong);
    public static readonly Type DoubleType = typeof(double);


    /// <summary>
    ///     Will try and parse an enum and it's default type.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns>True if the enum value is defined.</returns>
    public static bool TryEnumIsDefined(Type type, object value)
    {
        if (type == null || value == null || !type.GetTypeInfo().IsEnum)
            return false;

        // Return true if the value is an enum and is a matching type.
        if (type == value.GetType())
            return true;

        if (TryEnumIsDefined<int>(type, value))
            return true;
        if (TryEnumIsDefined<string>(type, value))
            return true;
        if (TryEnumIsDefined<byte>(type, value))
            return true;
        if (TryEnumIsDefined<short>(type, value))
            return true;
        if (TryEnumIsDefined<long>(type, value))
            return true;
        if (TryEnumIsDefined<sbyte>(type, value))
            return true;
        if (TryEnumIsDefined<ushort>(type, value))
            return true;
        if (TryEnumIsDefined<uint>(type, value))
            return true;
        if (TryEnumIsDefined<ulong>(type, value))
            return true;

        return false;
    }

    public static bool TryEnumIsDefined<T>(Type type, object value)
    {
        // Catch any casting errors that can occur or if 0 is not defined as a default value.
        try
        {
            if (value is T variable && Enum.IsDefined(type, variable))
                return true;
        }
        catch (Exception)
        {
            // ignored
        }

        return false;
    }

    public static bool IsNumeric(this Type type)
    {
        if (type.IsArray)
            return false;

        if (type == ByteType ||
            type == DecimalType ||
            type == DoubleType ||
            type == Int16Type ||
            type == Int32Type ||
            type == Int64Type ||
            type == SByteType ||
            type == SingleType ||
            type == UInt16Type ||
            type == UInt32Type ||
            type == UInt64Type)
            return true;

        return Type.GetTypeCode(type) switch
        {
            TypeCode.Byte => true,
            TypeCode.Decimal => true,
            TypeCode.Double => true,
            TypeCode.Int16 => true,
            TypeCode.Int32 => true,
            TypeCode.Int64 => true,
            TypeCode.SByte => true,
            TypeCode.Single => true,
            TypeCode.UInt16 => true,
            TypeCode.UInt32 => true,
            TypeCode.UInt64 => true,
            _ => false
        };
    }

    public static bool IsNullableNumeric(this Type type)
    {
        if (type.IsArray)
            return false;

        var t = Nullable.GetUnderlyingType(type);
        return t != null && t.IsNumeric();
    }

    public static T ToType<T>(this object value)
    {
        Type targetType = typeof(T);

        if (value == null)
            try
            {
                return (T)Convert.ChangeType(null, targetType);
            }
            catch
            {
                throw new ArgumentNullException(nameof(value));
            }

        TypeConverter converter = TypeDescriptor.GetConverter(targetType);
        Type valueType = value.GetType();

        if (targetType.IsAssignableFrom(valueType))
            return (T)value;

        TypeInfo targetTypeInfo = targetType.GetTypeInfo();
        if (targetTypeInfo.IsEnum && (value is string || valueType.GetTypeInfo().IsEnum))
        {
            // attempt to match enum by name.
            if (TryEnumIsDefined(targetType, value.ToString()))
            {
                object parsedValue = Enum.Parse(targetType, value.ToString(), false);
                return (T)parsedValue;
            }

            string message =
                $"The Enum value of '{value}' is not defined as a valid value for '{targetType.FullName}'.";
            throw new ArgumentException(message);
        }

        if (targetTypeInfo.IsEnum && valueType.IsNumeric())
            return (T)Enum.ToObject(targetType, value);

        if (converter.CanConvertFrom(valueType))
        {
            object convertedValue = converter.ConvertFrom(value);
            return (T)convertedValue;
        }

        if (!(value is IConvertible))
            throw new ArgumentException(
                $"An incompatible value specified.  Target Type: {targetType.FullName} Value Type: {value.GetType().FullName}",
                nameof(value));
        try
        {
            object convertedValue = Convert.ChangeType(value, targetType);
            return (T)convertedValue;
        }
        catch (Exception e)
        {
            throw new ArgumentException(
                $"An incompatible value specified.  Target Type: {targetType.FullName} Value Type: {value.GetType().FullName}",
                nameof(value), e);
        }
    }
}
#nullable enable