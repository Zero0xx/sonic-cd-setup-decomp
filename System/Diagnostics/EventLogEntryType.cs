using System;

namespace System.Diagnostics
{
	// Token: 0x02000752 RID: 1874
	public enum EventLogEntryType
	{
		// Token: 0x040032AC RID: 12972
		Error = 1,
		// Token: 0x040032AD RID: 12973
		Warning,
		// Token: 0x040032AE RID: 12974
		Information = 4,
		// Token: 0x040032AF RID: 12975
		SuccessAudit = 8,
		// Token: 0x040032B0 RID: 12976
		FailureAudit = 16
	}
}
