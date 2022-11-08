using System;
using System.Security;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x02000529 RID: 1321
	[SuppressUnmanagedCodeSecurity]
	internal class SafeLocalFreeChannelBinding : ChannelBinding
	{
		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x0600287F RID: 10367 RVA: 0x000A792C File Offset: 0x000A692C
		public override int Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x000A7934 File Offset: 0x000A6934
		public static SafeLocalFreeChannelBinding LocalAlloc(int cb)
		{
			SafeLocalFreeChannelBinding safeLocalFreeChannelBinding = UnsafeNclNativeMethods.SafeNetHandles.LocalAllocChannelBinding(0, (UIntPtr)((ulong)((long)cb)));
			if (safeLocalFreeChannelBinding.IsInvalid)
			{
				safeLocalFreeChannelBinding.SetHandleAsInvalid();
				throw new OutOfMemoryException();
			}
			safeLocalFreeChannelBinding.size = cb;
			return safeLocalFreeChannelBinding;
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x000A796B File Offset: 0x000A696B
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles.LocalFree(this.handle) == IntPtr.Zero;
		}

		// Token: 0x04002784 RID: 10116
		private const int LMEM_FIXED = 0;

		// Token: 0x04002785 RID: 10117
		private int size;
	}
}
