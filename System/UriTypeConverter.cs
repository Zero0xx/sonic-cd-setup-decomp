using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System
{
	// Token: 0x02000365 RID: 869
	public class UriTypeConverter : TypeConverter
	{
		// Token: 0x06001BA1 RID: 7073 RVA: 0x000685A3 File Offset: 0x000675A3
		public UriTypeConverter() : this(UriKind.RelativeOrAbsolute)
		{
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x000685AC File Offset: 0x000675AC
		internal UriTypeConverter(UriKind uriKind)
		{
			this.m_UriKind = uriKind;
		}

		// Token: 0x06001BA3 RID: 7075 RVA: 0x000685BB File Offset: 0x000675BB
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == null)
			{
				throw new ArgumentNullException("sourceType");
			}
			return sourceType == typeof(string) || typeof(Uri).IsAssignableFrom(sourceType) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x000685F6 File Offset: 0x000675F6
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || destinationType == typeof(string) || destinationType == typeof(Uri) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x00068630 File Offset: 0x00067630
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				return new Uri(text, this.m_UriKind);
			}
			Uri uri = value as Uri;
			if (uri != null)
			{
				return new Uri(uri.OriginalString, (this.m_UriKind == UriKind.RelativeOrAbsolute) ? (uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative) : this.m_UriKind);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x06001BA6 RID: 7078 RVA: 0x00068698 File Offset: 0x00067698
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			Uri uri = value as Uri;
			if (uri != null && destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo constructor = typeof(Uri).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[]
				{
					typeof(string),
					typeof(UriKind)
				}, null);
				return new InstanceDescriptor(constructor, new object[]
				{
					uri.OriginalString,
					(this.m_UriKind == UriKind.RelativeOrAbsolute) ? (uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative) : this.m_UriKind
				});
			}
			if (uri != null && destinationType == typeof(string))
			{
				return uri.OriginalString;
			}
			if (uri != null && destinationType == typeof(Uri))
			{
				return new Uri(uri.OriginalString, (this.m_UriKind == UriKind.RelativeOrAbsolute) ? (uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative) : this.m_UriKind);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x06001BA7 RID: 7079 RVA: 0x000687A0 File Offset: 0x000677A0
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			string text = value as string;
			if (text != null)
			{
				Uri uri;
				return Uri.TryCreate(text, this.m_UriKind, out uri);
			}
			return value is Uri;
		}

		// Token: 0x04001C4D RID: 7245
		private UriKind m_UriKind;
	}
}
