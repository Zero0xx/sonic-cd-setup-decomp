using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A3 RID: 419
	[StructLayout(LayoutKind.Sequential)]
	internal class MuiResourceTypeIdIntEntry : IDisposable
	{
		// Token: 0x0600145A RID: 5210 RVA: 0x00036650 File Offset: 0x00035650
		~MuiResourceTypeIdIntEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x00036680 File Offset: 0x00035680
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0003668C File Offset: 0x0003568C
		public void Dispose(bool fDisposing)
		{
			if (this.StringIds != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.StringIds);
				this.StringIds = IntPtr.Zero;
			}
			if (this.IntegerIds != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.IntegerIds);
				this.IntegerIds = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x04000747 RID: 1863
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr StringIds;

		// Token: 0x04000748 RID: 1864
		public uint StringIdsSize;

		// Token: 0x04000749 RID: 1865
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr IntegerIds;

		// Token: 0x0400074A RID: 1866
		public uint IntegerIdsSize;
	}
}
