using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005B5 RID: 1461
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum FileAccess
	{
		// Token: 0x04001C33 RID: 7219
		Read = 1,
		// Token: 0x04001C34 RID: 7220
		Write = 2,
		// Token: 0x04001C35 RID: 7221
		ReadWrite = 3
	}
}
