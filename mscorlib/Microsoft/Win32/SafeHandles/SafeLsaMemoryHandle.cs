using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000683 RID: 1667
	internal sealed class SafeLsaMemoryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003C47 RID: 15431 RVA: 0x000CE36C File Offset: 0x000CD36C
		private SafeLsaMemoryHandle() : base(true)
		{
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x000CE375 File Offset: 0x000CD375
		internal SafeLsaMemoryHandle(IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x06003C49 RID: 15433 RVA: 0x000CE385 File Offset: 0x000CD385
		internal static SafeLsaMemoryHandle InvalidHandle
		{
			get
			{
				return new SafeLsaMemoryHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x000CE391 File Offset: 0x000CD391
		protected override bool ReleaseHandle()
		{
			return Win32Native.LsaFreeMemory(this.handle) == 0;
		}
	}
}
