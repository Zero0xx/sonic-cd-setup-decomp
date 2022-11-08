using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000511 RID: 1297
	[ComVisible(true)]
	public interface ICustomMarshaler
	{
		// Token: 0x060031DE RID: 12766
		object MarshalNativeToManaged(IntPtr pNativeData);

		// Token: 0x060031DF RID: 12767
		IntPtr MarshalManagedToNative(object ManagedObj);

		// Token: 0x060031E0 RID: 12768
		void CleanUpNativeData(IntPtr pNativeData);

		// Token: 0x060031E1 RID: 12769
		void CleanUpManagedData(object ManagedObj);

		// Token: 0x060031E2 RID: 12770
		int GetNativeDataSize();
	}
}
