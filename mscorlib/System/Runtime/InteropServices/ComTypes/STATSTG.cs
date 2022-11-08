using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200057B RID: 1403
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct STATSTG
	{
		// Token: 0x04001B18 RID: 6936
		public string pwcsName;

		// Token: 0x04001B19 RID: 6937
		public int type;

		// Token: 0x04001B1A RID: 6938
		public long cbSize;

		// Token: 0x04001B1B RID: 6939
		public FILETIME mtime;

		// Token: 0x04001B1C RID: 6940
		public FILETIME ctime;

		// Token: 0x04001B1D RID: 6941
		public FILETIME atime;

		// Token: 0x04001B1E RID: 6942
		public int grfMode;

		// Token: 0x04001B1F RID: 6943
		public int grfLocksSupported;

		// Token: 0x04001B20 RID: 6944
		public Guid clsid;

		// Token: 0x04001B21 RID: 6945
		public int grfStateBits;

		// Token: 0x04001B22 RID: 6946
		public int reserved;
	}
}
