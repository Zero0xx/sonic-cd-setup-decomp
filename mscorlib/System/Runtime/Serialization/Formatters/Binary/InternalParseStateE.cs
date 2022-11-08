using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007C9 RID: 1993
	[Serializable]
	internal enum InternalParseStateE
	{
		// Token: 0x04002374 RID: 9076
		Initial,
		// Token: 0x04002375 RID: 9077
		Object,
		// Token: 0x04002376 RID: 9078
		Member,
		// Token: 0x04002377 RID: 9079
		MemberChild
	}
}
