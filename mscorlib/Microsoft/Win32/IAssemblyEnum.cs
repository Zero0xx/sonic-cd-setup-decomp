using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	// Token: 0x02000441 RID: 1089
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("21b8916c-f28e-11d2-a473-00c04f8ef448")]
	[ComImport]
	internal interface IAssemblyEnum
	{
		// Token: 0x06002C62 RID: 11362
		[PreserveSig]
		int GetNextAssembly(out IApplicationContext ppAppCtx, out IAssemblyName ppName, uint dwFlags);

		// Token: 0x06002C63 RID: 11363
		[PreserveSig]
		int Reset();

		// Token: 0x06002C64 RID: 11364
		[PreserveSig]
		int Clone(out IAssemblyEnum ppEnum);
	}
}
