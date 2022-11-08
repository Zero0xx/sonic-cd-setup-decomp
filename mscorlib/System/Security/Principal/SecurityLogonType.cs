using System;

namespace System.Security.Principal
{
	// Token: 0x020004CF RID: 1231
	[Serializable]
	internal enum SecurityLogonType
	{
		// Token: 0x040018B4 RID: 6324
		Interactive = 2,
		// Token: 0x040018B5 RID: 6325
		Network,
		// Token: 0x040018B6 RID: 6326
		Batch,
		// Token: 0x040018B7 RID: 6327
		Service,
		// Token: 0x040018B8 RID: 6328
		Proxy,
		// Token: 0x040018B9 RID: 6329
		Unlock
	}
}
