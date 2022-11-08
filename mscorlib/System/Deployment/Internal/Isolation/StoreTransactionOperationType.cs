using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200022E RID: 558
	internal enum StoreTransactionOperationType
	{
		// Token: 0x040008F4 RID: 2292
		Invalid,
		// Token: 0x040008F5 RID: 2293
		SetCanonicalizationContext = 14,
		// Token: 0x040008F6 RID: 2294
		StageComponent = 20,
		// Token: 0x040008F7 RID: 2295
		PinDeployment,
		// Token: 0x040008F8 RID: 2296
		UnpinDeployment,
		// Token: 0x040008F9 RID: 2297
		StageComponentFile,
		// Token: 0x040008FA RID: 2298
		InstallDeployment,
		// Token: 0x040008FB RID: 2299
		UninstallDeployment,
		// Token: 0x040008FC RID: 2300
		SetDeploymentMetadata,
		// Token: 0x040008FD RID: 2301
		Scavenge
	}
}
