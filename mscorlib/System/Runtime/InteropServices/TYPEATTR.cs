using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000550 RID: 1360
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.TYPEATTR instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPEATTR
	{
		// Token: 0x04001A82 RID: 6786
		public const int MEMBER_ID_NIL = -1;

		// Token: 0x04001A83 RID: 6787
		public Guid guid;

		// Token: 0x04001A84 RID: 6788
		public int lcid;

		// Token: 0x04001A85 RID: 6789
		public int dwReserved;

		// Token: 0x04001A86 RID: 6790
		public int memidConstructor;

		// Token: 0x04001A87 RID: 6791
		public int memidDestructor;

		// Token: 0x04001A88 RID: 6792
		public IntPtr lpstrSchema;

		// Token: 0x04001A89 RID: 6793
		public int cbSizeInstance;

		// Token: 0x04001A8A RID: 6794
		public TYPEKIND typekind;

		// Token: 0x04001A8B RID: 6795
		public short cFuncs;

		// Token: 0x04001A8C RID: 6796
		public short cVars;

		// Token: 0x04001A8D RID: 6797
		public short cImplTypes;

		// Token: 0x04001A8E RID: 6798
		public short cbSizeVft;

		// Token: 0x04001A8F RID: 6799
		public short cbAlignment;

		// Token: 0x04001A90 RID: 6800
		public TYPEFLAGS wTypeFlags;

		// Token: 0x04001A91 RID: 6801
		public short wMajorVerNum;

		// Token: 0x04001A92 RID: 6802
		public short wMinorVerNum;

		// Token: 0x04001A93 RID: 6803
		public TYPEDESC tdescAlias;

		// Token: 0x04001A94 RID: 6804
		public IDLDESC idldescType;
	}
}
