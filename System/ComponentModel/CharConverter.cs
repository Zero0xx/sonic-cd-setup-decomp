using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000B0 RID: 176
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class CharConverter : TypeConverter
	{
		// Token: 0x06000651 RID: 1617 RVA: 0x0001853E File Offset: 0x0001753E
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00018557 File Offset: 0x00017557
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value is char && (char)value == '\0')
			{
				return "";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00018588 File Offset: 0x00017588
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (!(value is string))
			{
				return base.ConvertFrom(context, culture, value);
			}
			string text = (string)value;
			if (text.Length > 1)
			{
				text = text.Trim();
			}
			if (text == null || text.Length <= 0)
			{
				return '\0';
			}
			if (text.Length != 1)
			{
				throw new FormatException(SR.GetString("ConvertInvalidPrimitive", new object[]
				{
					text,
					"Char"
				}));
			}
			return text[0];
		}
	}
}
