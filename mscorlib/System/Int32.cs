using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000C6 RID: 198
	[ComVisible(true)]
	[Serializable]
	public struct Int32 : IComparable, IFormattable, IConvertible, IComparable<int>, IEquatable<int>
	{
		// Token: 0x06000B2B RID: 2859 RVA: 0x000228E0 File Offset: 0x000218E0
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is int))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeInt32"));
			}
			int num = (int)value;
			if (this < num)
			{
				return -1;
			}
			if (this > num)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00022920 File Offset: 0x00021920
		public int CompareTo(int value)
		{
			if (this < value)
			{
				return -1;
			}
			if (this > value)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00022931 File Offset: 0x00021931
		public override bool Equals(object obj)
		{
			return obj is int && this == (int)obj;
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00022947 File Offset: 0x00021947
		public bool Equals(int obj)
		{
			return this == obj;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0002294E File Offset: 0x0002194E
		public override int GetHashCode()
		{
			return this;
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00022952 File Offset: 0x00021952
		public override string ToString()
		{
			return Number.FormatInt32(this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00022961 File Offset: 0x00021961
		public string ToString(string format)
		{
			return Number.FormatInt32(this, format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00022970 File Offset: 0x00021970
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatInt32(this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00022980 File Offset: 0x00021980
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatInt32(this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00022990 File Offset: 0x00021990
		public static int Parse(string s)
		{
			return Number.ParseInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0002299E File Offset: 0x0002199E
		public static int Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseInt32(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x000229B2 File Offset: 0x000219B2
		public static int Parse(string s, IFormatProvider provider)
		{
			return Number.ParseInt32(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x000229C1 File Offset: 0x000219C1
		public static int Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseInt32(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x000229D6 File Offset: 0x000219D6
		public static bool TryParse(string s, out int result)
		{
			return Number.TryParseInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x000229E5 File Offset: 0x000219E5
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out int result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.TryParseInt32(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x000229FB File Offset: 0x000219FB
		public TypeCode GetTypeCode()
		{
			return TypeCode.Int32;
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x000229FF File Offset: 0x000219FF
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00022A08 File Offset: 0x00021A08
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00022A11 File Offset: 0x00021A11
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00022A1A File Offset: 0x00021A1A
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00022A23 File Offset: 0x00021A23
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00022A2C File Offset: 0x00021A2C
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00022A35 File Offset: 0x00021A35
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00022A39 File Offset: 0x00021A39
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00022A42 File Offset: 0x00021A42
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00022A4B File Offset: 0x00021A4B
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00022A54 File Offset: 0x00021A54
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00022A5D File Offset: 0x00021A5D
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00022A66 File Offset: 0x00021A66
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00022A70 File Offset: 0x00021A70
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"Int32",
				"DateTime"
			}));
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00022AAE File Offset: 0x00021AAE
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x04000419 RID: 1049
		public const int MaxValue = 2147483647;

		// Token: 0x0400041A RID: 1050
		public const int MinValue = -2147483648;

		// Token: 0x0400041B RID: 1051
		internal int m_value;
	}
}
