using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000078 RID: 120
	[ComVisible(true)]
	[Serializable]
	public struct Boolean : IComparable, IConvertible, IComparable<bool>, IEquatable<bool>
	{
		// Token: 0x060006AA RID: 1706 RVA: 0x000165AD File Offset: 0x000155AD
		public override int GetHashCode()
		{
			if (!this)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x000165B6 File Offset: 0x000155B6
		public override string ToString()
		{
			if (!this)
			{
				return "False";
			}
			return "True";
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x000165C7 File Offset: 0x000155C7
		public string ToString(IFormatProvider provider)
		{
			if (!this)
			{
				return "False";
			}
			return "True";
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x000165D8 File Offset: 0x000155D8
		public override bool Equals(object obj)
		{
			return obj is bool && this == (bool)obj;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x000165EE File Offset: 0x000155EE
		public bool Equals(bool obj)
		{
			return this == obj;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x000165F5 File Offset: 0x000155F5
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is bool))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeBoolean"));
			}
			if (this == (bool)obj)
			{
				return 0;
			}
			if (!this)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00016627 File Offset: 0x00015627
		public int CompareTo(bool value)
		{
			if (this == value)
			{
				return 0;
			}
			if (!this)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00016638 File Offset: 0x00015638
		public static bool Parse(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			bool result = false;
			if (!bool.TryParse(value, out result))
			{
				throw new FormatException(Environment.GetResourceString("Format_BadBoolean"));
			}
			return result;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00016670 File Offset: 0x00015670
		public static bool TryParse(string value, out bool result)
		{
			result = false;
			if (value == null)
			{
				return false;
			}
			if ("True".Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				result = true;
				return true;
			}
			if ("False".Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				result = false;
				return true;
			}
			if (bool.m_trimmableChars == null)
			{
				char[] array = new char[string.WhitespaceChars.Length + 1];
				Array.Copy(string.WhitespaceChars, array, string.WhitespaceChars.Length);
				array[array.Length - 1] = '\0';
				bool.m_trimmableChars = array;
			}
			value = value.Trim(bool.m_trimmableChars);
			if ("True".Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				result = true;
				return true;
			}
			if ("False".Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				result = false;
				return true;
			}
			return false;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00016715 File Offset: 0x00015715
		public TypeCode GetTypeCode()
		{
			return TypeCode.Boolean;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00016718 File Offset: 0x00015718
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001671C File Offset: 0x0001571C
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"Boolean",
				"Char"
			}));
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0001675A File Offset: 0x0001575A
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00016763 File Offset: 0x00015763
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001676C File Offset: 0x0001576C
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00016775 File Offset: 0x00015775
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001677E File Offset: 0x0001577E
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00016787 File Offset: 0x00015787
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00016790 File Offset: 0x00015790
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00016799 File Offset: 0x00015799
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x000167A2 File Offset: 0x000157A2
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x000167AB File Offset: 0x000157AB
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000167B4 File Offset: 0x000157B4
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x000167C0 File Offset: 0x000157C0
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidCast_FromTo"), new object[]
			{
				"Boolean",
				"DateTime"
			}));
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x000167FE File Offset: 0x000157FE
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x04000216 RID: 534
		internal const int True = 1;

		// Token: 0x04000217 RID: 535
		internal const int False = 0;

		// Token: 0x04000218 RID: 536
		internal const string TrueLiteral = "True";

		// Token: 0x04000219 RID: 537
		internal const string FalseLiteral = "False";

		// Token: 0x0400021A RID: 538
		private bool m_value;

		// Token: 0x0400021B RID: 539
		private static char[] m_trimmableChars;

		// Token: 0x0400021C RID: 540
		public static readonly string TrueString = "True";

		// Token: 0x0400021D RID: 541
		public static readonly string FalseString = "False";
	}
}
