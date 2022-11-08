using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000A3 RID: 163
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class BaseNumberConverter : TypeConverter
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0001798E File Offset: 0x0001698E
		internal virtual bool AllowHex
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060005D3 RID: 1491
		internal abstract Type TargetType { get; }

		// Token: 0x060005D4 RID: 1492
		internal abstract object FromString(string value, int radix);

		// Token: 0x060005D5 RID: 1493
		internal abstract object FromString(string value, NumberFormatInfo formatInfo);

		// Token: 0x060005D6 RID: 1494
		internal abstract object FromString(string value, CultureInfo culture);

		// Token: 0x060005D7 RID: 1495 RVA: 0x00017994 File Offset: 0x00016994
		internal virtual Exception FromStringError(string failedText, Exception innerException)
		{
			return new Exception(SR.GetString("ConvertInvalidPrimitive", new object[]
			{
				failedText,
				this.TargetType.Name
			}), innerException);
		}

		// Token: 0x060005D8 RID: 1496
		internal abstract string ToString(object value, NumberFormatInfo formatInfo);

		// Token: 0x060005D9 RID: 1497 RVA: 0x000179CB File Offset: 0x000169CB
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x000179E4 File Offset: 0x000169E4
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				try
				{
					if (this.AllowHex && text[0] == '#')
					{
						return this.FromString(text.Substring(1), 16);
					}
					if ((this.AllowHex && text.StartsWith("0x")) || text.StartsWith("0X") || text.StartsWith("&h") || text.StartsWith("&H"))
					{
						return this.FromString(text.Substring(2), 16);
					}
					if (culture == null)
					{
						culture = CultureInfo.CurrentCulture;
					}
					NumberFormatInfo formatInfo = (NumberFormatInfo)culture.GetFormat(typeof(NumberFormatInfo));
					return this.FromString(text, formatInfo);
				}
				catch (Exception innerException)
				{
					throw this.FromStringError(text, innerException);
				}
				catch
				{
					throw;
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00017AE0 File Offset: 0x00016AE0
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string) && value != null && this.TargetType.IsInstanceOfType(value))
			{
				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}
				NumberFormatInfo formatInfo = (NumberFormatInfo)culture.GetFormat(typeof(NumberFormatInfo));
				return this.ToString(value, formatInfo);
			}
			if (destinationType.IsPrimitive)
			{
				return Convert.ChangeType(value, destinationType, culture);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00017B62 File Offset: 0x00016B62
		public override bool CanConvertTo(ITypeDescriptorContext context, Type t)
		{
			return base.CanConvertTo(context, t) || t.IsPrimitive;
		}
	}
}
