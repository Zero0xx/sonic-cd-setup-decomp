using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200057C RID: 1404
	[Guid("0000000c-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IStream
	{
		// Token: 0x06003415 RID: 13333
		void Read([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [Out] byte[] pv, int cb, IntPtr pcbRead);

		// Token: 0x06003416 RID: 13334
		void Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, IntPtr pcbWritten);

		// Token: 0x06003417 RID: 13335
		void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);

		// Token: 0x06003418 RID: 13336
		void SetSize(long libNewSize);

		// Token: 0x06003419 RID: 13337
		void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);

		// Token: 0x0600341A RID: 13338
		void Commit(int grfCommitFlags);

		// Token: 0x0600341B RID: 13339
		void Revert();

		// Token: 0x0600341C RID: 13340
		void LockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x0600341D RID: 13341
		void UnlockRegion(long libOffset, long cb, int dwLockType);

		// Token: 0x0600341E RID: 13342
		void Stat(out STATSTG pstatstg, int grfStatFlag);

		// Token: 0x0600341F RID: 13343
		void Clone(out IStream ppstm);
	}
}
