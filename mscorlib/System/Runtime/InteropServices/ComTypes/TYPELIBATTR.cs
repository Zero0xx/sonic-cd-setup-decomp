using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000599 RID: 1433
	[Serializable]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPELIBATTR
	{
		// Token: 0x04001BDA RID: 7130
		public Guid guid;

		// Token: 0x04001BDB RID: 7131
		public int lcid;

		// Token: 0x04001BDC RID: 7132
		public SYSKIND syskind;

		// Token: 0x04001BDD RID: 7133
		public short wMajorVerNum;

		// Token: 0x04001BDE RID: 7134
		public short wMinorVerNum;

		// Token: 0x04001BDF RID: 7135
		public LIBFLAGS wLibFlags;
	}
}
