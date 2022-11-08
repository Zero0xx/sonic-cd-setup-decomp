using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x020004B3 RID: 1203
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum PolicyStatementAttribute
	{
		// Token: 0x04001851 RID: 6225
		Nothing = 0,
		// Token: 0x04001852 RID: 6226
		Exclusive = 1,
		// Token: 0x04001853 RID: 6227
		LevelFinal = 2,
		// Token: 0x04001854 RID: 6228
		All = 3
	}
}
