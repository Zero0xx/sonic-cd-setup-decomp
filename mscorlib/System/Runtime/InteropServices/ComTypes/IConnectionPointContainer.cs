using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200056B RID: 1387
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B196B284-BAB4-101A-B69C-00AA00341D07")]
	[ComImport]
	public interface IConnectionPointContainer
	{
		// Token: 0x060033C5 RID: 13253
		void EnumConnectionPoints(out IEnumConnectionPoints ppEnum);

		// Token: 0x060033C6 RID: 13254
		void FindConnectionPoint([In] ref Guid riid, out IConnectionPoint ppCP);
	}
}
