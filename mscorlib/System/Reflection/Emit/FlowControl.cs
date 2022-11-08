using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000842 RID: 2114
	[ComVisible(true)]
	[Serializable]
	public enum FlowControl
	{
		// Token: 0x040027C0 RID: 10176
		Branch,
		// Token: 0x040027C1 RID: 10177
		Break,
		// Token: 0x040027C2 RID: 10178
		Call,
		// Token: 0x040027C3 RID: 10179
		Cond_Branch,
		// Token: 0x040027C4 RID: 10180
		Meta,
		// Token: 0x040027C5 RID: 10181
		Next,
		// Token: 0x040027C6 RID: 10182
		[Obsolete("This API has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		Phi,
		// Token: 0x040027C7 RID: 10183
		Return,
		// Token: 0x040027C8 RID: 10184
		Throw
	}
}
