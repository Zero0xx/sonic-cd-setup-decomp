using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x020006E8 RID: 1768
	[Editor("System.Windows.Forms.Design.BorderSidesEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[ComVisible(true)]
	[Flags]
	public enum ToolStripStatusLabelBorderSides
	{
		// Token: 0x0400394D RID: 14669
		All = 15,
		// Token: 0x0400394E RID: 14670
		Bottom = 8,
		// Token: 0x0400394F RID: 14671
		Left = 1,
		// Token: 0x04003950 RID: 14672
		Right = 4,
		// Token: 0x04003951 RID: 14673
		Top = 2,
		// Token: 0x04003952 RID: 14674
		None = 0
	}
}
