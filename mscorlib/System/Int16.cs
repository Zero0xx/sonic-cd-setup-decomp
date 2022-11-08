using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000C5 RID: 197
	[ComVisible(true)]
	[Serializable]
	public struct Int16 : IComparable, IFormattable, IConvertible, IComparable<short>, IEquatable<short>
	{
		// Token: 0x06000B09 RID: 2825 RVA: 0x000225EB File Offset: 0x000215EB
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (value is short)
			{
				return (int)(this - (short)value);
			}
			throw new ArgumentException(Environment.GetResourceString("Arg_MustBeInt16"));
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00022613 File Offset: 0x00021613
		public int CompareTo(short value)
		{
			return (int)(this - value);
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00022619 File Offset: 0x00021619
		public override bool Equals(object obj)
		{
			return obj is short && this == (short)obj;
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0002262F File Offset: 0x0002162F
		public bool Equals(short obj)
		{
			return this == obj;
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00022636 File Offset: 0x00021636
		public override int GetHashCode()
		{
			return (int)((ushort)this) | (int)this << 16;
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00022641 File Offset: 0x00021641
		public override string ToString()
		{
			return Number.FormatInt32((int)this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00022650 File Offset: 0x00021650
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatInt32((int)this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00022660 File Offset: 0x00021660
		public string ToString(string format)
		{
			return this.ToString(format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0002266E File Offset: 0x0002166E
		public string ToString(string format, IFormatProvider provider)
		{
			return this.ToString(format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x00022680 File Offset: 0x00021680
		private string ToString(string format, NumberFormatInfo info)
		{
			if (this < 0 && format != null && format.Length > 0 && (format[0] == 'X' || format[0] == 'x'))
			{
				uint value = (uint)this & 65535U;
				return Number.FormatUInt32(value, format, info);
			}
			return Number.FormatInt32((int)this, format, info);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x000226CF File Offset: 0x000216CF
		public static short Parse(string s)
		{
			return short.Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x000226DD File Offset: 0x000216DD
		public static short Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return short.Parse(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x000226F1 File Offset: 0x000216F1
		public static short Parse(string s, IFormatProvider provider)
		{
			return short.Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00022700 File Offset: 0x00021700
		public static short Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return short.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00022718 File Offset: 0x00021718
		private static short Parse(string s, NumberStyles style, NumberFormatInfo info)
		{
			int num = 0;
			try
			{
				num = Number.ParseInt32(s, style, info);
			}
			catch (OverflowException innerException)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_Int16"), innerException);
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (num < 0 || num > 65535)
				{
					throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
				}
				return (short)num;
			}
			else
			{
				if (num < -32768 || num > 32767)
				{
					throw new OverflowException(Environment.GetResourceString("Overflow_Int16"));
				}
				return (short)num;
			}
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x000227A0 File Offset: 0x000217A0
		public static bool TryParse(string s, out short result)
		{
			return short.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x000227AF File Offset: 0x000217AF
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out short result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return short.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x000227C8 File Offset: 0x000217C8
		private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out short result)
		{
			result = 0;
			int num;
			if (!Number.TryParseInt32(s, style, info, out num))
			{
				return false;
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (num < 0 || num > 65535)
				{
					return false;
				}
				result = (short)num;
				return true;
			}
			else
			{
				if (num < -32768 || num > 32767)
				{
					return false;
				}
				result = (short)num;
				return true;
			}
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0002281A File Offset: 0x0002181A
		public TypeCode GetTypeCode()
		{
			return TypeCode.Int16;
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0002281D File Offset: 0x0002181D
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00022826 File Offset: 0x00021826
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0002282F File Offset: 0x0002182F
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00022838 File Offset: 0x00021838
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00022841 File Offset: 0x00021841
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00022845 File Offset: 0x00021845
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0002284E File Offset: 0x0002184E
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00022857 File Offset: 0x00021857
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x00022860 File Offset: 0x00021860
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00022869 File Offset: 0x00021869
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00022872 File Offset: 0x00021872
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0002287B File Offset: 0x0002187B
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00022884 File Offset: 0x00021884
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00022890 File Offset: 0x00021890
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"Int16",
				"DateTime"
			}));
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x000228CE File Offset: 0x000218CE
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x04000416 RID: 1046
		public const short MaxValue = 32767;

		// Token: 0x04000417 RID: 1047
		public const short MinValue = -32768;

		// Token: 0x04000418 RID: 1048
		internal short m_value;
	}
}
