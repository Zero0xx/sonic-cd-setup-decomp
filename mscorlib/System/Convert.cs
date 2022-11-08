using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System
{
	// Token: 0x02000099 RID: 153
	public static class Convert
	{
		// Token: 0x0600080B RID: 2059 RVA: 0x0001A3C8 File Offset: 0x000193C8
		public static TypeCode GetTypeCode(object value)
		{
			if (value == null)
			{
				return TypeCode.Empty;
			}
			IConvertible convertible = value as IConvertible;
			if (convertible != null)
			{
				return convertible.GetTypeCode();
			}
			return TypeCode.Object;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0001A3EC File Offset: 0x000193EC
		public static bool IsDBNull(object value)
		{
			if (value == System.DBNull.Value)
			{
				return true;
			}
			IConvertible convertible = value as IConvertible;
			return convertible != null && convertible.GetTypeCode() == TypeCode.DBNull;
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0001A418 File Offset: 0x00019418
		public static object ChangeType(object value, TypeCode typeCode)
		{
			return Convert.ChangeType(value, typeCode, Thread.CurrentThread.CurrentCulture);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0001A42C File Offset: 0x0001942C
		public static object ChangeType(object value, TypeCode typeCode, IFormatProvider provider)
		{
			if (value == null && (typeCode == TypeCode.Empty || typeCode == TypeCode.String || typeCode == TypeCode.Object))
			{
				return null;
			}
			IConvertible convertible = value as IConvertible;
			if (convertible == null)
			{
				throw new InvalidCastException(Environment.GetResourceString("InvalidCast_IConvertible"));
			}
			switch (typeCode)
			{
			case TypeCode.Empty:
				throw new InvalidCastException(Environment.GetResourceString("InvalidCast_Empty"));
			case TypeCode.Object:
				return value;
			case TypeCode.DBNull:
				throw new InvalidCastException(Environment.GetResourceString("InvalidCast_DBNull"));
			case TypeCode.Boolean:
				return convertible.ToBoolean(provider);
			case TypeCode.Char:
				return convertible.ToChar(provider);
			case TypeCode.SByte:
				return convertible.ToSByte(provider);
			case TypeCode.Byte:
				return convertible.ToByte(provider);
			case TypeCode.Int16:
				return convertible.ToInt16(provider);
			case TypeCode.UInt16:
				return convertible.ToUInt16(provider);
			case TypeCode.Int32:
				return convertible.ToInt32(provider);
			case TypeCode.UInt32:
				return convertible.ToUInt32(provider);
			case TypeCode.Int64:
				return convertible.ToInt64(provider);
			case TypeCode.UInt64:
				return convertible.ToUInt64(provider);
			case TypeCode.Single:
				return convertible.ToSingle(provider);
			case TypeCode.Double:
				return convertible.ToDouble(provider);
			case TypeCode.Decimal:
				return convertible.ToDecimal(provider);
			case TypeCode.DateTime:
				return convertible.ToDateTime(provider);
			case TypeCode.String:
				return convertible.ToString(provider);
			}
			throw new ArgumentException(Environment.GetResourceString("Arg_UnknownTypeCode"));
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0001A5AC File Offset: 0x000195AC
		internal static object DefaultToType(IConvertible value, Type targetType, IFormatProvider provider)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			if (value.GetType() == targetType)
			{
				return value;
			}
			if (targetType == Convert.ConvertTypes[3])
			{
				return value.ToBoolean(provider);
			}
			if (targetType == Convert.ConvertTypes[4])
			{
				return value.ToChar(provider);
			}
			if (targetType == Convert.ConvertTypes[5])
			{
				return value.ToSByte(provider);
			}
			if (targetType == Convert.ConvertTypes[6])
			{
				return value.ToByte(provider);
			}
			if (targetType == Convert.ConvertTypes[7])
			{
				return value.ToInt16(provider);
			}
			if (targetType == Convert.ConvertTypes[8])
			{
				return value.ToUInt16(provider);
			}
			if (targetType == Convert.ConvertTypes[9])
			{
				return value.ToInt32(provider);
			}
			if (targetType == Convert.ConvertTypes[10])
			{
				return value.ToUInt32(provider);
			}
			if (targetType == Convert.ConvertTypes[11])
			{
				return value.ToInt64(provider);
			}
			if (targetType == Convert.ConvertTypes[12])
			{
				return value.ToUInt64(provider);
			}
			if (targetType == Convert.ConvertTypes[13])
			{
				return value.ToSingle(provider);
			}
			if (targetType == Convert.ConvertTypes[14])
			{
				return value.ToDouble(provider);
			}
			if (targetType == Convert.ConvertTypes[15])
			{
				return value.ToDecimal(provider);
			}
			if (targetType == Convert.ConvertTypes[16])
			{
				return value.ToDateTime(provider);
			}
			if (targetType == Convert.ConvertTypes[18])
			{
				return value.ToString(provider);
			}
			if (targetType == Convert.ConvertTypes[1])
			{
				return value;
			}
			if (targetType == Convert.EnumType)
			{
				return (Enum)value;
			}
			if (targetType == Convert.ConvertTypes[2])
			{
				throw new InvalidCastException(Environment.GetResourceString("InvalidCast_DBNull"));
			}
			if (targetType == Convert.ConvertTypes[0])
			{
				throw new InvalidCastException(Environment.GetResourceString("InvalidCast_Empty"));
			}
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				value.GetType().FullName,
				targetType.FullName
			}));
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0001A7B6 File Offset: 0x000197B6
		public static object ChangeType(object value, Type conversionType)
		{
			return Convert.ChangeType(value, conversionType, Thread.CurrentThread.CurrentCulture);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0001A7CC File Offset: 0x000197CC
		public static object ChangeType(object value, Type conversionType, IFormatProvider provider)
		{
			if (conversionType == null)
			{
				throw new ArgumentNullException("conversionType");
			}
			if (value == null)
			{
				if (conversionType.IsValueType)
				{
					throw new InvalidCastException(Environment.GetResourceString("InvalidCast_CannotCastNullToValueType"));
				}
				return null;
			}
			else
			{
				IConvertible convertible = value as IConvertible;
				if (convertible == null)
				{
					if (value.GetType() == conversionType)
					{
						return value;
					}
					throw new InvalidCastException(Environment.GetResourceString("InvalidCast_IConvertible"));
				}
				else
				{
					if (conversionType == Convert.ConvertTypes[3])
					{
						return convertible.ToBoolean(provider);
					}
					if (conversionType == Convert.ConvertTypes[4])
					{
						return convertible.ToChar(provider);
					}
					if (conversionType == Convert.ConvertTypes[5])
					{
						return convertible.ToSByte(provider);
					}
					if (conversionType == Convert.ConvertTypes[6])
					{
						return convertible.ToByte(provider);
					}
					if (conversionType == Convert.ConvertTypes[7])
					{
						return convertible.ToInt16(provider);
					}
					if (conversionType == Convert.ConvertTypes[8])
					{
						return convertible.ToUInt16(provider);
					}
					if (conversionType == Convert.ConvertTypes[9])
					{
						return convertible.ToInt32(provider);
					}
					if (conversionType == Convert.ConvertTypes[10])
					{
						return convertible.ToUInt32(provider);
					}
					if (conversionType == Convert.ConvertTypes[11])
					{
						return convertible.ToInt64(provider);
					}
					if (conversionType == Convert.ConvertTypes[12])
					{
						return convertible.ToUInt64(provider);
					}
					if (conversionType == Convert.ConvertTypes[13])
					{
						return convertible.ToSingle(provider);
					}
					if (conversionType == Convert.ConvertTypes[14])
					{
						return convertible.ToDouble(provider);
					}
					if (conversionType == Convert.ConvertTypes[15])
					{
						return convertible.ToDecimal(provider);
					}
					if (conversionType == Convert.ConvertTypes[16])
					{
						return convertible.ToDateTime(provider);
					}
					if (conversionType == Convert.ConvertTypes[18])
					{
						return convertible.ToString(provider);
					}
					if (conversionType == Convert.ConvertTypes[1])
					{
						return value;
					}
					return convertible.ToType(conversionType, provider);
				}
			}
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0001A99A File Offset: 0x0001999A
		public static bool ToBoolean(object value)
		{
			return value != null && ((IConvertible)value).ToBoolean(null);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001A9AD File Offset: 0x000199AD
		public static bool ToBoolean(object value, IFormatProvider provider)
		{
			return value != null && ((IConvertible)value).ToBoolean(provider);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0001A9C0 File Offset: 0x000199C0
		public static bool ToBoolean(bool value)
		{
			return value;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001A9C3 File Offset: 0x000199C3
		[CLSCompliant(false)]
		public static bool ToBoolean(sbyte value)
		{
			return value != 0;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0001A9CC File Offset: 0x000199CC
		public static bool ToBoolean(char value)
		{
			return ((IConvertible)value).ToBoolean(null);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001A9DA File Offset: 0x000199DA
		public static bool ToBoolean(byte value)
		{
			return value != 0;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0001A9E3 File Offset: 0x000199E3
		public static bool ToBoolean(short value)
		{
			return value != 0;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0001A9EC File Offset: 0x000199EC
		[CLSCompliant(false)]
		public static bool ToBoolean(ushort value)
		{
			return value != 0;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0001A9F5 File Offset: 0x000199F5
		public static bool ToBoolean(int value)
		{
			return value != 0;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0001A9FE File Offset: 0x000199FE
		[CLSCompliant(false)]
		public static bool ToBoolean(uint value)
		{
			return value != 0U;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0001AA07 File Offset: 0x00019A07
		public static bool ToBoolean(long value)
		{
			return value != 0L;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0001AA11 File Offset: 0x00019A11
		[CLSCompliant(false)]
		public static bool ToBoolean(ulong value)
		{
			return value != 0UL;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0001AA1B File Offset: 0x00019A1B
		public static bool ToBoolean(string value)
		{
			return value != null && bool.Parse(value);
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0001AA28 File Offset: 0x00019A28
		public static bool ToBoolean(string value, IFormatProvider provider)
		{
			return value != null && bool.Parse(value);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0001AA35 File Offset: 0x00019A35
		public static bool ToBoolean(float value)
		{
			return value != 0f;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0001AA42 File Offset: 0x00019A42
		public static bool ToBoolean(double value)
		{
			return value != 0.0;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001AA53 File Offset: 0x00019A53
		public static bool ToBoolean(decimal value)
		{
			return value != 0m;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0001AA61 File Offset: 0x00019A61
		public static bool ToBoolean(DateTime value)
		{
			return ((IConvertible)value).ToBoolean(null);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0001AA6F File Offset: 0x00019A6F
		public static char ToChar(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToChar(null);
			}
			return '\0';
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001AA82 File Offset: 0x00019A82
		public static char ToChar(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToChar(provider);
			}
			return '\0';
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0001AA95 File Offset: 0x00019A95
		public static char ToChar(bool value)
		{
			return ((IConvertible)value).ToChar(null);
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0001AAA3 File Offset: 0x00019AA3
		public static char ToChar(char value)
		{
			return value;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0001AAA6 File Offset: 0x00019AA6
		[CLSCompliant(false)]
		public static char ToChar(sbyte value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
			}
			return (char)value;
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0001AABE File Offset: 0x00019ABE
		public static char ToChar(byte value)
		{
			return (char)value;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0001AAC1 File Offset: 0x00019AC1
		public static char ToChar(short value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
			}
			return (char)value;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0001AAD9 File Offset: 0x00019AD9
		[CLSCompliant(false)]
		public static char ToChar(ushort value)
		{
			return (char)value;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0001AADC File Offset: 0x00019ADC
		public static char ToChar(int value)
		{
			if (value < 0 || value > 65535)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
			}
			return (char)value;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0001AAFC File Offset: 0x00019AFC
		[CLSCompliant(false)]
		public static char ToChar(uint value)
		{
			if (value > 65535U)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
			}
			return (char)value;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0001AB18 File Offset: 0x00019B18
		public static char ToChar(long value)
		{
			if (value < 0L || value > 65535L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
			}
			return (char)value;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0001AB3A File Offset: 0x00019B3A
		[CLSCompliant(false)]
		public static char ToChar(ulong value)
		{
			if (value > 65535UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Char"));
			}
			return (char)value;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0001AB57 File Offset: 0x00019B57
		public static char ToChar(string value)
		{
			return Convert.ToChar(value, null);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0001AB60 File Offset: 0x00019B60
		public static char ToChar(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value.Length != 1)
			{
				throw new FormatException(Environment.GetResourceString("Format_NeedSingleChar"));
			}
			return value[0];
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0001AB90 File Offset: 0x00019B90
		public static char ToChar(float value)
		{
			return ((IConvertible)value).ToChar(null);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0001AB9E File Offset: 0x00019B9E
		public static char ToChar(double value)
		{
			return ((IConvertible)value).ToChar(null);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001ABAC File Offset: 0x00019BAC
		public static char ToChar(decimal value)
		{
			return ((IConvertible)value).ToChar(null);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0001ABBA File Offset: 0x00019BBA
		public static char ToChar(DateTime value)
		{
			return ((IConvertible)value).ToChar(null);
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0001ABC8 File Offset: 0x00019BC8
		[CLSCompliant(false)]
		public static sbyte ToSByte(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToSByte(null);
			}
			return 0;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001ABDB File Offset: 0x00019BDB
		[CLSCompliant(false)]
		public static sbyte ToSByte(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToSByte(provider);
			}
			return 0;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0001ABEE File Offset: 0x00019BEE
		[CLSCompliant(false)]
		public static sbyte ToSByte(bool value)
		{
			if (!value)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0001ABF6 File Offset: 0x00019BF6
		[CLSCompliant(false)]
		public static sbyte ToSByte(sbyte value)
		{
			return value;
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0001ABF9 File Offset: 0x00019BF9
		[CLSCompliant(false)]
		public static sbyte ToSByte(char value)
		{
			if (value > '\u007f')
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001AC12 File Offset: 0x00019C12
		[CLSCompliant(false)]
		public static sbyte ToSByte(byte value)
		{
			if (value > 127)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0001AC2B File Offset: 0x00019C2B
		[CLSCompliant(false)]
		public static sbyte ToSByte(short value)
		{
			if (value < -128 || value > 127)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0001AC49 File Offset: 0x00019C49
		[CLSCompliant(false)]
		public static sbyte ToSByte(ushort value)
		{
			if (value > 127)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0001AC62 File Offset: 0x00019C62
		[CLSCompliant(false)]
		public static sbyte ToSByte(int value)
		{
			if (value < -128 || value > 127)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0001AC80 File Offset: 0x00019C80
		[CLSCompliant(false)]
		public static sbyte ToSByte(uint value)
		{
			if ((ulong)value > 127UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0001AC9B File Offset: 0x00019C9B
		[CLSCompliant(false)]
		public static sbyte ToSByte(long value)
		{
			if (value < -128L || value > 127L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0001ACBB File Offset: 0x00019CBB
		[CLSCompliant(false)]
		public static sbyte ToSByte(ulong value)
		{
			if (value > 127UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)value;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0001ACD5 File Offset: 0x00019CD5
		[CLSCompliant(false)]
		public static sbyte ToSByte(float value)
		{
			return Convert.ToSByte((double)value);
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0001ACDE File Offset: 0x00019CDE
		[CLSCompliant(false)]
		public static sbyte ToSByte(double value)
		{
			return Convert.ToSByte(Convert.ToInt32(value));
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0001ACEB File Offset: 0x00019CEB
		[CLSCompliant(false)]
		public static sbyte ToSByte(decimal value)
		{
			return decimal.ToSByte(decimal.Round(value, 0));
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0001ACF9 File Offset: 0x00019CF9
		[CLSCompliant(false)]
		public static sbyte ToSByte(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return sbyte.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0001AD0B File Offset: 0x00019D0B
		[CLSCompliant(false)]
		public static sbyte ToSByte(string value, IFormatProvider provider)
		{
			return sbyte.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0001AD15 File Offset: 0x00019D15
		[CLSCompliant(false)]
		public static sbyte ToSByte(DateTime value)
		{
			return ((IConvertible)value).ToSByte(null);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0001AD23 File Offset: 0x00019D23
		public static byte ToByte(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToByte(null);
			}
			return 0;
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0001AD36 File Offset: 0x00019D36
		public static byte ToByte(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToByte(provider);
			}
			return 0;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0001AD49 File Offset: 0x00019D49
		public static byte ToByte(bool value)
		{
			if (!value)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0001AD51 File Offset: 0x00019D51
		public static byte ToByte(byte value)
		{
			return value;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0001AD54 File Offset: 0x00019D54
		public static byte ToByte(char value)
		{
			if (value > 'ÿ')
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0001AD70 File Offset: 0x00019D70
		[CLSCompliant(false)]
		public static byte ToByte(sbyte value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0001AD88 File Offset: 0x00019D88
		public static byte ToByte(short value)
		{
			if (value < 0 || value > 255)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0001ADA8 File Offset: 0x00019DA8
		[CLSCompliant(false)]
		public static byte ToByte(ushort value)
		{
			if (value > 255)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0001ADC4 File Offset: 0x00019DC4
		public static byte ToByte(int value)
		{
			if (value < 0 || value > 255)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0001ADE4 File Offset: 0x00019DE4
		[CLSCompliant(false)]
		public static byte ToByte(uint value)
		{
			if (value > 255U)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0001AE00 File Offset: 0x00019E00
		public static byte ToByte(long value)
		{
			if (value < 0L || value > 255L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0001AE22 File Offset: 0x00019E22
		[CLSCompliant(false)]
		public static byte ToByte(ulong value)
		{
			if (value > 255UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)value;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0001AE3F File Offset: 0x00019E3F
		public static byte ToByte(float value)
		{
			return Convert.ToByte((double)value);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0001AE48 File Offset: 0x00019E48
		public static byte ToByte(double value)
		{
			return Convert.ToByte(Convert.ToInt32(value));
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0001AE55 File Offset: 0x00019E55
		public static byte ToByte(decimal value)
		{
			return decimal.ToByte(decimal.Round(value, 0));
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0001AE63 File Offset: 0x00019E63
		public static byte ToByte(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return byte.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0001AE75 File Offset: 0x00019E75
		public static byte ToByte(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return byte.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0001AE84 File Offset: 0x00019E84
		public static byte ToByte(DateTime value)
		{
			return ((IConvertible)value).ToByte(null);
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0001AE92 File Offset: 0x00019E92
		public static short ToInt16(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToInt16(null);
			}
			return 0;
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0001AEA5 File Offset: 0x00019EA5
		public static short ToInt16(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToInt16(provider);
			}
			return 0;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0001AEB8 File Offset: 0x00019EB8
		public static short ToInt16(bool value)
		{
			if (!value)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0001AEC0 File Offset: 0x00019EC0
		public static short ToInt16(char value)
		{
			if (value > '翿')
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
			}
			return (short)value;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0001AEDC File Offset: 0x00019EDC
		[CLSCompliant(false)]
		public static short ToInt16(sbyte value)
		{
			return (short)value;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0001AEDF File Offset: 0x00019EDF
		public static short ToInt16(byte value)
		{
			return (short)value;
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0001AEE2 File Offset: 0x00019EE2
		[CLSCompliant(false)]
		public static short ToInt16(ushort value)
		{
			if (value > 32767)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
			}
			return (short)value;
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0001AEFE File Offset: 0x00019EFE
		public static short ToInt16(int value)
		{
			if (value < -32768 || value > 32767)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
			}
			return (short)value;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0001AF22 File Offset: 0x00019F22
		[CLSCompliant(false)]
		public static short ToInt16(uint value)
		{
			if ((ulong)value > 32767UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
			}
			return (short)value;
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0001AF40 File Offset: 0x00019F40
		public static short ToInt16(short value)
		{
			return value;
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0001AF43 File Offset: 0x00019F43
		public static short ToInt16(long value)
		{
			if (value < -32768L || value > 32767L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
			}
			return (short)value;
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0001AF69 File Offset: 0x00019F69
		[CLSCompliant(false)]
		public static short ToInt16(ulong value)
		{
			if (value > 32767UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
			}
			return (short)value;
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0001AF86 File Offset: 0x00019F86
		public static short ToInt16(float value)
		{
			return Convert.ToInt16((double)value);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0001AF8F File Offset: 0x00019F8F
		public static short ToInt16(double value)
		{
			return Convert.ToInt16(Convert.ToInt32(value));
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0001AF9C File Offset: 0x00019F9C
		public static short ToInt16(decimal value)
		{
			return decimal.ToInt16(decimal.Round(value, 0));
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0001AFAA File Offset: 0x00019FAA
		public static short ToInt16(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return short.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0001AFBC File Offset: 0x00019FBC
		public static short ToInt16(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return short.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0001AFCB File Offset: 0x00019FCB
		public static short ToInt16(DateTime value)
		{
			return ((IConvertible)value).ToInt16(null);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0001AFD9 File Offset: 0x00019FD9
		[CLSCompliant(false)]
		public static ushort ToUInt16(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToUInt16(null);
			}
			return 0;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0001AFEC File Offset: 0x00019FEC
		[CLSCompliant(false)]
		public static ushort ToUInt16(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToUInt16(provider);
			}
			return 0;
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0001AFFF File Offset: 0x00019FFF
		[CLSCompliant(false)]
		public static ushort ToUInt16(bool value)
		{
			if (!value)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0001B007 File Offset: 0x0001A007
		[CLSCompliant(false)]
		public static ushort ToUInt16(char value)
		{
			return (ushort)value;
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x0001B00A File Offset: 0x0001A00A
		[CLSCompliant(false)]
		public static ushort ToUInt16(sbyte value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)value;
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0001B022 File Offset: 0x0001A022
		[CLSCompliant(false)]
		public static ushort ToUInt16(byte value)
		{
			return (ushort)value;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0001B025 File Offset: 0x0001A025
		[CLSCompliant(false)]
		public static ushort ToUInt16(short value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)value;
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0001B03D File Offset: 0x0001A03D
		[CLSCompliant(false)]
		public static ushort ToUInt16(int value)
		{
			if (value < 0 || value > 65535)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)value;
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0001B05D File Offset: 0x0001A05D
		[CLSCompliant(false)]
		public static ushort ToUInt16(ushort value)
		{
			return value;
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0001B060 File Offset: 0x0001A060
		[CLSCompliant(false)]
		public static ushort ToUInt16(uint value)
		{
			if (value > 65535U)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)value;
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0001B07C File Offset: 0x0001A07C
		[CLSCompliant(false)]
		public static ushort ToUInt16(long value)
		{
			if (value < 0L || value > 65535L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)value;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0001B09E File Offset: 0x0001A09E
		[CLSCompliant(false)]
		public static ushort ToUInt16(ulong value)
		{
			if (value > 65535UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)value;
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0001B0BB File Offset: 0x0001A0BB
		[CLSCompliant(false)]
		public static ushort ToUInt16(float value)
		{
			return Convert.ToUInt16((double)value);
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0001B0C4 File Offset: 0x0001A0C4
		[CLSCompliant(false)]
		public static ushort ToUInt16(double value)
		{
			return Convert.ToUInt16(Convert.ToInt32(value));
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0001B0D1 File Offset: 0x0001A0D1
		[CLSCompliant(false)]
		public static ushort ToUInt16(decimal value)
		{
			return decimal.ToUInt16(decimal.Round(value, 0));
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0001B0DF File Offset: 0x0001A0DF
		[CLSCompliant(false)]
		public static ushort ToUInt16(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return ushort.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0001B0F1 File Offset: 0x0001A0F1
		[CLSCompliant(false)]
		public static ushort ToUInt16(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return ushort.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0001B100 File Offset: 0x0001A100
		[CLSCompliant(false)]
		public static ushort ToUInt16(DateTime value)
		{
			return ((IConvertible)value).ToUInt16(null);
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x0001B10E File Offset: 0x0001A10E
		public static int ToInt32(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToInt32(null);
			}
			return 0;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0001B121 File Offset: 0x0001A121
		public static int ToInt32(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToInt32(provider);
			}
			return 0;
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0001B134 File Offset: 0x0001A134
		public static int ToInt32(bool value)
		{
			if (!value)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x0001B13C File Offset: 0x0001A13C
		public static int ToInt32(char value)
		{
			return (int)value;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0001B13F File Offset: 0x0001A13F
		[CLSCompliant(false)]
		public static int ToInt32(sbyte value)
		{
			return (int)value;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0001B142 File Offset: 0x0001A142
		public static int ToInt32(byte value)
		{
			return (int)value;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0001B145 File Offset: 0x0001A145
		public static int ToInt32(short value)
		{
			return (int)value;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0001B148 File Offset: 0x0001A148
		[CLSCompliant(false)]
		public static int ToInt32(ushort value)
		{
			return (int)value;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0001B14B File Offset: 0x0001A14B
		[CLSCompliant(false)]
		public static int ToInt32(uint value)
		{
			if (value > 2147483647U)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
			}
			return (int)value;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0001B166 File Offset: 0x0001A166
		public static int ToInt32(int value)
		{
			return value;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0001B169 File Offset: 0x0001A169
		public static int ToInt32(long value)
		{
			if (value < -2147483648L || value > 2147483647L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
			}
			return (int)value;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0001B18F File Offset: 0x0001A18F
		[CLSCompliant(false)]
		public static int ToInt32(ulong value)
		{
			if (value > 2147483647UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
			}
			return (int)value;
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0001B1AC File Offset: 0x0001A1AC
		public static int ToInt32(float value)
		{
			return Convert.ToInt32((double)value);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0001B1B8 File Offset: 0x0001A1B8
		public static int ToInt32(double value)
		{
			if (value >= 0.0)
			{
				if (value < 2147483647.5)
				{
					int num = (int)value;
					double num2 = value - (double)num;
					if (num2 > 0.5 || (num2 == 0.5 && (num & 1) != 0))
					{
						num++;
					}
					return num;
				}
			}
			else if (value >= -2147483648.5)
			{
				int num3 = (int)value;
				double num4 = value - (double)num3;
				if (num4 < -0.5 || (num4 == -0.5 && (num3 & 1) != 0))
				{
					num3--;
				}
				return num3;
			}
			throw new OverflowException(Environment.GetResourceString("Overflow_Int32"));
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0001B24E File Offset: 0x0001A24E
		public static int ToInt32(decimal value)
		{
			return decimal.FCallToInt32(value);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0001B256 File Offset: 0x0001A256
		public static int ToInt32(string value)
		{
			if (value == null)
			{
				return 0;
			}
			return int.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0001B268 File Offset: 0x0001A268
		public static int ToInt32(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0;
			}
			return int.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0001B277 File Offset: 0x0001A277
		public static int ToInt32(DateTime value)
		{
			return ((IConvertible)value).ToInt32(null);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0001B285 File Offset: 0x0001A285
		[CLSCompliant(false)]
		public static uint ToUInt32(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToUInt32(null);
			}
			return 0U;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0001B298 File Offset: 0x0001A298
		[CLSCompliant(false)]
		public static uint ToUInt32(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToUInt32(provider);
			}
			return 0U;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0001B2AB File Offset: 0x0001A2AB
		[CLSCompliant(false)]
		public static uint ToUInt32(bool value)
		{
			if (!value)
			{
				return 0U;
			}
			return 1U;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0001B2B3 File Offset: 0x0001A2B3
		[CLSCompliant(false)]
		public static uint ToUInt32(char value)
		{
			return (uint)value;
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0001B2B6 File Offset: 0x0001A2B6
		[CLSCompliant(false)]
		public static uint ToUInt32(sbyte value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
			}
			return (uint)value;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0001B2CD File Offset: 0x0001A2CD
		[CLSCompliant(false)]
		public static uint ToUInt32(byte value)
		{
			return (uint)value;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0001B2D0 File Offset: 0x0001A2D0
		[CLSCompliant(false)]
		public static uint ToUInt32(short value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
			}
			return (uint)value;
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0001B2E7 File Offset: 0x0001A2E7
		[CLSCompliant(false)]
		public static uint ToUInt32(ushort value)
		{
			return (uint)value;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0001B2EA File Offset: 0x0001A2EA
		[CLSCompliant(false)]
		public static uint ToUInt32(int value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
			}
			return (uint)value;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0001B301 File Offset: 0x0001A301
		[CLSCompliant(false)]
		public static uint ToUInt32(uint value)
		{
			return value;
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0001B304 File Offset: 0x0001A304
		[CLSCompliant(false)]
		public static uint ToUInt32(long value)
		{
			if (value < 0L || value > (long)((ulong)-1))
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
			}
			return (uint)value;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0001B322 File Offset: 0x0001A322
		[CLSCompliant(false)]
		public static uint ToUInt32(ulong value)
		{
			if (value > (ulong)-1)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
			}
			return (uint)value;
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0001B33B File Offset: 0x0001A33B
		[CLSCompliant(false)]
		public static uint ToUInt32(float value)
		{
			return Convert.ToUInt32((double)value);
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0001B344 File Offset: 0x0001A344
		[CLSCompliant(false)]
		public static uint ToUInt32(double value)
		{
			if (value >= -0.5 && value < 4294967295.5)
			{
				uint num = (uint)value;
				double num2 = value - num;
				if (num2 > 0.5 || (num2 == 0.5 && (num & 1U) != 0U))
				{
					num += 1U;
				}
				return num;
			}
			throw new OverflowException(Environment.GetResourceString("Overflow_UInt32"));
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0001B3A4 File Offset: 0x0001A3A4
		[CLSCompliant(false)]
		public static uint ToUInt32(decimal value)
		{
			return decimal.ToUInt32(decimal.Round(value, 0));
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0001B3B2 File Offset: 0x0001A3B2
		[CLSCompliant(false)]
		public static uint ToUInt32(string value)
		{
			if (value == null)
			{
				return 0U;
			}
			return uint.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0001B3C4 File Offset: 0x0001A3C4
		[CLSCompliant(false)]
		public static uint ToUInt32(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0U;
			}
			return uint.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0001B3D3 File Offset: 0x0001A3D3
		[CLSCompliant(false)]
		public static uint ToUInt32(DateTime value)
		{
			return ((IConvertible)value).ToUInt32(null);
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0001B3E1 File Offset: 0x0001A3E1
		public static long ToInt64(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToInt64(null);
			}
			return 0L;
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0001B3F5 File Offset: 0x0001A3F5
		public static long ToInt64(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToInt64(provider);
			}
			return 0L;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0001B409 File Offset: 0x0001A409
		public static long ToInt64(bool value)
		{
			return value ? 1L : 0L;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0001B413 File Offset: 0x0001A413
		public static long ToInt64(char value)
		{
			return (long)((ulong)value);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0001B417 File Offset: 0x0001A417
		[CLSCompliant(false)]
		public static long ToInt64(sbyte value)
		{
			return (long)value;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0001B41B File Offset: 0x0001A41B
		public static long ToInt64(byte value)
		{
			return (long)((ulong)value);
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0001B41F File Offset: 0x0001A41F
		public static long ToInt64(short value)
		{
			return (long)value;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0001B423 File Offset: 0x0001A423
		[CLSCompliant(false)]
		public static long ToInt64(ushort value)
		{
			return (long)((ulong)value);
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0001B427 File Offset: 0x0001A427
		public static long ToInt64(int value)
		{
			return (long)value;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0001B42B File Offset: 0x0001A42B
		[CLSCompliant(false)]
		public static long ToInt64(uint value)
		{
			return (long)((ulong)value);
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0001B42F File Offset: 0x0001A42F
		[CLSCompliant(false)]
		public static long ToInt64(ulong value)
		{
			if (value > 9223372036854775807UL)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int64"));
			}
			return (long)value;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0001B44E File Offset: 0x0001A44E
		public static long ToInt64(long value)
		{
			return value;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0001B451 File Offset: 0x0001A451
		public static long ToInt64(float value)
		{
			return Convert.ToInt64((double)value);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0001B45A File Offset: 0x0001A45A
		public static long ToInt64(double value)
		{
			return checked((long)Math.Round(value));
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0001B463 File Offset: 0x0001A463
		public static long ToInt64(decimal value)
		{
			return decimal.ToInt64(decimal.Round(value, 0));
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0001B471 File Offset: 0x0001A471
		public static long ToInt64(string value)
		{
			if (value == null)
			{
				return 0L;
			}
			return long.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0001B484 File Offset: 0x0001A484
		public static long ToInt64(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0L;
			}
			return long.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0001B494 File Offset: 0x0001A494
		public static long ToInt64(DateTime value)
		{
			return ((IConvertible)value).ToInt64(null);
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0001B4A2 File Offset: 0x0001A4A2
		[CLSCompliant(false)]
		public static ulong ToUInt64(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToUInt64(null);
			}
			return 0UL;
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0001B4B6 File Offset: 0x0001A4B6
		[CLSCompliant(false)]
		public static ulong ToUInt64(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToUInt64(provider);
			}
			return 0UL;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x0001B4CA File Offset: 0x0001A4CA
		[CLSCompliant(false)]
		public static ulong ToUInt64(bool value)
		{
			if (!value)
			{
				return 0UL;
			}
			return 1UL;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0001B4D4 File Offset: 0x0001A4D4
		[CLSCompliant(false)]
		public static ulong ToUInt64(char value)
		{
			return (ulong)value;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0001B4D8 File Offset: 0x0001A4D8
		[CLSCompliant(false)]
		public static ulong ToUInt64(sbyte value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
			}
			return (ulong)((long)value);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0001B4F0 File Offset: 0x0001A4F0
		[CLSCompliant(false)]
		public static ulong ToUInt64(byte value)
		{
			return (ulong)value;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0001B4F4 File Offset: 0x0001A4F4
		[CLSCompliant(false)]
		public static ulong ToUInt64(short value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
			}
			return (ulong)((long)value);
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0001B50C File Offset: 0x0001A50C
		[CLSCompliant(false)]
		public static ulong ToUInt64(ushort value)
		{
			return (ulong)value;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x0001B510 File Offset: 0x0001A510
		[CLSCompliant(false)]
		public static ulong ToUInt64(int value)
		{
			if (value < 0)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
			}
			return (ulong)((long)value);
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0001B528 File Offset: 0x0001A528
		[CLSCompliant(false)]
		public static ulong ToUInt64(uint value)
		{
			return (ulong)value;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x0001B52C File Offset: 0x0001A52C
		[CLSCompliant(false)]
		public static ulong ToUInt64(long value)
		{
			if (value < 0L)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt64"));
			}
			return (ulong)value;
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0001B544 File Offset: 0x0001A544
		[CLSCompliant(false)]
		public static ulong ToUInt64(ulong value)
		{
			return value;
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0001B547 File Offset: 0x0001A547
		[CLSCompliant(false)]
		public static ulong ToUInt64(float value)
		{
			return Convert.ToUInt64((double)value);
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0001B550 File Offset: 0x0001A550
		[CLSCompliant(false)]
		public static ulong ToUInt64(double value)
		{
			return checked((ulong)Math.Round(value));
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0001B559 File Offset: 0x0001A559
		[CLSCompliant(false)]
		public static ulong ToUInt64(decimal value)
		{
			return decimal.ToUInt64(decimal.Round(value, 0));
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x0001B567 File Offset: 0x0001A567
		[CLSCompliant(false)]
		public static ulong ToUInt64(string value)
		{
			if (value == null)
			{
				return 0UL;
			}
			return ulong.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0001B57A File Offset: 0x0001A57A
		[CLSCompliant(false)]
		public static ulong ToUInt64(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0UL;
			}
			return ulong.Parse(value, NumberStyles.Integer, provider);
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x0001B58A File Offset: 0x0001A58A
		[CLSCompliant(false)]
		public static ulong ToUInt64(DateTime value)
		{
			return ((IConvertible)value).ToUInt64(null);
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0001B598 File Offset: 0x0001A598
		public static float ToSingle(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToSingle(null);
			}
			return 0f;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0001B5AF File Offset: 0x0001A5AF
		public static float ToSingle(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToSingle(provider);
			}
			return 0f;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0001B5C6 File Offset: 0x0001A5C6
		[CLSCompliant(false)]
		public static float ToSingle(sbyte value)
		{
			return (float)value;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0001B5CA File Offset: 0x0001A5CA
		public static float ToSingle(byte value)
		{
			return (float)value;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x0001B5CE File Offset: 0x0001A5CE
		public static float ToSingle(char value)
		{
			return ((IConvertible)value).ToSingle(null);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0001B5DC File Offset: 0x0001A5DC
		public static float ToSingle(short value)
		{
			return (float)value;
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0001B5E0 File Offset: 0x0001A5E0
		[CLSCompliant(false)]
		public static float ToSingle(ushort value)
		{
			return (float)value;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0001B5E4 File Offset: 0x0001A5E4
		public static float ToSingle(int value)
		{
			return (float)value;
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0001B5E8 File Offset: 0x0001A5E8
		[CLSCompliant(false)]
		public static float ToSingle(uint value)
		{
			return value;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0001B5ED File Offset: 0x0001A5ED
		public static float ToSingle(long value)
		{
			return (float)value;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0001B5F1 File Offset: 0x0001A5F1
		[CLSCompliant(false)]
		public static float ToSingle(ulong value)
		{
			return value;
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0001B5F6 File Offset: 0x0001A5F6
		public static float ToSingle(float value)
		{
			return value;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0001B5F9 File Offset: 0x0001A5F9
		public static float ToSingle(double value)
		{
			return (float)value;
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0001B5FD File Offset: 0x0001A5FD
		public static float ToSingle(decimal value)
		{
			return (float)value;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0001B606 File Offset: 0x0001A606
		public static float ToSingle(string value)
		{
			if (value == null)
			{
				return 0f;
			}
			return float.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0001B61C File Offset: 0x0001A61C
		public static float ToSingle(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0f;
			}
			return float.Parse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, provider);
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0001B633 File Offset: 0x0001A633
		public static float ToSingle(bool value)
		{
			return (float)(value ? 1 : 0);
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0001B63D File Offset: 0x0001A63D
		public static float ToSingle(DateTime value)
		{
			return ((IConvertible)value).ToSingle(null);
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0001B64B File Offset: 0x0001A64B
		public static double ToDouble(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToDouble(null);
			}
			return 0.0;
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0001B666 File Offset: 0x0001A666
		public static double ToDouble(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToDouble(provider);
			}
			return 0.0;
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0001B681 File Offset: 0x0001A681
		[CLSCompliant(false)]
		public static double ToDouble(sbyte value)
		{
			return (double)value;
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0001B685 File Offset: 0x0001A685
		public static double ToDouble(byte value)
		{
			return (double)value;
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0001B689 File Offset: 0x0001A689
		public static double ToDouble(short value)
		{
			return (double)value;
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0001B68D File Offset: 0x0001A68D
		public static double ToDouble(char value)
		{
			return ((IConvertible)value).ToDouble(null);
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0001B69B File Offset: 0x0001A69B
		[CLSCompliant(false)]
		public static double ToDouble(ushort value)
		{
			return (double)value;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0001B69F File Offset: 0x0001A69F
		public static double ToDouble(int value)
		{
			return (double)value;
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0001B6A3 File Offset: 0x0001A6A3
		[CLSCompliant(false)]
		public static double ToDouble(uint value)
		{
			return value;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0001B6A8 File Offset: 0x0001A6A8
		public static double ToDouble(long value)
		{
			return (double)value;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0001B6AC File Offset: 0x0001A6AC
		[CLSCompliant(false)]
		public static double ToDouble(ulong value)
		{
			return value;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0001B6B1 File Offset: 0x0001A6B1
		public static double ToDouble(float value)
		{
			return (double)value;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0001B6B5 File Offset: 0x0001A6B5
		public static double ToDouble(double value)
		{
			return value;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0001B6B8 File Offset: 0x0001A6B8
		public static double ToDouble(decimal value)
		{
			return (double)value;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0001B6C1 File Offset: 0x0001A6C1
		public static double ToDouble(string value)
		{
			if (value == null)
			{
				return 0.0;
			}
			return double.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0001B6DB File Offset: 0x0001A6DB
		public static double ToDouble(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0.0;
			}
			return double.Parse(value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, provider);
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0001B6F6 File Offset: 0x0001A6F6
		public static double ToDouble(bool value)
		{
			return (double)(value ? 1 : 0);
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0001B700 File Offset: 0x0001A700
		public static double ToDouble(DateTime value)
		{
			return ((IConvertible)value).ToDouble(null);
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0001B70E File Offset: 0x0001A70E
		public static decimal ToDecimal(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToDecimal(null);
			}
			return 0m;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0001B726 File Offset: 0x0001A726
		public static decimal ToDecimal(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToDecimal(provider);
			}
			return 0m;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0001B73E File Offset: 0x0001A73E
		[CLSCompliant(false)]
		public static decimal ToDecimal(sbyte value)
		{
			return value;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0001B746 File Offset: 0x0001A746
		public static decimal ToDecimal(byte value)
		{
			return value;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0001B74E File Offset: 0x0001A74E
		public static decimal ToDecimal(char value)
		{
			return ((IConvertible)value).ToDecimal(null);
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0001B75C File Offset: 0x0001A75C
		public static decimal ToDecimal(short value)
		{
			return value;
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0001B764 File Offset: 0x0001A764
		[CLSCompliant(false)]
		public static decimal ToDecimal(ushort value)
		{
			return value;
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0001B76C File Offset: 0x0001A76C
		public static decimal ToDecimal(int value)
		{
			return value;
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0001B774 File Offset: 0x0001A774
		[CLSCompliant(false)]
		public static decimal ToDecimal(uint value)
		{
			return value;
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0001B77C File Offset: 0x0001A77C
		public static decimal ToDecimal(long value)
		{
			return value;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0001B784 File Offset: 0x0001A784
		[CLSCompliant(false)]
		public static decimal ToDecimal(ulong value)
		{
			return value;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0001B78C File Offset: 0x0001A78C
		public static decimal ToDecimal(float value)
		{
			return (decimal)value;
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0001B795 File Offset: 0x0001A795
		public static decimal ToDecimal(double value)
		{
			return (decimal)value;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0001B79E File Offset: 0x0001A79E
		public static decimal ToDecimal(string value)
		{
			if (value == null)
			{
				return 0m;
			}
			return decimal.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0001B7B5 File Offset: 0x0001A7B5
		public static decimal ToDecimal(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return 0m;
			}
			return decimal.Parse(value, NumberStyles.Number, provider);
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0001B7CA File Offset: 0x0001A7CA
		public static decimal ToDecimal(decimal value)
		{
			return value;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0001B7CD File Offset: 0x0001A7CD
		public static decimal ToDecimal(bool value)
		{
			return value ? 1 : 0;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0001B7DB File Offset: 0x0001A7DB
		public static decimal ToDecimal(DateTime value)
		{
			return ((IConvertible)value).ToDecimal(null);
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0001B7E9 File Offset: 0x0001A7E9
		public static DateTime ToDateTime(DateTime value)
		{
			return value;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0001B7EC File Offset: 0x0001A7EC
		public static DateTime ToDateTime(object value)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToDateTime(null);
			}
			return DateTime.MinValue;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0001B803 File Offset: 0x0001A803
		public static DateTime ToDateTime(object value, IFormatProvider provider)
		{
			if (value != null)
			{
				return ((IConvertible)value).ToDateTime(provider);
			}
			return DateTime.MinValue;
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0001B81A File Offset: 0x0001A81A
		public static DateTime ToDateTime(string value)
		{
			if (value == null)
			{
				return new DateTime(0L);
			}
			return DateTime.Parse(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0001B832 File Offset: 0x0001A832
		public static DateTime ToDateTime(string value, IFormatProvider provider)
		{
			if (value == null)
			{
				return new DateTime(0L);
			}
			return DateTime.Parse(value, provider);
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0001B846 File Offset: 0x0001A846
		[CLSCompliant(false)]
		public static DateTime ToDateTime(sbyte value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0001B854 File Offset: 0x0001A854
		public static DateTime ToDateTime(byte value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0001B862 File Offset: 0x0001A862
		public static DateTime ToDateTime(short value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0001B870 File Offset: 0x0001A870
		[CLSCompliant(false)]
		public static DateTime ToDateTime(ushort value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0001B87E File Offset: 0x0001A87E
		public static DateTime ToDateTime(int value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0001B88C File Offset: 0x0001A88C
		[CLSCompliant(false)]
		public static DateTime ToDateTime(uint value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x0001B89A File Offset: 0x0001A89A
		public static DateTime ToDateTime(long value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x0001B8A8 File Offset: 0x0001A8A8
		[CLSCompliant(false)]
		public static DateTime ToDateTime(ulong value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0001B8B6 File Offset: 0x0001A8B6
		public static DateTime ToDateTime(bool value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0001B8C4 File Offset: 0x0001A8C4
		public static DateTime ToDateTime(char value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0001B8D2 File Offset: 0x0001A8D2
		public static DateTime ToDateTime(float value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0001B8E0 File Offset: 0x0001A8E0
		public static DateTime ToDateTime(double value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x0001B8EE File Offset: 0x0001A8EE
		public static DateTime ToDateTime(decimal value)
		{
			return ((IConvertible)value).ToDateTime(null);
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x0001B8FC File Offset: 0x0001A8FC
		public static string ToString(object value)
		{
			return Convert.ToString(value, null);
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x0001B908 File Offset: 0x0001A908
		public static string ToString(object value, IFormatProvider provider)
		{
			IConvertible convertible = value as IConvertible;
			if (convertible != null)
			{
				return convertible.ToString(provider);
			}
			IFormattable formattable = value as IFormattable;
			if (formattable != null)
			{
				return formattable.ToString(null, provider);
			}
			if (value != null)
			{
				return value.ToString();
			}
			return string.Empty;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0001B949 File Offset: 0x0001A949
		public static string ToString(bool value)
		{
			return value.ToString();
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0001B952 File Offset: 0x0001A952
		public static string ToString(bool value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0001B95C File Offset: 0x0001A95C
		public static string ToString(char value)
		{
			return char.ToString(value);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0001B964 File Offset: 0x0001A964
		public static string ToString(char value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0001B96E File Offset: 0x0001A96E
		[CLSCompliant(false)]
		public static string ToString(sbyte value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0001B97C File Offset: 0x0001A97C
		[CLSCompliant(false)]
		public static string ToString(sbyte value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0001B986 File Offset: 0x0001A986
		public static string ToString(byte value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0001B994 File Offset: 0x0001A994
		public static string ToString(byte value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0001B99E File Offset: 0x0001A99E
		public static string ToString(short value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0001B9AC File Offset: 0x0001A9AC
		public static string ToString(short value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0001B9B6 File Offset: 0x0001A9B6
		[CLSCompliant(false)]
		public static string ToString(ushort value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0001B9C4 File Offset: 0x0001A9C4
		[CLSCompliant(false)]
		public static string ToString(ushort value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x0001B9CE File Offset: 0x0001A9CE
		public static string ToString(int value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0001B9DC File Offset: 0x0001A9DC
		public static string ToString(int value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0001B9E6 File Offset: 0x0001A9E6
		[CLSCompliant(false)]
		public static string ToString(uint value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0001B9F4 File Offset: 0x0001A9F4
		[CLSCompliant(false)]
		public static string ToString(uint value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0001B9FE File Offset: 0x0001A9FE
		public static string ToString(long value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0001BA0C File Offset: 0x0001AA0C
		public static string ToString(long value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0001BA16 File Offset: 0x0001AA16
		[CLSCompliant(false)]
		public static string ToString(ulong value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0001BA24 File Offset: 0x0001AA24
		[CLSCompliant(false)]
		public static string ToString(ulong value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0001BA2E File Offset: 0x0001AA2E
		public static string ToString(float value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0001BA3C File Offset: 0x0001AA3C
		public static string ToString(float value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0001BA46 File Offset: 0x0001AA46
		public static string ToString(double value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0001BA54 File Offset: 0x0001AA54
		public static string ToString(double value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0001BA5E File Offset: 0x0001AA5E
		public static string ToString(decimal value)
		{
			return value.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0001BA6C File Offset: 0x0001AA6C
		public static string ToString(decimal value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0001BA76 File Offset: 0x0001AA76
		public static string ToString(DateTime value)
		{
			return value.ToString();
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0001BA85 File Offset: 0x0001AA85
		public static string ToString(DateTime value, IFormatProvider provider)
		{
			return value.ToString(provider);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0001BA8F File Offset: 0x0001AA8F
		public static string ToString(string value)
		{
			return value;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0001BA92 File Offset: 0x0001AA92
		public static string ToString(string value, IFormatProvider provider)
		{
			return value;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0001BA98 File Offset: 0x0001AA98
		public static byte ToByte(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			int num = ParseNumbers.StringToInt(value, fromBase, 4608);
			if (num < 0 || num > 255)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Byte"));
			}
			return (byte)num;
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0001BAF4 File Offset: 0x0001AAF4
		[CLSCompliant(false)]
		public static sbyte ToSByte(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			int num = ParseNumbers.StringToInt(value, fromBase, 5120);
			if (fromBase != 10 && num <= 255)
			{
				return (sbyte)num;
			}
			if (num < -128 || num > 127)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
			}
			return (sbyte)num;
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0001BB5C File Offset: 0x0001AB5C
		public static short ToInt16(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			int num = ParseNumbers.StringToInt(value, fromBase, 6144);
			if (fromBase != 10 && num <= 65535)
			{
				return (short)num;
			}
			if (num < -32768 || num > 32767)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
			}
			return (short)num;
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0001BBCC File Offset: 0x0001ABCC
		[CLSCompliant(false)]
		public static ushort ToUInt16(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			int num = ParseNumbers.StringToInt(value, fromBase, 4608);
			if (num < 0 || num > 65535)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)num;
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0001BC26 File Offset: 0x0001AC26
		public static int ToInt32(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return ParseNumbers.StringToInt(value, fromBase, 4096);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0001BC56 File Offset: 0x0001AC56
		[CLSCompliant(false)]
		public static uint ToUInt32(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return (uint)ParseNumbers.StringToInt(value, fromBase, 4608);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0001BC86 File Offset: 0x0001AC86
		public static long ToInt64(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return ParseNumbers.StringToLong(value, fromBase, 4096);
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0001BCB6 File Offset: 0x0001ACB6
		[CLSCompliant(false)]
		public static ulong ToUInt64(string value, int fromBase)
		{
			if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return (ulong)ParseNumbers.StringToLong(value, fromBase, 4608);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0001BCE6 File Offset: 0x0001ACE6
		public static string ToString(byte value, int toBase)
		{
			if (toBase != 2 && toBase != 8 && toBase != 10 && toBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return ParseNumbers.IntToString((int)value, toBase, -1, ' ', 64);
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0001BD16 File Offset: 0x0001AD16
		public static string ToString(short value, int toBase)
		{
			if (toBase != 2 && toBase != 8 && toBase != 10 && toBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return ParseNumbers.IntToString((int)value, toBase, -1, ' ', 128);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0001BD49 File Offset: 0x0001AD49
		public static string ToString(int value, int toBase)
		{
			if (toBase != 2 && toBase != 8 && toBase != 10 && toBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return ParseNumbers.IntToString(value, toBase, -1, ' ', 0);
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0001BD78 File Offset: 0x0001AD78
		public static string ToString(long value, int toBase)
		{
			if (toBase != 2 && toBase != 8 && toBase != 10 && toBase != 16)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidBase"));
			}
			return ParseNumbers.LongToString(value, toBase, -1, ' ', 0);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0001BDA7 File Offset: 0x0001ADA7
		public static string ToBase64String(byte[] inArray)
		{
			if (inArray == null)
			{
				throw new ArgumentNullException("inArray");
			}
			return Convert.ToBase64String(inArray, 0, inArray.Length, Base64FormattingOptions.None);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0001BDC2 File Offset: 0x0001ADC2
		[ComVisible(false)]
		public static string ToBase64String(byte[] inArray, Base64FormattingOptions options)
		{
			if (inArray == null)
			{
				throw new ArgumentNullException("inArray");
			}
			return Convert.ToBase64String(inArray, 0, inArray.Length, options);
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0001BDDD File Offset: 0x0001ADDD
		public static string ToBase64String(byte[] inArray, int offset, int length)
		{
			return Convert.ToBase64String(inArray, offset, length, Base64FormattingOptions.None);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0001BDE8 File Offset: 0x0001ADE8
		[ComVisible(false)]
		public unsafe static string ToBase64String(byte[] inArray, int offset, int length, Base64FormattingOptions options)
		{
			if (inArray == null)
			{
				throw new ArgumentNullException("inArray");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
			}
			int num = inArray.Length;
			if (offset > num - length)
			{
				throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_OffsetLength"));
			}
			if (options < Base64FormattingOptions.None || options > Base64FormattingOptions.InsertLineBreaks)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[]
				{
					(int)options
				}));
			}
			if (num == 0)
			{
				return string.Empty;
			}
			bool insertLineBreaks = options == Base64FormattingOptions.InsertLineBreaks;
			int capacity = Convert.CalculateOutputLength(length, insertLineBreaks);
			string stringForStringBuilder = string.GetStringForStringBuilder(string.Empty, capacity);
			fixed (char* outChars = stringForStringBuilder)
			{
				fixed (byte* ptr = inArray)
				{
					int length2 = Convert.ConvertToBase64Array(outChars, ptr, offset, length, insertLineBreaks);
					stringForStringBuilder.SetLength(length2);
					return stringForStringBuilder;
				}
			}
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0001BEED File Offset: 0x0001AEED
		public static int ToBase64CharArray(byte[] inArray, int offsetIn, int length, char[] outArray, int offsetOut)
		{
			return Convert.ToBase64CharArray(inArray, offsetIn, length, outArray, offsetOut, Base64FormattingOptions.None);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0001BEFC File Offset: 0x0001AEFC
		[ComVisible(false)]
		public unsafe static int ToBase64CharArray(byte[] inArray, int offsetIn, int length, char[] outArray, int offsetOut, Base64FormattingOptions options)
		{
			if (inArray == null)
			{
				throw new ArgumentNullException("inArray");
			}
			if (outArray == null)
			{
				throw new ArgumentNullException("outArray");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (offsetIn < 0)
			{
				throw new ArgumentOutOfRangeException("offsetIn", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
			}
			if (offsetOut < 0)
			{
				throw new ArgumentOutOfRangeException("offsetOut", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
			}
			if (options < Base64FormattingOptions.None || options > Base64FormattingOptions.InsertLineBreaks)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[]
				{
					(int)options
				}));
			}
			int num = inArray.Length;
			if (offsetIn > num - length)
			{
				throw new ArgumentOutOfRangeException("offsetIn", Environment.GetResourceString("ArgumentOutOfRange_OffsetLength"));
			}
			if (num == 0)
			{
				return 0;
			}
			bool insertLineBreaks = options == Base64FormattingOptions.InsertLineBreaks;
			int num2 = outArray.Length;
			int num3 = Convert.CalculateOutputLength(length, insertLineBreaks);
			if (offsetOut > num2 - num3)
			{
				throw new ArgumentOutOfRangeException("offsetOut", Environment.GetResourceString("ArgumentOutOfRange_OffsetOut"));
			}
			int result;
			fixed (char* ptr = &outArray[offsetOut])
			{
				fixed (byte* ptr2 = inArray)
				{
					result = Convert.ConvertToBase64Array(ptr, ptr2, offsetIn, length, insertLineBreaks);
				}
			}
			return result;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0001C034 File Offset: 0x0001B034
		private unsafe static int ConvertToBase64Array(char* outChars, byte* inData, int offset, int length, bool insertLineBreaks)
		{
			int num = length % 3;
			int num2 = offset + (length - num);
			int num3 = 0;
			int num4 = 0;
			fixed (char* ptr = Convert.base64Table)
			{
				int i;
				for (i = offset; i < num2; i += 3)
				{
					if (insertLineBreaks)
					{
						if (num4 == 76)
						{
							outChars[num3++] = '\r';
							outChars[num3++] = '\n';
							num4 = 0;
						}
						num4 += 4;
					}
					outChars[num3] = ptr[(inData[i] & 252) >> 2];
					outChars[num3 + 1] = ptr[(int)(inData[i] & 3) << 4 | (inData[i + 1] & 240) >> 4];
					outChars[num3 + 2] = ptr[(int)(inData[i + 1] & 15) << 2 | (inData[i + 2] & 192) >> 6];
					outChars[num3 + 3] = ptr[inData[i + 2] & 63];
					num3 += 4;
				}
				i = num2;
				if (insertLineBreaks && num != 0 && num4 == 76)
				{
					outChars[num3++] = '\r';
					outChars[num3++] = '\n';
				}
				switch (num)
				{
				case 1:
					outChars[num3] = ptr[(inData[i] & 252) >> 2];
					outChars[num3 + 1] = ptr[(inData[i] & 3) << 4];
					outChars[num3 + 2] = ptr[64];
					outChars[num3 + 3] = ptr[64];
					num3 += 4;
					break;
				case 2:
					outChars[num3] = ptr[(inData[i] & 252) >> 2];
					outChars[num3 + 1] = ptr[(int)(inData[i] & 3) << 4 | (inData[i + 1] & 240) >> 4];
					outChars[num3 + 2] = ptr[(inData[i + 1] & 15) << 2];
					outChars[num3 + 3] = ptr[64];
					num3 += 4;
					break;
				}
			}
			return num3;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0001C268 File Offset: 0x0001B268
		private static int CalculateOutputLength(int inputLength, bool insertLineBreaks)
		{
			int num = inputLength / 3 * 4;
			num += ((inputLength % 3 != 0) ? 4 : 0);
			if (num == 0)
			{
				return num;
			}
			if (insertLineBreaks)
			{
				int num2 = num / 76;
				if (num % 76 == 0)
				{
					num2--;
				}
				num += num2 * 2;
			}
			return num;
		}

		// Token: 0x06000942 RID: 2370
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte[] FromBase64String(string s);

		// Token: 0x06000943 RID: 2371
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte[] FromBase64CharArray(char[] inArray, int offset, int length);

		// Token: 0x04000384 RID: 900
		internal static readonly Type[] ConvertTypes = new Type[]
		{
			typeof(Empty),
			typeof(object),
			typeof(DBNull),
			typeof(bool),
			typeof(char),
			typeof(sbyte),
			typeof(byte),
			typeof(short),
			typeof(ushort),
			typeof(int),
			typeof(uint),
			typeof(long),
			typeof(ulong),
			typeof(float),
			typeof(double),
			typeof(decimal),
			typeof(DateTime),
			typeof(object),
			typeof(string)
		};

		// Token: 0x04000385 RID: 901
		internal static readonly Type EnumType = typeof(Enum);

		// Token: 0x04000386 RID: 902
		internal static readonly char[] base64Table = new char[]
		{
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'I',
			'J',
			'K',
			'L',
			'M',
			'N',
			'O',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z',
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'g',
			'h',
			'i',
			'j',
			'k',
			'l',
			'm',
			'n',
			'o',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'w',
			'x',
			'y',
			'z',
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'+',
			'/',
			'='
		};

		// Token: 0x04000387 RID: 903
		public static readonly object DBNull = System.DBNull.Value;
	}
}
