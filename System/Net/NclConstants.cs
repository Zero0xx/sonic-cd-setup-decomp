using System;

namespace System.Net
{
	// Token: 0x020003EB RID: 1003
	internal static class NclConstants
	{
		// Token: 0x04001FB0 RID: 8112
		internal static readonly object Sentinel = new object();

		// Token: 0x04001FB1 RID: 8113
		internal static readonly object[] EmptyObjectArray = new object[0];

		// Token: 0x04001FB2 RID: 8114
		internal static readonly Uri[] EmptyUriArray = new Uri[0];

		// Token: 0x04001FB3 RID: 8115
		internal static readonly byte[] CRLF = new byte[]
		{
			13,
			10
		};

		// Token: 0x04001FB4 RID: 8116
		internal static readonly byte[] ChunkTerminator = new byte[]
		{
			48,
			13,
			10,
			13,
			10
		};
	}
}
