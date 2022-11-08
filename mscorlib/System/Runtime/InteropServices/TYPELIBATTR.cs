using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000565 RID: 1381
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.TYPELIBATTR instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPELIBATTR
	{
		// Token: 0x04001B08 RID: 6920
		public Guid guid;

		// Token: 0x04001B09 RID: 6921
		public int lcid;

		// Token: 0x04001B0A RID: 6922
		public SYSKIND syskind;

		// Token: 0x04001B0B RID: 6923
		public short wMajorVerNum;

		// Token: 0x04001B0C RID: 6924
		public short wMinorVerNum;

		// Token: 0x04001B0D RID: 6925
		public LIBFLAGS wLibFlags;
	}
}
