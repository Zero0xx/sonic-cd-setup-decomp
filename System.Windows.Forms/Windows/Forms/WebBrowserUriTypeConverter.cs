using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x0200073A RID: 1850
	internal class WebBrowserUriTypeConverter : UriTypeConverter
	{
		// Token: 0x06006299 RID: 25241 RVA: 0x00166878 File Offset: 0x00165878
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			Uri uri = base.ConvertFrom(context, culture, value) as Uri;
			if (uri != null && !string.IsNullOrEmpty(uri.OriginalString) && !uri.IsAbsoluteUri)
			{
				try
				{
					uri = new Uri("http://" + uri.OriginalString.Trim());
				}
				catch (UriFormatException)
				{
				}
			}
			return uri;
		}
	}
}
