﻿using System;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200015F RID: 351
	internal enum CMS_ASSEMBLY_DEPLOYMENT_FLAG
	{
		// Token: 0x04000612 RID: 1554
		CMS_ASSEMBLY_DEPLOYMENT_FLAG_BEFORE_APPLICATION_STARTUP = 4,
		// Token: 0x04000613 RID: 1555
		CMS_ASSEMBLY_DEPLOYMENT_FLAG_RUN_AFTER_INSTALL = 16,
		// Token: 0x04000614 RID: 1556
		CMS_ASSEMBLY_DEPLOYMENT_FLAG_INSTALL = 32,
		// Token: 0x04000615 RID: 1557
		CMS_ASSEMBLY_DEPLOYMENT_FLAG_TRUST_URL_PARAMETERS = 64,
		// Token: 0x04000616 RID: 1558
		CMS_ASSEMBLY_DEPLOYMENT_FLAG_DISALLOW_URL_ACTIVATION = 128,
		// Token: 0x04000617 RID: 1559
		CMS_ASSEMBLY_DEPLOYMENT_FLAG_MAP_FILE_EXTENSIONS = 256,
		// Token: 0x04000618 RID: 1560
		CMS_ASSEMBLY_DEPLOYMENT_FLAG_CREATE_DESKTOP_SHORTCUT = 512
	}
}