using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000132 RID: 306
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Serializable]
	public struct UInt32 : IComparable, IFormattable, IConvertible, IComparable<uint>, IEquatable<uint>
	{
		// Token: 0x060010E7 RID: 4327 RVA: 0x0002F2F4 File Offset: 0x0002E2F4
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is uint))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeUInt32"));
			}
			uint num = (uint)value;
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

		// Token: 0x060010E8 RID: 4328 RVA: 0x0002F334 File Offset: 0x0002E334
		public int CompareTo(uint value)
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

		// Token: 0x060010E9 RID: 4329 RVA: 0x0002F345 File Offset: 0x0002E345
		public override bool Equals(object obj)
		{
			return obj is uint && this == (uint)obj;
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x0002F35B File Offset: 0x0002E35B
		public bool Equals(uint obj)
		{
			return this == obj;
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0002F362 File Offset: 0x0002E362
		public override int GetHashCode()
		{
			return (int)this;
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0002F366 File Offset: 0x0002E366
		public override string ToString()
		{
			return Number.FormatUInt32(this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x0002F375 File Offset: 0x0002E375
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatUInt32(this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x0002F385 File Offset: 0x0002E385
		public string ToString(string format)
		{
			return Number.FormatUInt32(this, format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0002F394 File Offset: 0x0002E394
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatUInt32(this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x0002F3A4 File Offset: 0x0002E3A4
		[CLSCompliant(false)]
		public static uint Parse(string s)
		{
			return Number.ParseUInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x0002F3B2 File Offset: 0x0002E3B2
		[CLSCompliant(false)]
		public static uint Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseUInt32(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0002F3C6 File Offset: 0x0002E3C6
		[CLSCompliant(false)]
		public static uint Parse(string s, IFormatProvider provider)
		{
			return Number.ParseUInt32(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0002F3D5 File Offset: 0x0002E3D5
		[CLSCompliant(false)]
		public static uint Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseUInt32(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0002F3EA File Offset: 0x0002E3EA
		[CLSCompliant(false)]
		public static bool TryParse(string s, out uint result)
		{
			return Number.TryParseUInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0002F3F9 File Offset: 0x0002E3F9
		[CLSCompliant(false)]
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out uint result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.TryParseUInt32(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0002F40F File Offset: 0x0002E40F
		public TypeCode GetTypeCode()
		{
			return TypeCode.UInt32;
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0002F413 File Offset: 0x0002E413
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0002F41C File Offset: 0x0002E41C
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0002F425 File Offset: 0x0002E425
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x0002F42E File Offset: 0x0002E42E
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0002F437 File Offset: 0x0002E437
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0002F440 File Offset: 0x0002E440
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0002F449 File Offset: 0x0002E449
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0002F452 File Offset: 0x0002E452
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0002F456 File Offset: 0x0002E456
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0002F45F File Offset: 0x0002E45F
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x0002F468 File Offset: 0x0002E468
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x0002F471 File Offset: 0x0002E471
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0002F47A File Offset: 0x0002E47A
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0002F484 File Offset: 0x0002E484
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"UInt32",
				"DateTime"
			}));
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x0002F4C2 File Offset: 0x0002E4C2
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x040005BD RID: 1469
		public const uint MaxValue = 4294967295U;

		// Token: 0x040005BE RID: 1470
		public const uint MinValue = 0U;

		// Token: 0x040005BF RID: 1471
		private uint m_value;
	}
}
