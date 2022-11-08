using System;

namespace System.Windows.Forms
{
	// Token: 0x02000720 RID: 1824
	[Flags]
	public enum ValidationConstraints
	{
		// Token: 0x04003A9E RID: 15006
		None = 0,
		// Token: 0x04003A9F RID: 15007
		Selectable = 1,
		// Token: 0x04003AA0 RID: 15008
		Enabled = 2,
		// Token: 0x04003AA1 RID: 15009
		Visible = 4,
		// Token: 0x04003AA2 RID: 15010
		TabStop = 8,
		// Token: 0x04003AA3 RID: 15011
		ImmediateChildren = 16
	}
}
