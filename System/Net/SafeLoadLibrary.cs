using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000518 RID: 1304
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeLoadLibrary : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002832 RID: 10290 RVA: 0x000A5AAB File Offset: 0x000A4AAB
		private SafeLoadLibrary() : base(true)
		{
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x000A5AB4 File Offset: 0x000A4AB4
		private SafeLoadLibrary(bool ownsHandle) : base(ownsHandle)
		{
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x000A5AC0 File Offset: 0x000A4AC0
		public static SafeLoadLibrary LoadLibraryEx(string library)
		{
			SafeLoadLibrary safeLoadLibrary = ComNetOS.IsWin9x ? UnsafeNclNativeMethods.SafeNetHandles.LoadLibraryExA(library, null, 0U) : UnsafeNclNativeMethods.SafeNetHandles.LoadLibraryExW(library, null, 0U);
			if (safeLoadLibrary.IsInvalid)
			{
				safeLoadLibrary.SetHandleAsInvalid();
			}
			return safeLoadLibrary;
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x000A5AF8 File Offset: 0x000A4AF8
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles.FreeLibrary(this.handle);
		}

		// Token: 0x0400276A RID: 10090
		private const string KERNEL32 = "kernel32.dll";

		// Token: 0x0400276B RID: 10091
		public static readonly SafeLoadLibrary Zero = new SafeLoadLibrary(false);
	}
}
