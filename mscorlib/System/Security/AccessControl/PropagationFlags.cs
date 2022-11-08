using System;

namespace System.Security.AccessControl
{
	// Token: 0x020008E9 RID: 2281
	[Flags]
	public enum PropagationFlags
	{
		// Token: 0x04002AC7 RID: 10951
		None = 0,
		// Token: 0x04002AC8 RID: 10952
		NoPropagateInherit = 1,
		// Token: 0x04002AC9 RID: 10953
		InheritOnly = 2
	}
}
