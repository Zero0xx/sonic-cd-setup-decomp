using System;

namespace System.Windows.Forms
{
	// Token: 0x020005EF RID: 1519
	[Flags]
	public enum RichTextBoxSelectionTypes
	{
		// Token: 0x040034A3 RID: 13475
		Empty = 0,
		// Token: 0x040034A4 RID: 13476
		Text = 1,
		// Token: 0x040034A5 RID: 13477
		Object = 2,
		// Token: 0x040034A6 RID: 13478
		MultiChar = 4,
		// Token: 0x040034A7 RID: 13479
		MultiObject = 8
	}
}
