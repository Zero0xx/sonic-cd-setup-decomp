using System;

namespace System.Windows.Forms
{
	// Token: 0x020005B6 RID: 1462
	[Flags]
	public enum BatteryChargeStatus
	{
		// Token: 0x04003136 RID: 12598
		High = 1,
		// Token: 0x04003137 RID: 12599
		Low = 2,
		// Token: 0x04003138 RID: 12600
		Critical = 4,
		// Token: 0x04003139 RID: 12601
		Charging = 8,
		// Token: 0x0400313A RID: 12602
		NoSystemBattery = 128,
		// Token: 0x0400313B RID: 12603
		Unknown = 255
	}
}
