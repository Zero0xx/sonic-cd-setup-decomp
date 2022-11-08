using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004ED RID: 1261
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum TypeLibTypeFlags
	{
		// Token: 0x04001909 RID: 6409
		FAppObject = 1,
		// Token: 0x0400190A RID: 6410
		FCanCreate = 2,
		// Token: 0x0400190B RID: 6411
		FLicensed = 4,
		// Token: 0x0400190C RID: 6412
		FPreDeclId = 8,
		// Token: 0x0400190D RID: 6413
		FHidden = 16,
		// Token: 0x0400190E RID: 6414
		FControl = 32,
		// Token: 0x0400190F RID: 6415
		FDual = 64,
		// Token: 0x04001910 RID: 6416
		FNonExtensible = 128,
		// Token: 0x04001911 RID: 6417
		FOleAutomation = 256,
		// Token: 0x04001912 RID: 6418
		FRestricted = 512,
		// Token: 0x04001913 RID: 6419
		FAggregatable = 1024,
		// Token: 0x04001914 RID: 6420
		FReplaceable = 2048,
		// Token: 0x04001915 RID: 6421
		FDispatchable = 4096,
		// Token: 0x04001916 RID: 6422
		FReverseBind = 8192
	}
}
