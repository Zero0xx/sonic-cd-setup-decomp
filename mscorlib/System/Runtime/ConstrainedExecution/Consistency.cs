using System;

namespace System.Runtime.ConstrainedExecution
{
	// Token: 0x020004D5 RID: 1237
	[Serializable]
	public enum Consistency
	{
		// Token: 0x040018DF RID: 6367
		MayCorruptProcess,
		// Token: 0x040018E0 RID: 6368
		MayCorruptAppDomain,
		// Token: 0x040018E1 RID: 6369
		MayCorruptInstance,
		// Token: 0x040018E2 RID: 6370
		WillNotCorruptState
	}
}
