using System;
using System.Runtime.InteropServices;

namespace System.Net.Mail
{
	// Token: 0x0200068D RID: 1677
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct MetadataRecord
	{
		// Token: 0x04002FD6 RID: 12246
		internal uint Identifier;

		// Token: 0x04002FD7 RID: 12247
		internal uint Attributes;

		// Token: 0x04002FD8 RID: 12248
		internal uint UserType;

		// Token: 0x04002FD9 RID: 12249
		internal uint DataType;

		// Token: 0x04002FDA RID: 12250
		internal uint DataLen;

		// Token: 0x04002FDB RID: 12251
		internal IntPtr DataBuf;

		// Token: 0x04002FDC RID: 12252
		internal uint DataTag;
	}
}
