using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000687 RID: 1671
	internal sealed class SafeThreadHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003C57 RID: 15447 RVA: 0x000CE440 File Offset: 0x000CD440
		private SafeThreadHandle() : base(true)
		{
		}

		// Token: 0x06003C58 RID: 15448 RVA: 0x000CE449 File Offset: 0x000CD449
		internal SafeThreadHandle(IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x06003C59 RID: 15449 RVA: 0x000CE459 File Offset: 0x000CD459
		protected override bool ReleaseHandle()
		{
			return Win32Native.CloseHandle(this.handle);
		}
	}
}
