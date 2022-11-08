using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001E6 RID: 486
	internal struct BLOB : IDisposable
	{
		// Token: 0x06001513 RID: 5395 RVA: 0x00036DE8 File Offset: 0x00035DE8
		public void Dispose()
		{
			if (this.BlobData != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.BlobData);
				this.BlobData = IntPtr.Zero;
			}
		}

		// Token: 0x04000844 RID: 2116
		[MarshalAs(UnmanagedType.U4)]
		public uint Size;

		// Token: 0x04000845 RID: 2117
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr BlobData;
	}
}
