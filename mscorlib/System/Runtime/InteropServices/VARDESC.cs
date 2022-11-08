using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000559 RID: 1369
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.VARDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct VARDESC
	{
		// Token: 0x04001ABA RID: 6842
		public int memid;

		// Token: 0x04001ABB RID: 6843
		public string lpstrSchema;

		// Token: 0x04001ABC RID: 6844
		public ELEMDESC elemdescVar;

		// Token: 0x04001ABD RID: 6845
		public short wVarFlags;

		// Token: 0x04001ABE RID: 6846
		public VarEnum varkind;

		// Token: 0x0200055A RID: 1370
		[ComVisible(false)]
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct DESCUNION
		{
			// Token: 0x04001ABF RID: 6847
			[FieldOffset(0)]
			public int oInst;

			// Token: 0x04001AC0 RID: 6848
			[FieldOffset(0)]
			public IntPtr lpvarValue;
		}
	}
}
