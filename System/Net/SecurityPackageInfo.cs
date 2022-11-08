using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000547 RID: 1351
	internal struct SecurityPackageInfo
	{
		// Token: 0x0400281C RID: 10268
		internal int Capabilities;

		// Token: 0x0400281D RID: 10269
		internal short Version;

		// Token: 0x0400281E RID: 10270
		internal short RPCID;

		// Token: 0x0400281F RID: 10271
		internal int MaxToken;

		// Token: 0x04002820 RID: 10272
		internal IntPtr Name;

		// Token: 0x04002821 RID: 10273
		internal IntPtr Comment;

		// Token: 0x04002822 RID: 10274
		internal static readonly int Size = Marshal.SizeOf(typeof(SecurityPackageInfo));

		// Token: 0x04002823 RID: 10275
		internal static readonly int NameOffest = (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "Name");
	}
}
