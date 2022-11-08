using System;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000117 RID: 279
	[ComVisible(true)]
	[Serializable]
	public struct Single : IComparable, IFormattable, IConvertible, IComparable<float>, IEquatable<float>
	{
		// Token: 0x06001003 RID: 4099 RVA: 0x0002DB17 File Offset: 0x0002CB17
		public unsafe static bool IsInfinity(float f)
		{
			return (*(int*)(&f) & int.MaxValue) == 2139095040;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0002DB2A File Offset: 0x0002CB2A
		public unsafe static bool IsPositiveInfinity(float f)
		{
			return *(int*)(&f) == 2139095040;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0002DB37 File Offset: 0x0002CB37
		public unsafe static bool IsNegativeInfinity(float f)
		{
			return *(int*)(&f) == -8388608;
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0002DB44 File Offset: 0x0002CB44
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static bool IsNaN(float f)
		{
			return f != f;
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0002DB50 File Offset: 0x0002CB50
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is float))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeSingle"));
			}
			float num = (float)value;
			if (this < num)
			{
				return -1;
			}
			if (this > num)
			{
				return 1;
			}
			if (this == num)
			{
				return 0;
			}
			if (!float.IsNaN(this))
			{
				return 1;
			}
			if (!float.IsNaN(num))
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0002DBAC File Offset: 0x0002CBAC
		public int CompareTo(float value)
		{
			if (this < value)
			{
				return -1;
			}
			if (this > value)
			{
				return 1;
			}
			if (this == value)
			{
				return 0;
			}
			if (!float.IsNaN(this))
			{
				return 1;
			}
			if (!float.IsNaN(value))
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0002DBDC File Offset: 0x0002CBDC
		public override bool Equals(object obj)
		{
			if (!(obj is float))
			{
				return false;
			}
			float num = (float)obj;
			return num == this || (float.IsNaN(num) && float.IsNaN(this));
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0002DC12 File Offset: 0x0002CC12
		public bool Equals(float obj)
		{
			return obj == this || (float.IsNaN(obj) && float.IsNaN(this));
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0002DC2C File Offset: 0x0002CC2C
		public unsafe override int GetHashCode()
		{
			float num = this;
			if (num == 0f)
			{
				return 0;
			}
			return *(int*)(&num);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0002DC4C File Offset: 0x0002CC4C
		public override string ToString()
		{
			return Number.FormatSingle(this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0002DC5B File Offset: 0x0002CC5B
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatSingle(this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0002DC6B File Offset: 0x0002CC6B
		public string ToString(string format)
		{
			return Number.FormatSingle(this, format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0002DC7A File Offset: 0x0002CC7A
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatSingle(this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0002DC8A File Offset: 0x0002CC8A
		public static float Parse(string s)
		{
			return float.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0002DC9C File Offset: 0x0002CC9C
		public static float Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return float.Parse(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0002DCB0 File Offset: 0x0002CCB0
		public static float Parse(string s, IFormatProvider provider)
		{
			return float.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0002DCC3 File Offset: 0x0002CCC3
		public static float Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return float.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0002DCD8 File Offset: 0x0002CCD8
		private static float Parse(string s, NumberStyles style, NumberFormatInfo info)
		{
			float result;
			try
			{
				result = Number.ParseSingle(s, style, info);
			}
			catch (FormatException)
			{
				string text = s.Trim();
				if (text.Equals(info.PositiveInfinitySymbol))
				{
					result = float.PositiveInfinity;
				}
				else if (text.Equals(info.NegativeInfinitySymbol))
				{
					result = float.NegativeInfinity;
				}
				else
				{
					if (!text.Equals(info.NaNSymbol))
					{
						throw;
					}
					result = float.NaN;
				}
			}
			return result;
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0002DD50 File Offset: 0x0002CD50
		public static bool TryParse(string s, out float result)
		{
			return float.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0002DD63 File Offset: 0x0002CD63
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out float result)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return float.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0002DD7C File Offset: 0x0002CD7C
		private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out float result)
		{
			if (s == null)
			{
				result = 0f;
				return false;
			}
			if (!Number.TryParseSingle(s, style, info, out result))
			{
				string text = s.Trim();
				if (text.Equals(info.PositiveInfinitySymbol))
				{
					result = float.PositiveInfinity;
				}
				else if (text.Equals(info.NegativeInfinitySymbol))
				{
					result = float.NegativeInfinity;
				}
				else
				{
					if (!text.Equals(info.NaNSymbol))
					{
						return false;
					}
					result = float.NaN;
				}
			}
			return true;
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0002DDF1 File Offset: 0x0002CDF1
		public TypeCode GetTypeCode()
		{
			return TypeCode.Single;
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0002DDF5 File Offset: 0x0002CDF5
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0002DE00 File Offset: 0x0002CE00
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"Single",
				"Char"
			}));
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0002DE3E File Offset: 0x0002CE3E
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0002DE47 File Offset: 0x0002CE47
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0002DE50 File Offset: 0x0002CE50
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0002DE59 File Offset: 0x0002CE59
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0002DE62 File Offset: 0x0002CE62
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0002DE6B File Offset: 0x0002CE6B
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0002DE74 File Offset: 0x0002CE74
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0002DE7D File Offset: 0x0002CE7D
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0002DE86 File Offset: 0x0002CE86
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0002DE8A File Offset: 0x0002CE8A
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0002DE93 File Offset: 0x0002CE93
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0002DE9C File Offset: 0x0002CE9C
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"Single",
				"DateTime"
			}));
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0002DEDA File Offset: 0x0002CEDA
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x0400056E RID: 1390
		public const float MinValue = -3.4028235E+38f;

		// Token: 0x0400056F RID: 1391
		public const float Epsilon = 1E-45f;

		// Token: 0x04000570 RID: 1392
		public const float MaxValue = 3.4028235E+38f;

		// Token: 0x04000571 RID: 1393
		public const float PositiveInfinity = float.PositiveInfinity;

		// Token: 0x04000572 RID: 1394
		public const float NegativeInfinity = float.NegativeInfinity;

		// Token: 0x04000573 RID: 1395
		public const float NaN = float.NaN;

		// Token: 0x04000574 RID: 1396
		internal float m_value;
	}
}
