using System;

namespace System.ComponentModel
{
	// Token: 0x020000EC RID: 236
	[Obsolete("This interface has been deprecated. Add a TypeDescriptionProvider to handle type TypeDescriptor.ComObjectType instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
	public interface IComNativeDescriptorHandler
	{
		// Token: 0x060007DF RID: 2015
		AttributeCollection GetAttributes(object component);

		// Token: 0x060007E0 RID: 2016
		string GetClassName(object component);

		// Token: 0x060007E1 RID: 2017
		TypeConverter GetConverter(object component);

		// Token: 0x060007E2 RID: 2018
		EventDescriptor GetDefaultEvent(object component);

		// Token: 0x060007E3 RID: 2019
		PropertyDescriptor GetDefaultProperty(object component);

		// Token: 0x060007E4 RID: 2020
		object GetEditor(object component, Type baseEditorType);

		// Token: 0x060007E5 RID: 2021
		string GetName(object component);

		// Token: 0x060007E6 RID: 2022
		EventDescriptorCollection GetEvents(object component);

		// Token: 0x060007E7 RID: 2023
		EventDescriptorCollection GetEvents(object component, Attribute[] attributes);

		// Token: 0x060007E8 RID: 2024
		PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes);

		// Token: 0x060007E9 RID: 2025
		object GetPropertyValue(object component, string propertyName, ref bool success);

		// Token: 0x060007EA RID: 2026
		object GetPropertyValue(object component, int dispid, ref bool success);
	}
}
