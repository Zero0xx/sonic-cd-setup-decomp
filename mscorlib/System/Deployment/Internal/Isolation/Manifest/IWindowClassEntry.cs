using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001CC RID: 460
	[Guid("8AD3FC86-AFD3-477a-8FD5-146C291195BA")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IWindowClassEntry
	{
		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x060014C2 RID: 5314
		WindowClassEntry AllData { get; }

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060014C3 RID: 5315
		string ClassName { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060014C4 RID: 5316
		string HostDll { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x060014C5 RID: 5317
		bool fVersioned { get; }
	}
}
