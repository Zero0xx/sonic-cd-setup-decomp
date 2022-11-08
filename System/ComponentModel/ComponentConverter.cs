using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000B9 RID: 185
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ComponentConverter : ReferenceConverter
	{
		// Token: 0x06000675 RID: 1653 RVA: 0x00018AB8 File Offset: 0x00017AB8
		public ComponentConverter(Type type) : base(type)
		{
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00018AC1 File Offset: 0x00017AC1
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(value, attributes);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00018ACA File Offset: 0x00017ACA
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
