using System;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000478 RID: 1144
	[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	public sealed class SafeFileHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002D9F RID: 11679 RVA: 0x00098EAD File Offset: 0x00097EAD
		private SafeFileHandle() : base(true)
		{
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x00098EB6 File Offset: 0x00097EB6
		public SafeFileHandle(IntPtr preexistingHandle, bool ownsHandle) : base(ownsHandle)
		{
			base.SetHandle(preexistingHandle);
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x00098EC6 File Offset: 0x00097EC6
		protected override bool ReleaseHandle()
		{
			return Win32Native.CloseHandle(this.handle);
		}
	}
}
