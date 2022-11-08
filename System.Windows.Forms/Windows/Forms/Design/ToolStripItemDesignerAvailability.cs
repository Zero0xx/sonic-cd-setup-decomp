using System;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000783 RID: 1923
	[Flags]
	public enum ToolStripItemDesignerAvailability
	{
		// Token: 0x04003C0A RID: 15370
		None = 0,
		// Token: 0x04003C0B RID: 15371
		ToolStrip = 1,
		// Token: 0x04003C0C RID: 15372
		MenuStrip = 2,
		// Token: 0x04003C0D RID: 15373
		ContextMenuStrip = 4,
		// Token: 0x04003C0E RID: 15374
		StatusStrip = 8,
		// Token: 0x04003C0F RID: 15375
		All = 15
	}
}
