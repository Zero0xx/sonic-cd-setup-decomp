using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000515 RID: 1301
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeLocalFree : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002823 RID: 10275 RVA: 0x000A5934 File Offset: 0x000A4934
		private SafeLocalFree() : base(true)
		{
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x000A593D File Offset: 0x000A493D
		private SafeLocalFree(bool ownsHandle) : base(ownsHandle)
		{
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x000A5948 File Offset: 0x000A4948
		public static SafeLocalFree LocalAlloc(int cb)
		{
			SafeLocalFree safeLocalFree = UnsafeNclNativeMethods.SafeNetHandles.LocalAlloc(0, (UIntPtr)((ulong)((long)cb)));
			if (safeLocalFree.IsInvalid)
			{
				safeLocalFree.SetHandleAsInvalid();
				throw new OutOfMemoryException();
			}
			return safeLocalFree;
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x000A5978 File Offset: 0x000A4978
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles.LocalFree(this.handle) == IntPtr.Zero;
		}

		// Token: 0x04002764 RID: 10084
		private const int LMEM_FIXED = 0;

		// Token: 0x04002765 RID: 10085
		private const int NULL = 0;

		// Token: 0x04002766 RID: 10086
		public static SafeLocalFree Zero = new SafeLocalFree(false);
	}
}
