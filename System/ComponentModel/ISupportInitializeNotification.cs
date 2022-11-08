using System;

namespace System.ComponentModel
{
	// Token: 0x02000102 RID: 258
	public interface ISupportInitializeNotification : ISupportInitialize
	{
		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600082B RID: 2091
		bool IsInitialized { get; }

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x0600082C RID: 2092
		// (remove) Token: 0x0600082D RID: 2093
		event EventHandler Initialized;
	}
}
