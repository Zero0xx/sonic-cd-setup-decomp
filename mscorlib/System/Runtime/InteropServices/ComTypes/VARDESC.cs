using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200058D RID: 1421
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct VARDESC
	{
		// Token: 0x04001B89 RID: 7049
		public int memid;

		// Token: 0x04001B8A RID: 7050
		public string lpstrSchema;

		// Token: 0x04001B8B RID: 7051
		public VARDESC.DESCUNION desc;

		// Token: 0x04001B8C RID: 7052
		public ELEMDESC elemdescVar;

		// Token: 0x04001B8D RID: 7053
		public short wVarFlags;

		// Token: 0x04001B8E RID: 7054
		public VARKIND varkind;

		// Token: 0x0200058E RID: 1422
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct DESCUNION
		{
			// Token: 0x04001B8F RID: 7055
			[FieldOffset(0)]
			public int oInst;

			// Token: 0x04001B90 RID: 7056
			[FieldOffset(0)]
			public IntPtr lpvarValue;
		}
	}
}
