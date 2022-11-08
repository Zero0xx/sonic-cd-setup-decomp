using System;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200047A RID: 1146
	internal sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002DA5 RID: 11685 RVA: 0x00098EF9 File Offset: 0x00097EF9
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeFindHandle() : base(true)
		{
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x00098F02 File Offset: 0x00097F02
		protected override bool ReleaseHandle()
		{
			return Win32Native.FindClose(this.handle);
		}
	}
}
