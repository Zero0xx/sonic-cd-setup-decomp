using System;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;

namespace System.ComponentModel
{
	// Token: 0x020000C0 RID: 192
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class CultureInfoConverter : TypeConverter
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x00019372 File Offset: 0x00018372
		private string DefaultCultureString
		{
			get
			{
				return SR.GetString("CultureInfoConverterDefaultCultureString");
			}
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0001937E File Offset: 0x0001837E
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00019397 File Offset: 0x00018397
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x000193B0 File Offset: 0x000183B0
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (!(value is string))
			{
				return base.ConvertFrom(context, culture, value);
			}
			string text = (string)value;
			CultureInfo cultureInfo = null;
			CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
			if (culture != null && culture.Equals(CultureInfo.InvariantCulture))
			{
				Thread.CurrentThread.CurrentUICulture = culture;
			}
			try
			{
				if (text == null || text.Length == 0 || string.Compare(text, this.DefaultCultureString, StringComparison.Ordinal) == 0)
				{
					cultureInfo = CultureInfo.InvariantCulture;
				}
				if (cultureInfo == null)
				{
					ICollection standardValues = this.GetStandardValues(context);
					foreach (object obj in standardValues)
					{
						CultureInfo cultureInfo2 = (CultureInfo)obj;
						if (cultureInfo2 != null && string.Compare(cultureInfo2.DisplayName, text, StringComparison.Ordinal) == 0)
						{
							cultureInfo = cultureInfo2;
							break;
						}
					}
				}
				if (cultureInfo == null)
				{
					try
					{
						cultureInfo = new CultureInfo(text);
					}
					catch
					{
					}
				}
				if (cultureInfo == null)
				{
					text = text.ToLower(CultureInfo.CurrentCulture);
					foreach (object obj2 in this.values)
					{
						CultureInfo cultureInfo3 = (CultureInfo)obj2;
						if (cultureInfo3 != null && cultureInfo3.DisplayName.ToLower(CultureInfo.CurrentCulture).StartsWith(text))
						{
							cultureInfo = cultureInfo3;
							break;
						}
					}
				}
			}
			finally
			{
				Thread.CurrentThread.CurrentUICulture = currentUICulture;
			}
			if (cultureInfo == null)
			{
				throw new ArgumentException(SR.GetString("CultureInfoConverterInvalidCulture", new object[]
				{
					(string)value
				}));
			}
			return cultureInfo;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00019520 File Offset: 0x00018520
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(string))
			{
				CultureInfo currentUICulture = Thread.CurrentThread.CurrentUICulture;
				if (culture != null && culture.Equals(CultureInfo.InvariantCulture))
				{
					Thread.CurrentThread.CurrentUICulture = culture;
				}
				string result;
				try
				{
					if (value == null || value == CultureInfo.InvariantCulture)
					{
						result = this.DefaultCultureString;
					}
					else
					{
						result = ((CultureInfo)value).DisplayName;
					}
				}
				finally
				{
					Thread.CurrentThread.CurrentUICulture = currentUICulture;
				}
				return result;
			}
			if (destinationType == typeof(InstanceDescriptor) && value is CultureInfo)
			{
				CultureInfo cultureInfo = (CultureInfo)value;
				ConstructorInfo constructor = typeof(CultureInfo).GetConstructor(new Type[]
				{
					typeof(string)
				});
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[]
					{
						cultureInfo.Name
					});
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00019620 File Offset: 0x00018620
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.values == null)
			{
				CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures | CultureTypes.SpecificCultures);
				CultureInfo[] array = new CultureInfo[cultures.Length + 1];
				Array.Copy(cultures, array, cultures.Length);
				Array.Sort(array, new CultureInfoConverter.CultureComparer());
				if (array[0] == null)
				{
					array[0] = CultureInfo.InvariantCulture;
				}
				this.values = new TypeConverter.StandardValuesCollection(array);
			}
			return this.values;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001967B File Offset: 0x0001867B
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001967E File Offset: 0x0001867E
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x04000925 RID: 2341
		private TypeConverter.StandardValuesCollection values;

		// Token: 0x020000C1 RID: 193
		private class CultureComparer : IComparer
		{
			// Token: 0x060006A6 RID: 1702 RVA: 0x0001968C File Offset: 0x0001868C
			public int Compare(object item1, object item2)
			{
				if (item1 == null)
				{
					if (item2 == null)
					{
						return 0;
					}
					return -1;
				}
				else
				{
					if (item2 == null)
					{
						return 1;
					}
					string displayName = ((CultureInfo)item1).DisplayName;
					string displayName2 = ((CultureInfo)item2).DisplayName;
					CompareInfo compareInfo = CultureInfo.CurrentCulture.CompareInfo;
					return compareInfo.Compare(displayName, displayName2, CompareOptions.StringSort);
				}
			}
		}
	}
}
