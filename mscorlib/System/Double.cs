using System;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000AB RID: 171
	[ComVisible(true)]
	[Serializable]
	public struct Double : IComparable, IFormattable, IConvertible, IComparable<double>, IEquatable<double>
	{
		// Token: 0x06000A27 RID: 2599 RVA: 0x0001F458 File Offset: 0x0001E458
		public unsafe static bool IsInfinity(double d)
		{
			return (*(long*)(&d) & long.MaxValue) == 9218868437227405312L;
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0001F473 File Offset: 0x0001E473
		public static bool IsPositiveInfinity(double d)
		{
			return d == double.PositiveInfinity;
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0001F484 File Offset: 0x0001E484
		public static bool IsNegativeInfinity(double d)
		{
			return d == double.NegativeInfinity;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0001F495 File Offset: 0x0001E495
		internal unsafe static bool IsNegative(double d)
		{
			return (*(long*)(&d) & long.MinValue) == long.MinValue;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0001F4B0 File Offset: 0x0001E4B0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static bool IsNaN(double d)
		{
			return d != d;
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0001F4BC File Offset: 0x0001E4BC
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is double))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDouble"));
			}
			double num = (double)value;
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
			if (!double.IsNaN(this))
			{
				return 1;
			}
			if (!double.IsNaN(num))
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0001F518 File Offset: 0x0001E518
		public int CompareTo(double value)
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
			if (!double.IsNaN(this))
			{
				return 1;
			}
			if (!double.IsNaN(value))
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0001F548 File Offset: 0x0001E548
		public override bool Equals(object obj)
		{
			if (!(obj is double))
			{
				return false;
			}
			double num = (double)obj;
			return num == this || (double.IsNaN(num) && double.IsNaN(this));
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0001F57E File Offset: 0x0001E57E
		public bool Equals(double obj)
		{
			return obj == this || (double.IsNaN(obj) && double.IsNaN(this));
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0001F598 File Offset: 0x0001E598
		public unsafe override int GetHashCode()
		{
			double num = this;
			if (num == 0.0)
			{
				return 0;
			}
			long num2 = *(long*)(&num);
			return (int)num2 ^ (int)(num2 >> 32);
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0001F5C3 File Offset: 0x0001E5C3
		public override string ToString()
		{
			return Number.FormatDouble(this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0001F5D2 File Offset: 0x0001E5D2
		public string ToString(string format)
		{
			return Number.FormatDouble(this, format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0001F5E1 File Offset: 0x0001E5E1
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatDouble(this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0001F5F1 File Offset: 0x0001E5F1
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatDouble(this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0001F601 File Offset: 0x0001E601
		public static double Parse(string s)
		{
			return double.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0001F613 File Offset: 0x0001E613
		public static double Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return double.Parse(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0001F627 File Offset: 0x0001E627
		public static double Parse(string s, IFormatProvider provider)
		{
			return double.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x0001F63A File Offset: 0x0001E63A
		public static double Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return double.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0001F650 File Offset: 0x0001E650
		private static double Parse(string s, NumberStyles style, NumberFormatInfo info)
		{
			double result;
			try
			{
				result = Number.ParseDouble(s, style, info);
			}
			catch (FormatException)
			{
				string text = s.Trim();
				if (text.Equals(info.PositiveInfinitySymbol))
				{
					result = double.PositiveInfinity;
				}
				else if (text.Equals(info.NegativeInfinitySymbol))
				{
					result = double.NegativeInfinity;
				}
				else
				{
					if (!text.Equals(info.NaNSymbol))
					{
						throw;
					}
					result = double.NaN;
				}
			}
			return result;
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0001F6D4 File Offset: 0x0001E6D4
		public static bool TryParse(string s, out double result)
		{
			return double.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0001F6E7 File Offset: 0x0001E6E7
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out double result)
		{
			NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
			return double.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0001F700 File Offset: 0x0001E700
		private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out double result)
		{
			if (s == null)
			{
				result = 0.0;
				return false;
			}
			if (!Number.TryParseDouble(s, style, info, out result))
			{
				string text = s.Trim();
				if (text.Equals(info.PositiveInfinitySymbol))
				{
					result = double.PositiveInfinity;
				}
				else if (text.Equals(info.NegativeInfinitySymbol))
				{
					result = double.NegativeInfinity;
				}
				else
				{
					if (!text.Equals(info.NaNSymbol))
					{
						return false;
					}
					result = double.NaN;
				}
			}
			return true;
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0001F785 File Offset: 0x0001E785
		public TypeCode GetTypeCode()
		{
			return TypeCode.Double;
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0001F789 File Offset: 0x0001E789
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0001F794 File Offset: 0x0001E794
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"Double",
				"Char"
			}));
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0001F7D2 File Offset: 0x0001E7D2
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0001F7DB File Offset: 0x0001E7DB
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0001F7E4 File Offset: 0x0001E7E4
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0001F7ED File Offset: 0x0001E7ED
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0001F7F6 File Offset: 0x0001E7F6
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0001F7FF File Offset: 0x0001E7FF
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0001F808 File Offset: 0x0001E808
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0001F811 File Offset: 0x0001E811
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0001F81A File Offset: 0x0001E81A
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0001F823 File Offset: 0x0001E823
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0001F827 File Offset: 0x0001E827
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0001F830 File Offset: 0x0001E830
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"Double",
				"DateTime"
			}));
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0001F86E File Offset: 0x0001E86E
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x040003B9 RID: 953
		public const double MinValue = -1.7976931348623157E+308;

		// Token: 0x040003BA RID: 954
		public const double MaxValue = 1.7976931348623157E+308;

		// Token: 0x040003BB RID: 955
		public const double Epsilon = 5E-324;

		// Token: 0x040003BC RID: 956
		public const double NegativeInfinity = double.NegativeInfinity;

		// Token: 0x040003BD RID: 957
		public const double PositiveInfinity = double.PositiveInfinity;

		// Token: 0x040003BE RID: 958
		public const double NaN = double.NaN;

		// Token: 0x040003BF RID: 959
		internal double m_value;

		// Token: 0x040003C0 RID: 960
		internal static double NegativeZero = BitConverter.Int64BitsToDouble(long.MinValue);
	}
}
