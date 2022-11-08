using System;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000684 RID: 1668
	internal sealed class SafeLsaPolicyHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003C4B RID: 15435 RVA: 0x000CE3A1 File Offset: 0x000CD3A1
		private SafeLsaPolicyHandle() : base(true)
		{
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x000CE3AA File Offset: 0x000CD3AA
		internal SafeLsaPolicyHandle(IntPtr handle) : base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x06003C4D RID: 15437 RVA: 0x000CE3BA File Offset: 0x000CD3BA
		internal static SafeLsaPolicyHandle InvalidHandle
		{
			get
			{
				return new SafeLsaPolicyHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x000CE3C6 File Offset: 0x000CD3C6
		protected override bool ReleaseHandle()
		{
			return Win32Native.LsaClose(this.handle) == 0;
		}
	}
}
