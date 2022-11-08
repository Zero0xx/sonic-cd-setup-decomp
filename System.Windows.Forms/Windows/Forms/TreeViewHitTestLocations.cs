using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000708 RID: 1800
	[Flags]
	[ComVisible(true)]
	public enum TreeViewHitTestLocations
	{
		// Token: 0x04003A25 RID: 14885
		None = 1,
		// Token: 0x04003A26 RID: 14886
		Image = 2,
		// Token: 0x04003A27 RID: 14887
		Label = 4,
		// Token: 0x04003A28 RID: 14888
		Indent = 8,
		// Token: 0x04003A29 RID: 14889
		AboveClientArea = 256,
		// Token: 0x04003A2A RID: 14890
		BelowClientArea = 512,
		// Token: 0x04003A2B RID: 14891
		LeftOfClientArea = 2048,
		// Token: 0x04003A2C RID: 14892
		RightOfClientArea = 1024,
		// Token: 0x04003A2D RID: 14893
		RightOfLabel = 32,
		// Token: 0x04003A2E RID: 14894
		StateImage = 64,
		// Token: 0x04003A2F RID: 14895
		PlusMinus = 16
	}
}
