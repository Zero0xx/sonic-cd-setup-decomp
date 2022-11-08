using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000549 RID: 1353
	[Guid("0000000c-0000-0000-C000-000000000046")]
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IStream instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface UCOMIStream
	{
		// Token: 0x0600338D RID: 13197
		void Read([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] pv, int cb, IntPtr pcbRead);

		// Token: 0x0600338E RID: 13198
		void Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, IntPtr pcbWritten);

		// Token: 0x0600338F RID: 13199
		void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);

		// Token: 0x06003390 RID: 13200
		void SetSize(long libNewSize);

		// Token: 0x06003391 RID: 13201
		void CopyTo(UCOMIStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);

		// Token: 0x06003392 RID: 13202
		void Commit(int grfCommitFlags);

		// Token: 0x06003393 RID: 13203
		void Revert();

		// Token: 0x06003394 RID: 13204
		void LockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x06003395 RID: 13205
		void UnlockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x06003396 RID: 13206
		void Stat(out STATSTG pstatstg, int grfStatFlag);

		// Token: 0x06003397 RID: 13207
		void Clone(out UCOMIStream ppstm);
	}
}
