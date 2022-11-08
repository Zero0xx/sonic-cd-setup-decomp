using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000AD RID: 173
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ByteConverter : BaseNumberConverter
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x000184B5 File Offset: 0x000174B5
		internal override Type TargetType
		{
			get
			{
				return typeof(byte);
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x000184C1 File Offset: 0x000174C1
		internal override object FromString(string value, int radix)
		{
			return Convert.ToByte(value, radix);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x000184CF File Offset: 0x000174CF
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return byte.Parse(value, NumberStyles.Integer, formatInfo);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x000184DE File Offset: 0x000174DE
		internal override object FromString(string value, CultureInfo culture)
		{
			return byte.Parse(value, culture);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x000184EC File Offset: 0x000174EC
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((byte)value).ToString("G", formatInfo);
		}
	}
}
