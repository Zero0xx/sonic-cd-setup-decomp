using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A6 RID: 422
	[StructLayout(LayoutKind.Sequential)]
	internal class MuiResourceMapEntry : IDisposable
	{
		// Token: 0x06001461 RID: 5217 RVA: 0x000366FC File Offset: 0x000356FC
		~MuiResourceMapEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0003672C File Offset: 0x0003572C
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x00036738 File Offset: 0x00035738
		public void Dispose(bool fDisposing)
		{
			if (this.ResourceTypeIdInt != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.ResourceTypeIdInt);
				this.ResourceTypeIdInt = IntPtr.Zero;
			}
			if (this.ResourceTypeIdString != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.ResourceTypeIdString);
				this.ResourceTypeIdString = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x04000750 RID: 1872
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr ResourceTypeIdInt;

		// Token: 0x04000751 RID: 1873
		public uint ResourceTypeIdIntSize;

		// Token: 0x04000752 RID: 1874
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr ResourceTypeIdString;

		// Token: 0x04000753 RID: 1875
		public uint ResourceTypeIdStringSize;
	}
}
