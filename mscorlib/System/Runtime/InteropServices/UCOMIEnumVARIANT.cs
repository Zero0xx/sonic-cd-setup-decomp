using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000541 RID: 1345
	[Guid("00020404-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IEnumVARIANT instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[ComImport]
	public interface UCOMIEnumVARIANT
	{
		// Token: 0x06003358 RID: 13144
		[PreserveSig]
		int Next(int celt, int rgvar, int pceltFetched);

		// Token: 0x06003359 RID: 13145
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x0600335A RID: 13146
		[PreserveSig]
		int Reset();

		// Token: 0x0600335B RID: 13147
		void Clone(int ppenum);
	}
}
