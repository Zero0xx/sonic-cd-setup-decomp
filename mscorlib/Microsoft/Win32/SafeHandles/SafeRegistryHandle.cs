using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200047C RID: 1148
	internal sealed class SafeRegistryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002DAA RID: 11690 RVA: 0x00098F39 File Offset: 0x00097F39
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeRegistryHandle() : base(true)
		{
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x00098F42 File Offset: 0x00097F42
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeRegistryHandle(IntPtr preexistingHandle, bool ownsHandle) : base(ownsHandle)
		{
			base.SetHandle(preexistingHandle);
		}

		// Token: 0x06002DAC RID: 11692
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("advapi32.dll")]
		private static extern int RegCloseKey(IntPtr hKey);

		// Token: 0x06002DAD RID: 11693 RVA: 0x00098F54 File Offset: 0x00097F54
		protected override bool ReleaseHandle()
		{
			int num = SafeRegistryHandle.RegCloseKey(this.handle);
			return num == 0;
		}
	}
}
