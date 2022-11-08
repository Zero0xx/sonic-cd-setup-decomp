using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000522 RID: 1314
	[ComVisible(true)]
	public interface ICustomAdapter
	{
		// Token: 0x060032E1 RID: 13025
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetUnderlyingObject();
	}
}
