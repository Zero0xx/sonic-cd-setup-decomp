using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x02000775 RID: 1909
	internal class ProcessInfo
	{
		// Token: 0x040033A9 RID: 13225
		public ArrayList threadInfoList = new ArrayList();

		// Token: 0x040033AA RID: 13226
		public int basePriority;

		// Token: 0x040033AB RID: 13227
		public string processName;

		// Token: 0x040033AC RID: 13228
		public int processId;

		// Token: 0x040033AD RID: 13229
		public int handleCount;

		// Token: 0x040033AE RID: 13230
		public long poolPagedBytes;

		// Token: 0x040033AF RID: 13231
		public long poolNonpagedBytes;

		// Token: 0x040033B0 RID: 13232
		public long virtualBytes;

		// Token: 0x040033B1 RID: 13233
		public long virtualBytesPeak;

		// Token: 0x040033B2 RID: 13234
		public long workingSetPeak;

		// Token: 0x040033B3 RID: 13235
		public long workingSet;

		// Token: 0x040033B4 RID: 13236
		public long pageFileBytesPeak;

		// Token: 0x040033B5 RID: 13237
		public long pageFileBytes;

		// Token: 0x040033B6 RID: 13238
		public long privateBytes;

		// Token: 0x040033B7 RID: 13239
		public int mainModuleId;

		// Token: 0x040033B8 RID: 13240
		public int sessionId;
	}
}
