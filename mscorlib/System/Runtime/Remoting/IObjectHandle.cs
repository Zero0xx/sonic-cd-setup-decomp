using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting
{
	// Token: 0x02000156 RID: 342
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[Guid("C460E2B4-E199-412a-8456-84DC3E4838C3")]
	public interface IObjectHandle
	{
		// Token: 0x06001273 RID: 4723
		object Unwrap();
	}
}
