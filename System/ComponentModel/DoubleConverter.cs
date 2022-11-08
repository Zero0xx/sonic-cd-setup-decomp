using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000D9 RID: 217
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class DoubleConverter : BaseNumberConverter
	{
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0001AB32 File Offset: 0x00019B32
		internal override bool AllowHex
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x0001AB35 File Offset: 0x00019B35
		internal override Type TargetType
		{
			get
			{
				return typeof(double);
			}
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0001AB41 File Offset: 0x00019B41
		internal override object FromString(string value, int radix)
		{
			return Convert.ToDouble(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001AB53 File Offset: 0x00019B53
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return double.Parse(value, NumberStyles.Float, formatInfo);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0001AB66 File Offset: 0x00019B66
		internal override object FromString(string value, CultureInfo culture)
		{
			return double.Parse(value, culture);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0001AB74 File Offset: 0x00019B74
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((double)value).ToString("R", formatInfo);
		}
	}
}
