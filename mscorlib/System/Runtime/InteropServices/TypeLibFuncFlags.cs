using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004EE RID: 1262
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum TypeLibFuncFlags
	{
		// Token: 0x04001918 RID: 6424
		FRestricted = 1,
		// Token: 0x04001919 RID: 6425
		FSource = 2,
		// Token: 0x0400191A RID: 6426
		FBindable = 4,
		// Token: 0x0400191B RID: 6427
		FRequestEdit = 8,
		// Token: 0x0400191C RID: 6428
		FDisplayBind = 16,
		// Token: 0x0400191D RID: 6429
		FDefaultBind = 32,
		// Token: 0x0400191E RID: 6430
		FHidden = 64,
		// Token: 0x0400191F RID: 6431
		FUsesGetLastError = 128,
		// Token: 0x04001920 RID: 6432
		FDefaultCollelem = 256,
		// Token: 0x04001921 RID: 6433
		FUiDefault = 512,
		// Token: 0x04001922 RID: 6434
		FNonBrowsable = 1024,
		// Token: 0x04001923 RID: 6435
		FReplaceable = 2048,
		// Token: 0x04001924 RID: 6436
		FImmediateBind = 4096
	}
}
