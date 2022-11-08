using System;

namespace System.IO.Ports
{
	// Token: 0x020007A9 RID: 1961
	public enum SerialError
	{
		// Token: 0x04003514 RID: 13588
		TXFull = 256,
		// Token: 0x04003515 RID: 13589
		RXOver = 1,
		// Token: 0x04003516 RID: 13590
		Overrun,
		// Token: 0x04003517 RID: 13591
		RXParity = 4,
		// Token: 0x04003518 RID: 13592
		Frame = 8
	}
}
