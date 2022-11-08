using System;

namespace System.Security.AccessControl
{
	// Token: 0x020008F9 RID: 2297
	[Flags]
	public enum MutexRights
	{
		// Token: 0x04002B0E RID: 11022
		Modify = 1,
		// Token: 0x04002B0F RID: 11023
		Delete = 65536,
		// Token: 0x04002B10 RID: 11024
		ReadPermissions = 131072,
		// Token: 0x04002B11 RID: 11025
		ChangePermissions = 262144,
		// Token: 0x04002B12 RID: 11026
		TakeOwnership = 524288,
		// Token: 0x04002B13 RID: 11027
		Synchronize = 1048576,
		// Token: 0x04002B14 RID: 11028
		FullControl = 2031617
	}
}
