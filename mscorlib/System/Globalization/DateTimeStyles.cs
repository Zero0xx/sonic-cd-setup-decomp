using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003A6 RID: 934
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum DateTimeStyles
	{
		// Token: 0x04001065 RID: 4197
		None = 0,
		// Token: 0x04001066 RID: 4198
		AllowLeadingWhite = 1,
		// Token: 0x04001067 RID: 4199
		AllowTrailingWhite = 2,
		// Token: 0x04001068 RID: 4200
		AllowInnerWhite = 4,
		// Token: 0x04001069 RID: 4201
		AllowWhiteSpaces = 7,
		// Token: 0x0400106A RID: 4202
		NoCurrentDateDefault = 8,
		// Token: 0x0400106B RID: 4203
		AdjustToUniversal = 16,
		// Token: 0x0400106C RID: 4204
		AssumeLocal = 32,
		// Token: 0x0400106D RID: 4205
		AssumeUniversal = 64,
		// Token: 0x0400106E RID: 4206
		RoundtripKind = 128
	}
}
