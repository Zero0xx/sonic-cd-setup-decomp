using System;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x0200050F RID: 1295
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeInternetHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002813 RID: 10259 RVA: 0x000A5574 File Offset: 0x000A4574
		public SafeInternetHandle() : base(true)
		{
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x000A557D File Offset: 0x000A457D
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.WinHttp.WinHttpCloseHandle(this.handle);
		}
	}
}
