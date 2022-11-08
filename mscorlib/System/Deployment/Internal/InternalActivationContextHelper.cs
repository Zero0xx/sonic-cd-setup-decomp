using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal
{
	// Token: 0x0200006B RID: 107
	[ComVisible(false)]
	public static class InternalActivationContextHelper
	{
		// Token: 0x06000636 RID: 1590 RVA: 0x0001562A File Offset: 0x0001462A
		public static object GetActivationContextData(ActivationContext appInfo)
		{
			return appInfo.ActivationContextData;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00015632 File Offset: 0x00014632
		public static object GetApplicationComponentManifest(ActivationContext appInfo)
		{
			return appInfo.ApplicationComponentManifest;
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0001563A File Offset: 0x0001463A
		public static object GetDeploymentComponentManifest(ActivationContext appInfo)
		{
			return appInfo.DeploymentComponentManifest;
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00015642 File Offset: 0x00014642
		public static void PrepareForExecution(ActivationContext appInfo)
		{
			appInfo.PrepareForExecution();
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001564A File Offset: 0x0001464A
		public static bool IsFirstRun(ActivationContext appInfo)
		{
			return appInfo.LastApplicationStateResult == ActivationContext.ApplicationStateDisposition.RunningFirstTime;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00015659 File Offset: 0x00014659
		public static byte[] GetApplicationManifestBytes(ActivationContext appInfo)
		{
			if (appInfo == null)
			{
				throw new ArgumentNullException("appInfo");
			}
			return appInfo.GetApplicationManifestBytes();
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0001566F File Offset: 0x0001466F
		public static byte[] GetDeploymentManifestBytes(ActivationContext appInfo)
		{
			if (appInfo == null)
			{
				throw new ArgumentNullException("appInfo");
			}
			return appInfo.GetDeploymentManifestBytes();
		}
	}
}
