using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005BA RID: 1466
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum FileOptions
	{
		// Token: 0x04001C43 RID: 7235
		None = 0,
		// Token: 0x04001C44 RID: 7236
		WriteThrough = -2147483648,
		// Token: 0x04001C45 RID: 7237
		Asynchronous = 1073741824,
		// Token: 0x04001C46 RID: 7238
		RandomAccess = 268435456,
		// Token: 0x04001C47 RID: 7239
		DeleteOnClose = 67108864,
		// Token: 0x04001C48 RID: 7240
		SequentialScan = 134217728,
		// Token: 0x04001C49 RID: 7241
		Encrypted = 16384
	}
}
