using System;

namespace System.Security
{
	// Token: 0x0200067A RID: 1658
	[Flags]
	internal enum PermissionTokenType
	{
		// Token: 0x04001EE6 RID: 7910
		Normal = 1,
		// Token: 0x04001EE7 RID: 7911
		IUnrestricted = 2,
		// Token: 0x04001EE8 RID: 7912
		DontKnow = 4,
		// Token: 0x04001EE9 RID: 7913
		BuiltIn = 8
	}
}
