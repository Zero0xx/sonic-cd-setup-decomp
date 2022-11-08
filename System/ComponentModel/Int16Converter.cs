using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000FA RID: 250
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class Int16Converter : BaseNumberConverter
	{
		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x0001C008 File Offset: 0x0001B008
		internal override Type TargetType
		{
			get
			{
				return typeof(short);
			}
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0001C014 File Offset: 0x0001B014
		internal override object FromString(string value, int radix)
		{
			return Convert.ToInt16(value, radix);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0001C022 File Offset: 0x0001B022
		internal override object FromString(string value, CultureInfo culture)
		{
			return short.Parse(value, culture);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0001C030 File Offset: 0x0001B030
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return short.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0001C040 File Offset: 0x0001B040
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((short)value).ToString("G", formatInfo);
		}
	}
}
