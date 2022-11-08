using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Net.NetworkInformation
{
	// Token: 0x0200060E RID: 1550
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeWinINetNativeMethods
	{
		// Token: 0x06002FE5 RID: 12261
		[DllImport("wininet.dll")]
		internal static extern bool InternetGetConnectedState(ref uint flags, uint dwReserved);

		// Token: 0x04002DB8 RID: 11704
		private const string WININET = "wininet.dll";
	}
}
