using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000600 RID: 1536
	[ComVisible(true)]
	public enum ScrollEventType
	{
		// Token: 0x040034DF RID: 13535
		SmallDecrement,
		// Token: 0x040034E0 RID: 13536
		SmallIncrement,
		// Token: 0x040034E1 RID: 13537
		LargeDecrement,
		// Token: 0x040034E2 RID: 13538
		LargeIncrement,
		// Token: 0x040034E3 RID: 13539
		ThumbPosition,
		// Token: 0x040034E4 RID: 13540
		ThumbTrack,
		// Token: 0x040034E5 RID: 13541
		First,
		// Token: 0x040034E6 RID: 13542
		Last,
		// Token: 0x040034E7 RID: 13543
		EndScroll
	}
}
