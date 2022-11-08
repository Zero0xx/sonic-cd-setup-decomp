using System;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000479 RID: 1145
	internal sealed class SafeFileMappingHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002DA2 RID: 11682 RVA: 0x00098ED3 File Offset: 0x00097ED3
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeFileMappingHandle() : base(true)
		{
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x00098EDC File Offset: 0x00097EDC
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeFileMappingHandle(IntPtr handle, bool ownsHandle) : base(ownsHandle)
		{
			base.SetHandle(handle);
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x00098EEC File Offset: 0x00097EEC
		protected override bool ReleaseHandle()
		{
			return Win32Native.CloseHandle(this.handle);
		}
	}
}
