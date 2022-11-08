using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000133 RID: 307
	[ComVisible(true)]
	[CLSCompliant(false)]
	[Serializable]
	public struct UInt64 : IComparable, IFormattable, IConvertible, IComparable<ulong>, IEquatable<ulong>
	{
		// Token: 0x06001106 RID: 4358 RVA: 0x0002F4D4 File Offset: 0x0002E4D4
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is ulong))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeUInt64"));
			}
			ulong num = (ulong)value;
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

		// Token: 0x06001107 RID: 4359 RVA: 0x0002F514 File Offset: 0x0002E514
		public int CompareTo(ulong value)
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

		// Token: 0x06001108 RID: 4360 RVA: 0x0002F525 File Offset: 0x0002E525
		public override bool Equals(object obj)
		{
			return obj is ulong && this == (ulong)obj;
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0002F53B File Offset: 0x0002E53B
		public bool Equals(ulong obj)
		{
			return this == obj;
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x0002F542 File Offset: 0x0002E542
		public override int GetHashCode()
		{
			return (int)this ^ (int)(this >> 32);
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x0002F54E File Offset: 0x0002E54E
		public override string ToString()
		{
			return Number.FormatUInt64(this, null, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x0002F55D File Offset: 0x0002E55D
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatUInt64(this, null, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x0002F56D File Offset: 0x0002E56D
		public string ToString(string format)
		{
			return Number.FormatUInt64(this, format, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x0002F57C File Offset: 0x0002E57C
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatUInt64(this, format, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x0002F58C File Offset: 0x0002E58C
		[CLSCompliant(false)]
		public static ulong Parse(string s)
		{
			return Number.ParseUInt64(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0002F59A File Offset: 0x0002E59A
		[CLSCompliant(false)]
		public static ulong Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseUInt64(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x0002F5AE File Offset: 0x0002E5AE
		[CLSCompliant(false)]
		public static ulong Parse(string s, IFormatProvider provider)
		{
			return Number.ParseUInt64(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x0002F5BD File Offset: 0x0002E5BD
		[CLSCompliant(false)]
		public static ulong Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseUInt64(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x0002F5D2 File Offset: 0x0002E5D2
		[CLSCompliant(false)]
		public static bool TryParse(string s, out ulong result)
		{
			return Number.TryParseUInt64(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x0002F5E1 File Offset: 0x0002E5E1
		[CLSCompliant(false)]
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ulong result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.TryParseUInt64(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x0002F5F7 File Offset: 0x0002E5F7
		public TypeCode GetTypeCode()
		{
			return TypeCode.UInt64;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x0002F5FB File Offset: 0x0002E5FB
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x0002F604 File Offset: 0x0002E604
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x0002F60D File Offset: 0x0002E60D
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x0002F616 File Offset: 0x0002E616
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0002F61F File Offset: 0x0002E61F
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x0002F628 File Offset: 0x0002E628
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0002F631 File Offset: 0x0002E631
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x0002F63A File Offset: 0x0002E63A
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0002F643 File Offset: 0x0002E643
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x0002F64C File Offset: 0x0002E64C
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x0002F650 File Offset: 0x0002E650
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x0002F659 File Offset: 0x0002E659
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x0002F662 File Offset: 0x0002E662
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x0002F66C File Offset: 0x0002E66C
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"UInt64",
				"DateTime"
			}));
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x0002F6AA File Offset: 0x0002E6AA
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x040005C0 RID: 1472
		public const ulong MaxValue = 18446744073709551615UL;

		// Token: 0x040005C1 RID: 1473
		public const ulong MinValue = 0UL;

		// Token: 0x040005C2 RID: 1474
		private ulong m_value;
	}
}
