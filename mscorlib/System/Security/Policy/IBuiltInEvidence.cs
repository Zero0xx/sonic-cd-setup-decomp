using System;

namespace System.Security.Policy
{
	// Token: 0x02000494 RID: 1172
	internal interface IBuiltInEvidence
	{
		// Token: 0x06002E70 RID: 11888
		int OutputToBuffer(char[] buffer, int position, bool verbose);

		// Token: 0x06002E71 RID: 11889
		int InitFromBuffer(char[] buffer, int position);

		// Token: 0x06002E72 RID: 11890
		int GetRequiredSize(bool verbose);
	}
}
