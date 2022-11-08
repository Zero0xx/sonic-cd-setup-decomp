using System;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x0200050E RID: 1294
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeCloseIcmpHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002811 RID: 10257 RVA: 0x000A553F File Offset: 0x000A453F
		private SafeCloseIcmpHandle() : base(true)
		{
			this.IsPostWin2K = ComNetOS.IsPostWin2K;
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x000A5553 File Offset: 0x000A4553
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			if (this.IsPostWin2K)
			{
				return UnsafeNetInfoNativeMethods.IcmpCloseHandle(this.handle);
			}
			return UnsafeIcmpNativeMethods.IcmpCloseHandle(this.handle);
		}

		// Token: 0x0400275C RID: 10076
		private bool IsPostWin2K;
	}
}
