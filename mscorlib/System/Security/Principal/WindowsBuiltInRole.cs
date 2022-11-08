using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004D3 RID: 1235
	[ComVisible(true)]
	[Serializable]
	public enum WindowsBuiltInRole
	{
		// Token: 0x040018D1 RID: 6353
		Administrator = 544,
		// Token: 0x040018D2 RID: 6354
		User,
		// Token: 0x040018D3 RID: 6355
		Guest,
		// Token: 0x040018D4 RID: 6356
		PowerUser,
		// Token: 0x040018D5 RID: 6357
		AccountOperator,
		// Token: 0x040018D6 RID: 6358
		SystemOperator,
		// Token: 0x040018D7 RID: 6359
		PrintOperator,
		// Token: 0x040018D8 RID: 6360
		BackupOperator,
		// Token: 0x040018D9 RID: 6361
		Replicator
	}
}
