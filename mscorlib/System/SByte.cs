using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000114 RID: 276
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Serializable]
	public struct SByte : IComparable, IFormattable, IConvertible, IComparable<sbyte>, IEquatable<sbyte>
	{
		// Token: 0x06000FD7 RID: 4055 RVA: 0x0002D683 File Offset: 0x0002C683
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is sbyte))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeSByte"));
			}
			return (int)(this - (sbyte)obj);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0002D6AB File Offset: 0x0002C6AB
		public int CompareTo(sbyte value)
		{
			return (int)(this - value);
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0002D6B1 File Offset: 0x0002C6B1
		public override bool Equals(object obj)
		{
			return obj is sbyte && this == (sbyte)obj;
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0002D6C7 File Offset: 0x0002C6C7
		public bool Equals(sbyte obj)
		{
			return this == obj;
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0002D6CE File Offset: 0x0002C6CE
		public override int GetHashCode()
		{
			return (int)this ^ (int)this << 8;
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0002D6D7 File Offset: 0x0002C6D7
		public override string ToString()
		{
			return Number.FormatInt32((int)this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0002D6E6 File Offset: 0x0002C6E6
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatInt32((int)this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0002D6F6 File Offset: 0x0002C6F6
		public string ToString(string format)
		{
			return this.ToString(format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0002D704 File Offset: 0x0002C704
		public string ToString(string format, IFormatProvider provider)
		{
			return this.ToString(format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0002D714 File Offset: 0x0002C714
		private string ToString(string format, NumberFormatInfo info)
		{
			if (this < 0 && format != null && format.Length > 0 && (format[0] == 'X' || format[0] == 'x'))
			{
				uint value = (uint)this & 255U;
				return Number.FormatUInt32(value, format, info);
			}
			return Number.FormatInt32((int)this, format, info);
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0002D763 File Offset: 0x0002C763
		[CLSCompliant(false)]
		public static sbyte Parse(string s)
		{
			return sbyte.Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0002D771 File Offset: 0x0002C771
		[CLSCompliant(false)]
		public static sbyte Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return sbyte.Parse(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0002D785 File Offset: 0x0002C785
		[CLSCompliant(false)]
		public static sbyte Parse(string s, IFormatProvider provider)
		{
			return sbyte.Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0002D794 File Offset: 0x0002C794
		[CLSCompliant(false)]
		public static sbyte Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return sbyte.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0002D7AC File Offset: 0x0002C7AC
		private static sbyte Parse(string s, NumberStyles style, NumberFormatInfo info)
		{
			int num = 0;
			try
			{
				num = Number.ParseInt32(s, style, info);
			}
			catch (OverflowException innerException)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_SByte"), innerException);
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (num < 0 || num > 255)
				{
					throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
				}
				return (sbyte)num;
			}
			else
			{
				if (num < -128 || num > 127)
				{
					throw new OverflowException(Environment.GetResourceString("Overflow_SByte"));
				}
				return (sbyte)num;
			}
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0002D82C File Offset: 0x0002C82C
		[CLSCompliant(false)]
		public static bool TryParse(string s, out sbyte result)
		{
			return sbyte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x0002D83B File Offset: 0x0002C83B
		[CLSCompliant(false)]
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out sbyte result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return sbyte.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0002D854 File Offset: 0x0002C854
		private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out sbyte result)
		{
			result = 0;
			int num;
			if (!Number.TryParseInt32(s, style, info, out num))
			{
				return false;
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (num < 0 || num > 255)
				{
					return false;
				}
				result = (sbyte)num;
				return true;
			}
			else
			{
				if (num < -128 || num > 127)
				{
					return false;
				}
				result = (sbyte)num;
				return true;
			}
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x0002D8A0 File Offset: 0x0002C8A0
		public TypeCode GetTypeCode()
		{
			return TypeCode.SByte;
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0002D8A3 File Offset: 0x0002C8A3
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0002D8AC File Offset: 0x0002C8AC
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0002D8B5 File Offset: 0x0002C8B5
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0002D8B9 File Offset: 0x0002C8B9
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0002D8C2 File Offset: 0x0002C8C2
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0002D8CB File Offset: 0x0002C8CB
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0002D8D4 File Offset: 0x0002C8D4
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return (int)this;
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0002D8D8 File Offset: 0x0002C8D8
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0002D8E1 File Offset: 0x0002C8E1
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0002D8EA File Offset: 0x0002C8EA
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0002D8F3 File Offset: 0x0002C8F3
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x0002D8FC File Offset: 0x0002C8FC
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0002D905 File Offset: 0x0002C905
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0002D910 File Offset: 0x0002C910
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(Environment.GetResourceString("InvalidCast_FromTo", new object[]
			{
				"SByte",
				"DateTime"
			}));
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0002D944 File Offset: 0x0002C944
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x04000566 RID: 1382
		public const sbyte MaxValue = 127;

		// Token: 0x04000567 RID: 1383
		public const sbyte MinValue = -128;

		// Token: 0x04000568 RID: 1384
		private sbyte m_value;
	}
}
