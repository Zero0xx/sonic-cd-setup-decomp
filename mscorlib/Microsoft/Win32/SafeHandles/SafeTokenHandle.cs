using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000688 RID: 1672
	internal sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003C5A RID: 15450 RVA: 0x000CE466 File Offset: 0x000CD466
		private SafeTokenHandle() : base(true)
		{
		}

		// Token: 0x06003C5B RID: 15451 RVA: 0x000CE46F File Offset: 0x000CD46F
		internal SafeTokenHandle(IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x06003C5C RID: 15452 RVA: 0x000CE47F File Offset: 0x000CD47F
		internal static SafeTokenHandle InvalidHandle
		{
			get
			{
				return new SafeTokenHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06003C5D RID: 15453 RVA: 0x000CE48B File Offset: 0x000CD48B
		protected override bool ReleaseHandle()
		{
			return Win32Native.CloseHandle(this.handle);
		}
	}
}
