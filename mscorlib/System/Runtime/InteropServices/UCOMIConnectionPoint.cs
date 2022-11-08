using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000539 RID: 1337
	[Guid("B196B286-BAB4-101A-B69C-00AA00341D07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IConnectionPoint instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[ComImport]
	public interface UCOMIConnectionPoint
	{
		// Token: 0x0600333F RID: 13119
		void GetConnectionInterface(out Guid pIID);

		// Token: 0x06003340 RID: 13120
		void GetConnectionPointContainer(out UCOMIConnectionPointContainer ppCPC);

		// Token: 0x06003341 RID: 13121
		void Advise([MarshalAs(UnmanagedType.Interface)] object pUnkSink, out int pdwCookie);

		// Token: 0x06003342 RID: 13122
		void Unadvise(int dwCookie);

		// Token: 0x06003343 RID: 13123
		void EnumConnections(out UCOMIEnumConnections ppEnum);
	}
}
