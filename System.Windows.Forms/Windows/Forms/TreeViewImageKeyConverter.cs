using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x0200070A RID: 1802
	public class TreeViewImageKeyConverter : ImageKeyConverter
	{
		// Token: 0x0600603F RID: 24639 RVA: 0x0015EEB4 File Offset: 0x0015DEB4
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value == null)
			{
				return SR.GetString("toStringDefault");
			}
			string text = value as string;
			if (text != null && text.Length == 0)
			{
				return SR.GetString("toStringDefault");
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
