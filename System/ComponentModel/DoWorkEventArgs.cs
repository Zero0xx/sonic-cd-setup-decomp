using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000DA RID: 218
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class DoWorkEventArgs : CancelEventArgs
	{
		// Token: 0x06000756 RID: 1878 RVA: 0x0001AB9D File Offset: 0x00019B9D
		public DoWorkEventArgs(object argument)
		{
			this.argument = argument;
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x0001ABAC File Offset: 0x00019BAC
		[SRDescription("BackgroundWorker_DoWorkEventArgs_Argument")]
		public object Argument
		{
			get
			{
				return this.argument;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x0001ABB4 File Offset: 0x00019BB4
		// (set) Token: 0x06000759 RID: 1881 RVA: 0x0001ABBC File Offset: 0x00019BBC
		[SRDescription("BackgroundWorker_DoWorkEventArgs_Result")]
		public object Result
		{
			get
			{
				return this.result;
			}
			set
			{
				this.result = value;
			}
		}

		// Token: 0x0400095D RID: 2397
		private object result;

		// Token: 0x0400095E RID: 2398
		private object argument;
	}
}
