using System;

namespace System.Security.Policy
{
	// Token: 0x020004B1 RID: 1201
	internal sealed class CodeGroupStackFrame
	{
		// Token: 0x0400184C RID: 6220
		internal CodeGroup current;

		// Token: 0x0400184D RID: 6221
		internal PolicyStatement policy;

		// Token: 0x0400184E RID: 6222
		internal CodeGroupStackFrame parent;
	}
}
