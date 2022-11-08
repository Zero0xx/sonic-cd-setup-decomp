using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A0 RID: 416
	[StructLayout(LayoutKind.Sequential)]
	internal class MuiResourceTypeIdStringEntry : IDisposable
	{
		// Token: 0x06001453 RID: 5203 RVA: 0x000365A4 File Offset: 0x000355A4
		~MuiResourceTypeIdStringEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x000365D4 File Offset: 0x000355D4
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x000365E0 File Offset: 0x000355E0
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

		// Token: 0x0400073E RID: 1854
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr StringIds;

		// Token: 0x0400073F RID: 1855
		public uint StringIdsSize;

		// Token: 0x04000740 RID: 1856
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr IntegerIds;

		// Token: 0x04000741 RID: 1857
		public uint IntegerIdsSize;
	}
}
