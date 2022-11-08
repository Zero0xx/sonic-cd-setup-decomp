using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200064C RID: 1612
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum ReflectionPermissionFlag
	{
		// Token: 0x04001E26 RID: 7718
		NoFlags = 0,
		// Token: 0x04001E27 RID: 7719
		[Obsolete("This API has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		TypeInformation = 1,
		// Token: 0x04001E28 RID: 7720
		MemberAccess = 2,
		// Token: 0x04001E29 RID: 7721
		ReflectionEmit = 4,
		// Token: 0x04001E2A RID: 7722
		[ComVisible(false)]
		RestrictedMemberAccess = 8,
		// Token: 0x04001E2B RID: 7723
		AllFlags = 7
	}
}
