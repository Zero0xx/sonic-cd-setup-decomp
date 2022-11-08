using System;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000057 RID: 87
	[Flags]
	public enum ClipStatusFlags
	{
		// Token: 0x04000D76 RID: 3446
		Plane5 = 2048,
		// Token: 0x04000D77 RID: 3447
		Plane4 = 1024,
		// Token: 0x04000D78 RID: 3448
		Plane3 = 512,
		// Token: 0x04000D79 RID: 3449
		Plane2 = 256,
		// Token: 0x04000D7A RID: 3450
		Plane1 = 128,
		// Token: 0x04000D7B RID: 3451
		Plane0 = 64,
		// Token: 0x04000D7C RID: 3452
		Bottom = 8,
		// Token: 0x04000D7D RID: 3453
		Top = 4,
		// Token: 0x04000D7E RID: 3454
		Right = 2,
		// Token: 0x04000D7F RID: 3455
		Left = 1,
		// Token: 0x04000D80 RID: 3456
		All = 4095,
		// Token: 0x04000D81 RID: 3457
		Back = 32,
		// Token: 0x04000D82 RID: 3458
		Front = 16
	}
}
