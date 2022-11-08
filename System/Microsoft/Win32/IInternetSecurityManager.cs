using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Microsoft.Win32
{
	// Token: 0x02000378 RID: 888
	[ComVisible(false)]
	[Guid("79eac9ee-baf9-11ce-8c82-00aa004ba90b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IInternetSecurityManager
	{
		// Token: 0x06001BD0 RID: 7120
		unsafe void SetSecuritySite(void* pSite);

		// Token: 0x06001BD1 RID: 7121
		unsafe void GetSecuritySite(void** ppSite);

		// Token: 0x06001BD2 RID: 7122
		[SuppressUnmanagedCodeSecurity]
		void MapUrlToZone([MarshalAs(UnmanagedType.BStr)] [In] string pwszUrl, out int pdwZone, [In] int dwFlags);

		// Token: 0x06001BD3 RID: 7123
		unsafe void GetSecurityId(string pwszUrl, byte* pbSecurityId, int* pcbSecurityId, int dwReserved);

		// Token: 0x06001BD4 RID: 7124
		unsafe void ProcessUrlAction(string pwszUrl, int dwAction, byte* pPolicy, int cbPolicy, byte* pContext, int cbContext, int dwFlags, int dwReserved);

		// Token: 0x06001BD5 RID: 7125
		unsafe void QueryCustomPolicy(string pwszUrl, void* guidKey, byte** ppPolicy, int* pcbPolicy, byte* pContext, int cbContext, int dwReserved);

		// Token: 0x06001BD6 RID: 7126
		void SetZoneMapping(int dwZone, string lpszPattern, int dwFlags);

		// Token: 0x06001BD7 RID: 7127
		unsafe void GetZoneMappings(int dwZone, void** ppenumString, int dwFlags);
	}
}
