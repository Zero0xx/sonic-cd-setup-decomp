using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200047E RID: 1150
	[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	public sealed class SafeWaitHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002DB1 RID: 11697 RVA: 0x00098FA7 File Offset: 0x00097FA7
		private SafeWaitHandle() : base(true)
		{
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x00098FB0 File Offset: 0x00097FB0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public SafeWaitHandle(IntPtr existingHandle, bool ownsHandle) : base(ownsHandle)
		{
			base.SetHandle(existingHandle);
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x00098FC0 File Offset: 0x00097FC0
		protected override bool ReleaseHandle()
		{
			return Win32Native.CloseHandle(this.handle);
		}
	}
}
