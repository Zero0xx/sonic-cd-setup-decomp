using System;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200047D RID: 1149
	internal sealed class SafeViewOfFileHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002DAE RID: 11694 RVA: 0x00098F71 File Offset: 0x00097F71
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeViewOfFileHandle() : base(true)
		{
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x00098F7A File Offset: 0x00097F7A
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeViewOfFileHandle(IntPtr handle, bool ownsHandle) : base(ownsHandle)
		{
			base.SetHandle(handle);
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x00098F8A File Offset: 0x00097F8A
		protected override bool ReleaseHandle()
		{
			if (Win32Native.UnmapViewOfFile(this.handle))
			{
				this.handle = IntPtr.Zero;
				return true;
			}
			return false;
		}
	}
}
