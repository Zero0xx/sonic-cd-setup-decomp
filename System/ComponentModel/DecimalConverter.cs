using System;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000CA RID: 202
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class DecimalConverter : BaseNumberConverter
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0001A14B File Offset: 0x0001914B
		internal override bool AllowHex
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0001A14E File Offset: 0x0001914E
		internal override Type TargetType
		{
			get
			{
				return typeof(decimal);
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0001A15A File Offset: 0x0001915A
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0001A174 File Offset: 0x00019174
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType != typeof(InstanceDescriptor) || !(value is decimal))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			object[] arguments = new object[]
			{
				decimal.GetBits((decimal)value)
			};
			MemberInfo constructor = typeof(decimal).GetConstructor(new Type[]
			{
				typeof(int[])
			});
			if (constructor != null)
			{
				return new InstanceDescriptor(constructor, arguments);
			}
			return null;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0001A1FA File Offset: 0x000191FA
		internal override object FromString(string value, int radix)
		{
			return Convert.ToDecimal(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0001A20C File Offset: 0x0001920C
		internal override object FromString(string value, NumberFormatInfo formatInfo)
		{
			return decimal.Parse(value, NumberStyles.Float, formatInfo);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0001A21F File Offset: 0x0001921F
		internal override object FromString(string value, CultureInfo culture)
		{
			return decimal.Parse(value, culture);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0001A230 File Offset: 0x00019230
		internal override string ToString(object value, NumberFormatInfo formatInfo)
		{
			return ((decimal)value).ToString("G", formatInfo);
		}
	}
}
