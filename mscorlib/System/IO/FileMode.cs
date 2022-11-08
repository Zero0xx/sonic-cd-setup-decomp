using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005B8 RID: 1464
	[ComVisible(true)]
	[Serializable]
	public enum FileMode
	{
		// Token: 0x04001C3A RID: 7226
		CreateNew = 1,
		// Token: 0x04001C3B RID: 7227
		Create,
		// Token: 0x04001C3C RID: 7228
		Open,
		// Token: 0x04001C3D RID: 7229
		OpenOrCreate,
		// Token: 0x04001C3E RID: 7230
		Truncate,
		// Token: 0x04001C3F RID: 7231
		Append
	}
}
