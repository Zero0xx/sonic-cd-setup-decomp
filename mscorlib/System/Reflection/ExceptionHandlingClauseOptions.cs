using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000335 RID: 821
	[ComVisible(true)]
	[Flags]
	public enum ExceptionHandlingClauseOptions
	{
		// Token: 0x04000D8B RID: 3467
		Clause = 0,
		// Token: 0x04000D8C RID: 3468
		Filter = 1,
		// Token: 0x04000D8D RID: 3469
		Finally = 2,
		// Token: 0x04000D8E RID: 3470
		Fault = 4
	}
}
