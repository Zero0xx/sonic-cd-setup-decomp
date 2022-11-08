using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000682 RID: 1666
	internal sealed class SafeLsaLogonProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003C43 RID: 15427 RVA: 0x000CE334 File Offset: 0x000CD334
		private SafeLsaLogonProcessHandle() : base(true)
		{
		}

		// Token: 0x06003C44 RID: 15428 RVA: 0x000CE33D File Offset: 0x000CD33D
		internal SafeLsaLogonProcessHandle(IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x06003C45 RID: 15429 RVA: 0x000CE34D File Offset: 0x000CD34D
		internal static SafeLsaLogonProcessHandle InvalidHandle
		{
			get
			{
				return new SafeLsaLogonProcessHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06003C46 RID: 15430 RVA: 0x000CE359 File Offset: 0x000CD359
		protected override bool ReleaseHandle()
		{
			return Win32Native.LsaDeregisterLogonProcess(this.handle) >= 0;
		}
	}
}
