using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000601 RID: 1537
	internal struct MibIcmpStats
	{
		// Token: 0x04002D7F RID: 11647
		internal uint messages;

		// Token: 0x04002D80 RID: 11648
		internal uint errors;

		// Token: 0x04002D81 RID: 11649
		internal uint destinationUnreachables;

		// Token: 0x04002D82 RID: 11650
		internal uint timeExceeds;

		// Token: 0x04002D83 RID: 11651
		internal uint parameterProblems;

		// Token: 0x04002D84 RID: 11652
		internal uint sourceQuenches;

		// Token: 0x04002D85 RID: 11653
		internal uint redirects;

		// Token: 0x04002D86 RID: 11654
		internal uint echoRequests;

		// Token: 0x04002D87 RID: 11655
		internal uint echoReplies;

		// Token: 0x04002D88 RID: 11656
		internal uint timestampRequests;

		// Token: 0x04002D89 RID: 11657
		internal uint timestampReplies;

		// Token: 0x04002D8A RID: 11658
		internal uint addressMaskRequests;

		// Token: 0x04002D8B RID: 11659
		internal uint addressMaskReplies;
	}
}
