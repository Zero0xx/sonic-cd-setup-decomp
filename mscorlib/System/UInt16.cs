using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000131 RID: 305
	[ComVisible(true)]
	[CLSCompliant(false)]
	[Serializable]
	public struct UInt16 : IComparable, IFormattable, IConvertible, IComparable<ushort>, IEquatable<ushort>
	{
		// Token: 0x060010C6 RID: 4294 RVA: 0x0002F0B0 File Offset: 0x0002E0B0
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (value is ushort)
			{
				return (int)(this - (ushort)value);
			}
			throw new ArgumentException(Environment.GetResourceString("Arg_MustBeUInt16"));
		}

		// Token: 0x060010C7 RID: 4295 RVA: 0x0002F0D8 File Offset: 0x0002E0D8
		public int CompareTo(ushort value)
		{
			return (int)(this - value);
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x0002F0DE File Offset: 0x0002E0DE
		public override bool Equals(object obj)
		{
			return obj is ushort && this == (ushort)obj;
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0002F0F4 File Offset: 0x0002E0F4
		public bool Equals(ushort obj)
		{
			return this == obj;
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x0002F0FB File Offset: 0x0002E0FB
		public override int GetHashCode()
		{
			return (int)this;
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x0002F0FF File Offset: 0x0002E0FF
		public override string ToString()
		{
			return Number.FormatUInt32((uint)this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x0002F10E File Offset: 0x0002E10E
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatUInt32((uint)this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060010CD RID: 4301 RVA: 0x0002F11E File Offset: 0x0002E11E
		public string ToString(string format)
		{
			return Number.FormatUInt32((uint)this, format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060010CE RID: 4302 RVA: 0x0002F12D File Offset: 0x0002E12D
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatUInt32((uint)this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x0002F13D File Offset: 0x0002E13D
		[CLSCompliant(false)]
		public static ushort Parse(string s)
		{
			return ushort.Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x0002F14B File Offset: 0x0002E14B
		[CLSCompliant(false)]
		public static ushort Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return ushort.Parse(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x0002F15F File Offset: 0x0002E15F
		[CLSCompliant(false)]
		public static ushort Parse(string s, IFormatProvider provider)
		{
			return ushort.Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0002F16E File Offset: 0x0002E16E
		[CLSCompliant(false)]
		public static ushort Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return ushort.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x0002F184 File Offset: 0x0002E184
		private static ushort Parse(string s, NumberStyles style, NumberFormatInfo info)
		{
			uint num = 0U;
			try
			{
				num = Number.ParseUInt32(s, style, info);
			}
			catch (OverflowException innerException)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"), innerException);
			}
			if (num > 65535U)
			{
				throw new OverflowException(Environment.GetResourceString("Overflow_UInt16"));
			}
			return (ushort)num;
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x0002F1DC File Offset: 0x0002E1DC
		[CLSCompliant(false)]
		public static bool TryParse(string s, out ushort result)
		{
			return ushort.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x0002F1EB File Offset: 0x0002E1EB
		[CLSCompliant(false)]
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ushort result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return ushort.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x0002F204 File Offset: 0x0002E204
		private static bool TryParse(string s, NumberStyles style, NumberFormatInfo info, out ushort result)
		{
			result = 0;
			uint num;
			if (!Number.TryParseUInt32(s, style, info, out num))
			{
				return false;
			}
			if (num > 65535U)
			{
				return false;
			}
			result = (ushort)num;
			return true;
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x0002F231 File Offset: 0x0002E231
		public TypeCode GetTypeCode()
		{
			return TypeCode.UInt16;
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x0002F234 File Offset: 0x0002E234
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x0002F23D File Offset: 0x0002E23D
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x0002F246 File Offset: 0x0002E246
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x0002F24F File Offset: 0x0002E24F
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0002F258 File Offset: 0x0002E258
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0002F261 File Offset: 0x0002E261
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x0002F265 File Offset: 0x0002E265
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0002F26E File Offset: 0x0002E26E
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0002F277 File Offset: 0x0002E277
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x0002F280 File Offset: 0x0002E280
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x0002F289 File Offset: 0x0002E289
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x0002F292 File Offset: 0x0002E292
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x0002F29B File Offset: 0x0002E29B
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x0002F2A4 File Offset: 0x0002E2A4
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"UInt16",
				"DateTime"
			}));
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x0002F2E2 File Offset: 0x0002E2E2
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x040005BA RID: 1466
		public const ushort MaxValue = 65535;

		// Token: 0x040005BB RID: 1467
		public const ushort MinValue = 0;

		// Token: 0x040005BC RID: 1468
		private ushort m_value;
	}
}
