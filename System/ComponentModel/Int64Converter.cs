using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000FC RID: 252
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class Int64Converter : BaseNumberConverter
	{
		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x0001C0C9 File Offset: 0x0001B0C9
		internal override Type TargetType
		{
			get
			{
				return typeof(long);
			}
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0001C0D5 File Offset: 0x0001B0D5
		internal override object FromString(string value, int radix)
		{
			return Convert.ToInt64(value, radix);
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0001C0E3 File Offset: 0x0001B0E3
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return long.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0001C0F2 File Offset: 0x0001B0F2
		internal override object FromString(string value, CultureInfo culture)
		{
			return long.Parse(value, culture);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0001C100 File Offset: 0x0001B100
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((long)value).ToString("G", formatInfo);
		}
	}
}
