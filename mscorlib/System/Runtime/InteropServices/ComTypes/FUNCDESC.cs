using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000584 RID: 1412
	public struct FUNCDESC
	{
		// Token: 0x04001B5F RID: 7007
		public int memid;

		// Token: 0x04001B60 RID: 7008
		public IntPtr lprgscode;

		// Token: 0x04001B61 RID: 7009
		public IntPtr lprgelemdescParam;

		// Token: 0x04001B62 RID: 7010
		public FUNCKIND funckind;

		// Token: 0x04001B63 RID: 7011
		public INVOKEKIND invkind;

		// Token: 0x04001B64 RID: 7012
		public CALLCONV callconv;

		// Token: 0x04001B65 RID: 7013
		public short cParams;

		// Token: 0x04001B66 RID: 7014
		public short cParamsOpt;

		// Token: 0x04001B67 RID: 7015
		public short oVft;

		// Token: 0x04001B68 RID: 7016
		public short cScodes;

		// Token: 0x04001B69 RID: 7017
		public ELEMDESC elemdescFunc;

		// Token: 0x04001B6A RID: 7018
		public short wFuncFlags;
	}
}
