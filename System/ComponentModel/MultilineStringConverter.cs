using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000121 RID: 289
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class MultilineStringConverter : TypeConverter
	{
		// Token: 0x06000939 RID: 2361 RVA: 0x0001F0AC File Offset: 0x0001E0AC
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value is string)
			{
				return SR.GetString("MultilineStringConverterText");
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0001F0E9 File Offset: 0x0001E0E9
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return null;
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0001F0EC File Offset: 0x0001E0EC
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return false;
		}
	}
}
