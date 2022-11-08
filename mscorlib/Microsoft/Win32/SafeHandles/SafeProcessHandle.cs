using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000686 RID: 1670
	internal sealed class SafeProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003C53 RID: 15443 RVA: 0x000CE40E File Offset: 0x000CD40E
		private SafeProcessHandle() : base(true)
		{
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x000CE417 File Offset: 0x000CD417
		internal SafeProcessHandle(IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x06003C55 RID: 15445 RVA: 0x000CE427 File Offset: 0x000CD427
		internal static SafeProcessHandle InvalidHandle
		{
			get
			{
				return new SafeProcessHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x000CE433 File Offset: 0x000CD433
		protected override bool ReleaseHandle()
		{
			return Win32Native.CloseHandle(this.handle);
		}
	}
}
