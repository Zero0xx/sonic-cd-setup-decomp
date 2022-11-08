using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	// Token: 0x02000442 RID: 1090
	[Guid("7c23ff90-33af-11d3-95da-00a024a85b51")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IApplicationContext
	{
		// Token: 0x06002C65 RID: 11365
		void SetContextNameObject(IAssemblyName pName);

		// Token: 0x06002C66 RID: 11366
		void GetContextNameObject(out IAssemblyName ppName);

		// Token: 0x06002C67 RID: 11367
		void Set([MarshalAs(UnmanagedType.LPWStr)] string szName, int pvValue, uint cbValue, uint dwFlags);

		// Token: 0x06002C68 RID: 11368
		void Get([MarshalAs(UnmanagedType.LPWStr)] string szName, out int pvValue, ref uint pcbValue, uint dwFlags);

		// Token: 0x06002C69 RID: 11369
		void GetDynamicDirectory(out int wzDynamicDir, ref uint pdwSize);
	}
}
