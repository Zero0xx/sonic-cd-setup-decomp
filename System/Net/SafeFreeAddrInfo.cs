using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x0200050C RID: 1292
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeFreeAddrInfo : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002808 RID: 10248 RVA: 0x000A5434 File Offset: 0x000A4434
		private SafeFreeAddrInfo() : base(true)
		{
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x000A543D File Offset: 0x000A443D
		internal static int GetAddrInfo(string nodename, string servicename, ref AddressInfo hints, out SafeFreeAddrInfo outAddrInfo)
		{
			return UnsafeNclNativeMethods.SafeNetHandlesXPOrLater.getaddrinfo(nodename, servicename, ref hints, out outAddrInfo);
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x000A5448 File Offset: 0x000A4448
		protected override bool ReleaseHandle()
		{
			UnsafeNclNativeMethods.SafeNetHandlesXPOrLater.freeaddrinfo(this.handle);
			return true;
		}

		// Token: 0x04002757 RID: 10071
		private const string WS2_32 = "ws2_32.dll";
	}
}
