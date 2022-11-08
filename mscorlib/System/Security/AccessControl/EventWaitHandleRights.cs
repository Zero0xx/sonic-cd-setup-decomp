using System;

namespace System.Security.AccessControl
{
	// Token: 0x020008EF RID: 2287
	[Flags]
	public enum EventWaitHandleRights
	{
		// Token: 0x04002AED RID: 10989
		Modify = 2,
		// Token: 0x04002AEE RID: 10990
		Delete = 65536,
		// Token: 0x04002AEF RID: 10991
		ReadPermissions = 131072,
		// Token: 0x04002AF0 RID: 10992
		ChangePermissions = 262144,
		// Token: 0x04002AF1 RID: 10993
		TakeOwnership = 524288,
		// Token: 0x04002AF2 RID: 10994
		Synchronize = 1048576,
		// Token: 0x04002AF3 RID: 10995
		FullControl = 2031619
	}
}
