using System;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000E7 RID: 231
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class GuidConverter : TypeConverter
	{
		// Token: 0x060007C9 RID: 1993 RVA: 0x0001BDD9 File Offset: 0x0001ADD9
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0001BDF2 File Offset: 0x0001ADF2
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0001BE0C File Offset: 0x0001AE0C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string g = ((string)value).Trim();
				return new Guid(g);
			}
			return base.ConvertFrom(context, culture, value);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0001BE44 File Offset: 0x0001AE44
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == typeof(InstanceDescriptor) && value is Guid)
			{
				ConstructorInfo constructor = typeof(Guid).GetConstructor(new Type[]
				{
					typeof(string)
				});
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[]
					{
						value.ToString()
					});
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
