using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004EF RID: 1263
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum TypeLibVarFlags
	{
		// Token: 0x04001926 RID: 6438
		FReadOnly = 1,
		// Token: 0x04001927 RID: 6439
		FSource = 2,
		// Token: 0x04001928 RID: 6440
		FBindable = 4,
		// Token: 0x04001929 RID: 6441
		FRequestEdit = 8,
		// Token: 0x0400192A RID: 6442
		FDisplayBind = 16,
		// Token: 0x0400192B RID: 6443
		FDefaultBind = 32,
		// Token: 0x0400192C RID: 6444
		FHidden = 64,
		// Token: 0x0400192D RID: 6445
		FRestricted = 128,
		// Token: 0x0400192E RID: 6446
		FDefaultCollelem = 256,
		// Token: 0x0400192F RID: 6447
		FUiDefault = 512,
		// Token: 0x04001930 RID: 6448
		FNonBrowsable = 1024,
		// Token: 0x04001931 RID: 6449
		FReplaceable = 2048,
		// Token: 0x04001932 RID: 6450
		FImmediateBind = 4096
	}
}
