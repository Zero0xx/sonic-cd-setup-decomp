using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A9 RID: 425
	[StructLayout(LayoutKind.Sequential)]
	internal class HashElementEntry : IDisposable
	{
		// Token: 0x06001468 RID: 5224 RVA: 0x000367A8 File Offset: 0x000357A8
		~HashElementEntry()
		{
			this.Dispose(false);
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x000367D8 File Offset: 0x000357D8
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x000367E4 File Offset: 0x000357E4
		public void Dispose(bool fDisposing)
		{
			if (this.TransformMetadata != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.TransformMetadata);
				this.TransformMetadata = IntPtr.Zero;
			}
			if (this.DigestValue != IntPtr.Zero)
			{
				Marshal.FreeCoTaskMem(this.DigestValue);
				this.DigestValue = IntPtr.Zero;
			}
			if (fDisposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x04000759 RID: 1881
		public uint index;

		// Token: 0x0400075A RID: 1882
		public byte Transform;

		// Token: 0x0400075B RID: 1883
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr TransformMetadata;

		// Token: 0x0400075C RID: 1884
		public uint TransformMetadataSize;

		// Token: 0x0400075D RID: 1885
		public byte DigestMethod;

		// Token: 0x0400075E RID: 1886
		[MarshalAs(UnmanagedType.SysInt)]
		public IntPtr DigestValue;

		// Token: 0x0400075F RID: 1887
		public uint DigestValueSize;

		// Token: 0x04000760 RID: 1888
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Xml;
	}
}
