using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004CB RID: 1227
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum TokenAccessLevels
	{
		// Token: 0x04001891 RID: 6289
		AssignPrimary = 1,
		// Token: 0x04001892 RID: 6290
		Duplicate = 2,
		// Token: 0x04001893 RID: 6291
		Impersonate = 4,
		// Token: 0x04001894 RID: 6292
		Query = 8,
		// Token: 0x04001895 RID: 6293
		QuerySource = 16,
		// Token: 0x04001896 RID: 6294
		AdjustPrivileges = 32,
		// Token: 0x04001897 RID: 6295
		AdjustGroups = 64,
		// Token: 0x04001898 RID: 6296
		AdjustDefault = 128,
		// Token: 0x04001899 RID: 6297
		AdjustSessionId = 256,
		// Token: 0x0400189A RID: 6298
		Read = 131080,
		// Token: 0x0400189B RID: 6299
		Write = 131296,
		// Token: 0x0400189C RID: 6300
		AllAccess = 983551,
		// Token: 0x0400189D RID: 6301
		MaximumAllowed = 33554432
	}
}
