using System;

namespace System.Security.Policy
{
	// Token: 0x02000710 RID: 1808
	[Serializable]
	internal class ApplicationTrustExtraInfo
	{
		// Token: 0x17001459 RID: 5209
		// (get) Token: 0x0600605E RID: 24670 RVA: 0x00160069 File Offset: 0x0015F069
		// (set) Token: 0x0600605F RID: 24671 RVA: 0x00160071 File Offset: 0x0015F071
		public bool RequestsShellIntegration
		{
			get
			{
				return this.requestsShellIntegration;
			}
			set
			{
				this.requestsShellIntegration = value;
			}
		}

		// Token: 0x04003A40 RID: 14912
		private bool requestsShellIntegration = true;
	}
}
