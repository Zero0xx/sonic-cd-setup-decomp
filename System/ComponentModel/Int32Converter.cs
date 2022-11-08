using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000FB RID: 251
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class Int32Converter : BaseNumberConverter
	{
		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x0001C069 File Offset: 0x0001B069
		internal override Type TargetType
		{
			get
			{
				return typeof(int);
			}
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0001C075 File Offset: 0x0001B075
		internal override object FromString(string value, int radix)
		{
			return Convert.ToInt32(value, radix);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001C083 File Offset: 0x0001B083
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return int.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0001C092 File Offset: 0x0001B092
		internal override object FromString(string value, CultureInfo culture)
		{
			return int.Parse(value, culture);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001C0A0 File Offset: 0x0001B0A0
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((int)value).ToString("G", formatInfo);
		}
	}
}
