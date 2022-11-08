using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005BE RID: 1470
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum FileAttributes
	{
		// Token: 0x04001C7D RID: 7293
		ReadOnly = 1,
		// Token: 0x04001C7E RID: 7294
		Hidden = 2,
		// Token: 0x04001C7F RID: 7295
		System = 4,
		// Token: 0x04001C80 RID: 7296
		Directory = 16,
		// Token: 0x04001C81 RID: 7297
		Archive = 32,
		// Token: 0x04001C82 RID: 7298
		Device = 64,
		// Token: 0x04001C83 RID: 7299
		Normal = 128,
		// Token: 0x04001C84 RID: 7300
		Temporary = 256,
		// Token: 0x04001C85 RID: 7301
		SparseFile = 512,
		// Token: 0x04001C86 RID: 7302
		ReparsePoint = 1024,
		// Token: 0x04001C87 RID: 7303
		Compressed = 2048,
		// Token: 0x04001C88 RID: 7304
		Offline = 4096,
		// Token: 0x04001C89 RID: 7305
		NotContentIndexed = 8192,
		// Token: 0x04001C8A RID: 7306
		Encrypted = 16384
	}
}
