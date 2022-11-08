﻿using System;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200005D RID: 93
	internal enum CMS_SYSTEM_PROTECTION
	{
		// Token: 0x04000BDB RID: 3035
		CMS_SYSTEM_PROTECTION_READ_ONLY_IGNORE_WRITES = 1,
		// Token: 0x04000BDC RID: 3036
		CMS_SYSTEM_PROTECTION_READ_ONLY_FAIL_WRITES,
		// Token: 0x04000BDD RID: 3037
		CMS_SYSTEM_PROTECTION_OS_ONLY_IGNORE_WRITES,
		// Token: 0x04000BDE RID: 3038
		CMS_SYSTEM_PROTECTION_OS_ONLY_FAIL_WRITES,
		// Token: 0x04000BDF RID: 3039
		CMS_SYSTEM_PROTECTION_TRANSACTED,
		// Token: 0x04000BE0 RID: 3040
		CMS_SYSTEM_PROTECTION_APPLICATION_VIRTUALIZED,
		// Token: 0x04000BE1 RID: 3041
		CMS_SYSTEM_PROTECTION_USER_VIRTUALIZED,
		// Token: 0x04000BE2 RID: 3042
		CMS_SYSTEM_PROTECTION_APPLICATION_AND_USER_VIRTUALIZED,
		// Token: 0x04000BE3 RID: 3043
		CMS_SYSTEM_PROTECTION_INHERIT,
		// Token: 0x04000BE4 RID: 3044
		CMS_SYSTEM_PROTECTION_NOT_PROTECTED
	}
}
