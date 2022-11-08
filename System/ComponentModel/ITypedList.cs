using System;

namespace System.ComponentModel
{
	// Token: 0x02000105 RID: 261
	public interface ITypedList
	{
		// Token: 0x06000837 RID: 2103
		string GetListName(PropertyDescriptor[] listAccessors);

		// Token: 0x06000838 RID: 2104
		PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors);
	}
}
