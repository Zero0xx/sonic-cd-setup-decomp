using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000681 RID: 1665
	internal sealed class SafeLocalAllocHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003C3F RID: 15423 RVA: 0x000CE2F8 File Offset: 0x000CD2F8
		private SafeLocalAllocHandle() : base(true)
		{
		}

		// Token: 0x06003C40 RID: 15424 RVA: 0x000CE301 File Offset: 0x000CD301
		internal SafeLocalAllocHandle(IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x06003C41 RID: 15425 RVA: 0x000CE311 File Offset: 0x000CD311
		internal static SafeLocalAllocHandle InvalidHandle
		{
			get
			{
				return new SafeLocalAllocHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06003C42 RID: 15426 RVA: 0x000CE31D File Offset: 0x000CD31D
		protected override bool ReleaseHandle()
		{
			return Win32Native.LocalFree(this.handle) == IntPtr.Zero;
		}
	}
}
