using System;

namespace System.Diagnostics
{
	// Token: 0x02000795 RID: 1941
	internal class ProcessData
	{
		// Token: 0x06003BE1 RID: 15329 RVA: 0x001002AC File Offset: 0x000FF2AC
		public ProcessData(int pid, long startTime)
		{
			this.ProcessId = pid;
			this.StartupTime = startTime;
		}

		// Token: 0x04003484 RID: 13444
		public int ProcessId;

		// Token: 0x04003485 RID: 13445
		public long StartupTime;
	}
}
