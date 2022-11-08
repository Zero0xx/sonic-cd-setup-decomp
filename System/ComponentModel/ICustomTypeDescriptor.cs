using System;

namespace System.ComponentModel
{
	// Token: 0x020000C2 RID: 194
	public interface ICustomTypeDescriptor
	{
		// Token: 0x060006A8 RID: 1704
		AttributeCollection GetAttributes();

		// Token: 0x060006A9 RID: 1705
		string GetClassName();

		// Token: 0x060006AA RID: 1706
		string GetComponentName();

		// Token: 0x060006AB RID: 1707
		TypeConverter GetConverter();

		// Token: 0x060006AC RID: 1708
		EventDescriptor GetDefaultEvent();

		// Token: 0x060006AD RID: 1709
		PropertyDescriptor GetDefaultProperty();

		// Token: 0x060006AE RID: 1710
		object GetEditor(Type editorBaseType);

		// Token: 0x060006AF RID: 1711
		EventDescriptorCollection GetEvents();

		// Token: 0x060006B0 RID: 1712
		EventDescriptorCollection GetEvents(Attribute[] attributes);

		// Token: 0x060006B1 RID: 1713
		PropertyDescriptorCollection GetProperties();

		// Token: 0x060006B2 RID: 1714
		PropertyDescriptorCollection GetProperties(Attribute[] attributes);

		// Token: 0x060006B3 RID: 1715
		object GetPropertyOwner(PropertyDescriptor pd);
	}
}
