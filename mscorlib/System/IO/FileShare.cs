using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005BB RID: 1467
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum FileShare
	{
		// Token: 0x04001C4B RID: 7243
		None = 0,
		// Token: 0x04001C4C RID: 7244
		Read = 1,
		// Token: 0x04001C4D RID: 7245
		Write = 2,
		// Token: 0x04001C4E RID: 7246
		ReadWrite = 3,
		// Token: 0x04001C4F RID: 7247
		Delete = 4,
		// Token: 0x04001C50 RID: 7248
		Inheritable = 16
	}
}
