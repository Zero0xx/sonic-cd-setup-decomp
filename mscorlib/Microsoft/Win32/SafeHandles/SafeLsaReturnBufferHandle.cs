using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000685 RID: 1669
	internal sealed class SafeLsaReturnBufferHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003C4F RID: 15439 RVA: 0x000CE3D6 File Offset: 0x000CD3D6
		private SafeLsaReturnBufferHandle() : base(true)
		{
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x000CE3DF File Offset: 0x000CD3DF
		internal SafeLsaReturnBufferHandle(IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x06003C51 RID: 15441 RVA: 0x000CE3EF File Offset: 0x000CD3EF
		internal static SafeLsaReturnBufferHandle InvalidHandle
		{
			get
			{
				return new SafeLsaReturnBufferHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06003C52 RID: 15442 RVA: 0x000CE3FB File Offset: 0x000CD3FB
		protected override bool ReleaseHandle()
		{
			return Win32Native.LsaFreeReturnBuffer(this.handle) >= 0;
		}
	}
}
