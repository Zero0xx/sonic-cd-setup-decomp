using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x020000EC RID: 236
	[ComVisible(true)]
	[Serializable]
	public enum PlatformID
	{
		// Token: 0x0400047C RID: 1148
		Win32S,
		// Token: 0x0400047D RID: 1149
		Win32Windows,
		// Token: 0x0400047E RID: 1150
		Win32NT,
		// Token: 0x0400047F RID: 1151
		WinCE,
		// Token: 0x04000480 RID: 1152
		Unix,
		// Token: 0x04000481 RID: 1153
		Xbox,
		// Token: 0x04000482 RID: 1154
		MacOSX
	}
}
