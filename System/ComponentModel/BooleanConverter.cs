using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000AB RID: 171
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class BooleanConverter : TypeConverter
	{
		// Token: 0x06000637 RID: 1591 RVA: 0x0001834C File Offset: 0x0001734C
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00018368 File Offset: 0x00017368
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string value2 = ((string)value).Trim();
				try
				{
					return bool.Parse(value2);
				}
				catch (FormatException innerException)
				{
					throw new FormatException(SR.GetString("ConvertInvalidPrimitive", new object[]
					{
						(string)value,
						"Boolean"
					}), innerException);
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x000183E0 File Offset: 0x000173E0
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (BooleanConverter.values == null)
			{
				BooleanConverter.values = new TypeConverter.StandardValuesCollection(new object[]
				{
					true,
					false
				});
			}
			return BooleanConverter.values;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001841D File Offset: 0x0001741D
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00018420 File Offset: 0x00017420
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x04000906 RID: 2310
		private static TypeConverter.StandardValuesCollection values;
	}
}
