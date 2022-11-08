using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200056C RID: 1388
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B196B286-BAB4-101A-B69C-00AA00341D07")]
	[ComImport]
	public interface IConnectionPoint
	{
		// Token: 0x060033C7 RID: 13255
		void GetConnectionInterface(out Guid pIID);

		// Token: 0x060033C8 RID: 13256
		void GetConnectionPointContainer(out IConnectionPointContainer ppCPC);

		// Token: 0x060033C9 RID: 13257
		void Advise([MarshalAs(UnmanagedType.Interface)] object pUnkSink, out int pdwCookie);

		// Token: 0x060033CA RID: 13258
		void Unadvise(int dwCookie);

		// Token: 0x060033CB RID: 13259
		void EnumConnections(out IEnumConnections ppEnum);
	}
}
