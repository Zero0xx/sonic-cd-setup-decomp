using System;

namespace System.Diagnostics
{
	// Token: 0x020002C3 RID: 707
	[Serializable]
	internal enum LoggingLevels
	{
		// Token: 0x04000A77 RID: 2679
		TraceLevel0,
		// Token: 0x04000A78 RID: 2680
		TraceLevel1,
		// Token: 0x04000A79 RID: 2681
		TraceLevel2,
		// Token: 0x04000A7A RID: 2682
		TraceLevel3,
		// Token: 0x04000A7B RID: 2683
		TraceLevel4,
		// Token: 0x04000A7C RID: 2684
		StatusLevel0 = 20,
		// Token: 0x04000A7D RID: 2685
		StatusLevel1,
		// Token: 0x04000A7E RID: 2686
		StatusLevel2,
		// Token: 0x04000A7F RID: 2687
		StatusLevel3,
		// Token: 0x04000A80 RID: 2688
		StatusLevel4,
		// Token: 0x04000A81 RID: 2689
		WarningLevel = 40,
		// Token: 0x04000A82 RID: 2690
		ErrorLevel = 50,
		// Token: 0x04000A83 RID: 2691
		PanicLevel = 100
	}
}
