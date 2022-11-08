using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000551 RID: 1361
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.FUNCDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	public struct FUNCDESC
	{
		// Token: 0x04001A95 RID: 6805
		public int memid;

		// Token: 0x04001A96 RID: 6806
		public IntPtr lprgscode;

		// Token: 0x04001A97 RID: 6807
		public IntPtr lprgelemdescParam;

		// Token: 0x04001A98 RID: 6808
		public FUNCKIND funckind;

		// Token: 0x04001A99 RID: 6809
		public INVOKEKIND invkind;

		// Token: 0x04001A9A RID: 6810
		public CALLCONV callconv;

		// Token: 0x04001A9B RID: 6811
		public short cParams;

		// Token: 0x04001A9C RID: 6812
		public short cParamsOpt;

		// Token: 0x04001A9D RID: 6813
		public short oVft;

		// Token: 0x04001A9E RID: 6814
		public short cScodes;

		// Token: 0x04001A9F RID: 6815
		public ELEMDESC elemdescFunc;

		// Token: 0x04001AA0 RID: 6816
		public short wFuncFlags;
	}
}
