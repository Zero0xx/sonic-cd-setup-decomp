using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000583 RID: 1411
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPEATTR
	{
		// Token: 0x04001B4C RID: 6988
		public const int MEMBER_ID_NIL = -1;

		// Token: 0x04001B4D RID: 6989
		public Guid guid;

		// Token: 0x04001B4E RID: 6990
		public int lcid;

		// Token: 0x04001B4F RID: 6991
		public int dwReserved;

		// Token: 0x04001B50 RID: 6992
		public int memidConstructor;

		// Token: 0x04001B51 RID: 6993
		public int memidDestructor;

		// Token: 0x04001B52 RID: 6994
		public IntPtr lpstrSchema;

		// Token: 0x04001B53 RID: 6995
		public int cbSizeInstance;

		// Token: 0x04001B54 RID: 6996
		public TYPEKIND typekind;

		// Token: 0x04001B55 RID: 6997
		public short cFuncs;

		// Token: 0x04001B56 RID: 6998
		public short cVars;

		// Token: 0x04001B57 RID: 6999
		public short cImplTypes;

		// Token: 0x04001B58 RID: 7000
		public short cbSizeVft;

		// Token: 0x04001B59 RID: 7001
		public short cbAlignment;

		// Token: 0x04001B5A RID: 7002
		public TYPEFLAGS wTypeFlags;

		// Token: 0x04001B5B RID: 7003
		public short wMajorVerNum;

		// Token: 0x04001B5C RID: 7004
		public short wMinorVerNum;

		// Token: 0x04001B5D RID: 7005
		public TYPEDESC tdescAlias;

		// Token: 0x04001B5E RID: 7006
		public IDLDESC idldescType;
	}
}
