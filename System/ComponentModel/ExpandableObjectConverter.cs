using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000E4 RID: 228
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ExpandableObjectConverter : TypeConverter
	{
		// Token: 0x060007B4 RID: 1972 RVA: 0x0001BB33 File Offset: 0x0001AB33
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(value, attributes);
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0001BB3C File Offset: 0x0001AB3C
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
