using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Windows.Forms.Design;

namespace System.Windows.Forms.PropertyGridInternal
{
	// Token: 0x020007C5 RID: 1989
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class PropertiesTab : PropertyTab
	{
		// Token: 0x17001666 RID: 5734
		// (get) Token: 0x06006910 RID: 26896 RVA: 0x0018254F File Offset: 0x0018154F
		public override string TabName
		{
			get
			{
				return SR.GetString("PBRSToolTipProperties");
			}
		}

		// Token: 0x17001667 RID: 5735
		// (get) Token: 0x06006911 RID: 26897 RVA: 0x0018255B File Offset: 0x0018155B
		public override string HelpKeyword
		{
			get
			{
				return "vs.properties";
			}
		}

		// Token: 0x06006912 RID: 26898 RVA: 0x00182564 File Offset: 0x00181564
		public override PropertyDescriptor GetDefaultProperty(object obj)
		{
			PropertyDescriptor propertyDescriptor = base.GetDefaultProperty(obj);
			if (propertyDescriptor == null)
			{
				PropertyDescriptorCollection properties = this.GetProperties(obj);
				if (properties != null)
				{
					for (int i = 0; i < properties.Count; i++)
					{
						if ("Name".Equals(properties[i].Name))
						{
							propertyDescriptor = properties[i];
							break;
						}
					}
				}
			}
			return propertyDescriptor;
		}

		// Token: 0x06006913 RID: 26899 RVA: 0x001825BB File Offset: 0x001815BB
		public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
		{
			return this.GetProperties(null, component, attributes);
		}

		// Token: 0x06006914 RID: 26900 RVA: 0x001825C8 File Offset: 0x001815C8
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object component, Attribute[] attributes)
		{
			if (attributes == null)
			{
				attributes = new Attribute[]
				{
					BrowsableAttribute.Yes
				};
			}
			if (context == null)
			{
				return TypeDescriptor.GetProperties(component, attributes);
			}
			TypeConverter typeConverter = (context.PropertyDescriptor == null) ? TypeDescriptor.GetConverter(component) : context.PropertyDescriptor.Converter;
			if (typeConverter == null || !typeConverter.GetPropertiesSupported(context))
			{
				return TypeDescriptor.GetProperties(component, attributes);
			}
			return typeConverter.GetProperties(context, component, attributes);
		}
	}
}
