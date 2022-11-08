using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000B7 RID: 183
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ReferenceConverter : TypeConverter
	{
		// Token: 0x0600066A RID: 1642 RVA: 0x000187EA File Offset: 0x000177EA
		public ReferenceConverter(Type type)
		{
			this.type = type;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x000187F9 File Offset: 0x000177F9
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return (sourceType == typeof(string) && context != null) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00018818 File Offset: 0x00017818
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = ((string)value).Trim();
				if (!string.Equals(text, ReferenceConverter.none) && context != null)
				{
					IReferenceService referenceService = (IReferenceService)context.GetService(typeof(IReferenceService));
					if (referenceService != null)
					{
						object reference = referenceService.GetReference(text);
						if (reference != null)
						{
							return reference;
						}
					}
					IContainer container = context.Container;
					if (container != null)
					{
						object obj = container.Components[text];
						if (obj != null)
						{
							return obj;
						}
					}
				}
				return null;
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0001889C File Offset: 0x0001789C
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType != typeof(string))
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
			if (value != null)
			{
				if (context != null)
				{
					IReferenceService referenceService = (IReferenceService)context.GetService(typeof(IReferenceService));
					if (referenceService != null)
					{
						string name = referenceService.GetName(value);
						if (name != null)
						{
							return name;
						}
					}
				}
				if (!Marshal.IsComObject(value) && value is IComponent)
				{
					IComponent component = (IComponent)value;
					ISite site = component.Site;
					if (site != null)
					{
						string name2 = site.Name;
						if (name2 != null)
						{
							return name2;
						}
					}
				}
				return string.Empty;
			}
			return ReferenceConverter.none;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0001893C File Offset: 0x0001793C
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			object[] array = null;
			if (context != null)
			{
				ArrayList arrayList = new ArrayList();
				arrayList.Add(null);
				IReferenceService referenceService = (IReferenceService)context.GetService(typeof(IReferenceService));
				if (referenceService != null)
				{
					object[] references = referenceService.GetReferences(this.type);
					int num = references.Length;
					for (int i = 0; i < num; i++)
					{
						if (this.IsValueAllowed(context, references[i]))
						{
							arrayList.Add(references[i]);
						}
					}
				}
				else
				{
					IContainer container = context.Container;
					if (container != null)
					{
						ComponentCollection components = container.Components;
						foreach (object obj in components)
						{
							IComponent component = (IComponent)obj;
							if (component != null && this.type.IsInstanceOfType(component) && this.IsValueAllowed(context, component))
							{
								arrayList.Add(component);
							}
						}
					}
				}
				array = arrayList.ToArray();
				Array.Sort(array, 0, array.Length, new ReferenceConverter.ReferenceComparer(this));
			}
			return new TypeConverter.StandardValuesCollection(array);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00018A58 File Offset: 0x00017A58
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00018A5B File Offset: 0x00017A5B
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00018A5E File Offset: 0x00017A5E
		protected virtual bool IsValueAllowed(ITypeDescriptorContext context, object value)
		{
			return true;
		}

		// Token: 0x04000917 RID: 2327
		private static readonly string none = SR.GetString("toStringNone");

		// Token: 0x04000918 RID: 2328
		private Type type;

		// Token: 0x020000B8 RID: 184
		private class ReferenceComparer : IComparer
		{
			// Token: 0x06000673 RID: 1651 RVA: 0x00018A72 File Offset: 0x00017A72
			public ReferenceComparer(ReferenceConverter converter)
			{
				this.converter = converter;
			}

			// Token: 0x06000674 RID: 1652 RVA: 0x00018A84 File Offset: 0x00017A84
			public int Compare(object item1, object item2)
			{
				string strA = this.converter.ConvertToString(item1);
				string strB = this.converter.ConvertToString(item2);
				return string.Compare(strA, strB, false, CultureInfo.InvariantCulture);
			}

			// Token: 0x04000919 RID: 2329
			private ReferenceConverter converter;
		}
	}
}
