using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000548 RID: 1352
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.STATSTG instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct STATSTG
	{
		// Token: 0x04001A4E RID: 6734
		public string pwcsName;

		// Token: 0x04001A4F RID: 6735
		public int type;

		// Token: 0x04001A50 RID: 6736
		public long cbSize;

		// Token: 0x04001A51 RID: 6737
		public FILETIME mtime;

		// Token: 0x04001A52 RID: 6738
		public FILETIME ctime;

		// Token: 0x04001A53 RID: 6739
		public FILETIME atime;

		// Token: 0x04001A54 RID: 6740
		public int grfMode;

		// Token: 0x04001A55 RID: 6741
		public int grfLocksSupported;

		// Token: 0x04001A56 RID: 6742
		public Guid clsid;

		// Token: 0x04001A57 RID: 6743
		public int grfStateBits;

		// Token: 0x04001A58 RID: 6744
		public int reserved;
	}
}
