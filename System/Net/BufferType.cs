using System;

namespace System.Net
{
	// Token: 0x020003F7 RID: 1015
	internal enum BufferType
	{
		// Token: 0x04002027 RID: 8231
		Empty,
		// Token: 0x04002028 RID: 8232
		Data,
		// Token: 0x04002029 RID: 8233
		Token,
		// Token: 0x0400202A RID: 8234
		Parameters,
		// Token: 0x0400202B RID: 8235
		Missing,
		// Token: 0x0400202C RID: 8236
		Extra,
		// Token: 0x0400202D RID: 8237
		Trailer,
		// Token: 0x0400202E RID: 8238
		Header,
		// Token: 0x0400202F RID: 8239
		Padding = 9,
		// Token: 0x04002030 RID: 8240
		Stream,
		// Token: 0x04002031 RID: 8241
		ChannelBindings = 14,
		// Token: 0x04002032 RID: 8242
		TargetHost = 16,
		// Token: 0x04002033 RID: 8243
		ReadOnlyFlag = -2147483648,
		// Token: 0x04002034 RID: 8244
		ReadOnlyWithChecksum = 268435456
	}
}
