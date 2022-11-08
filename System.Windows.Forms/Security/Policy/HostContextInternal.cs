using System;

namespace System.Security.Policy
{
	// Token: 0x02000713 RID: 1811
	internal class HostContextInternal
	{
		// Token: 0x06006072 RID: 24690 RVA: 0x00160208 File Offset: 0x0015F208
		public HostContextInternal(TrustManagerContext trustManagerContext)
		{
			if (trustManagerContext == null)
			{
				this.persist = true;
				return;
			}
			this.ignorePersistedDecision = trustManagerContext.IgnorePersistedDecision;
			this.noPrompt = trustManagerContext.NoPrompt;
			this.persist = trustManagerContext.Persist;
			this.previousAppId = trustManagerContext.PreviousApplicationIdentity;
		}

		// Token: 0x17001461 RID: 5217
		// (get) Token: 0x06006073 RID: 24691 RVA: 0x00160256 File Offset: 0x0015F256
		public bool IgnorePersistedDecision
		{
			get
			{
				return this.ignorePersistedDecision;
			}
		}

		// Token: 0x17001462 RID: 5218
		// (get) Token: 0x06006074 RID: 24692 RVA: 0x0016025E File Offset: 0x0015F25E
		public bool NoPrompt
		{
			get
			{
				return this.noPrompt;
			}
		}

		// Token: 0x17001463 RID: 5219
		// (get) Token: 0x06006075 RID: 24693 RVA: 0x00160266 File Offset: 0x0015F266
		public bool Persist
		{
			get
			{
				return this.persist;
			}
		}

		// Token: 0x17001464 RID: 5220
		// (get) Token: 0x06006076 RID: 24694 RVA: 0x0016026E File Offset: 0x0015F26E
		public ApplicationIdentity PreviousAppId
		{
			get
			{
				return this.previousAppId;
			}
		}

		// Token: 0x04003A50 RID: 14928
		private bool ignorePersistedDecision;

		// Token: 0x04003A51 RID: 14929
		private bool noPrompt;

		// Token: 0x04003A52 RID: 14930
		private bool persist;

		// Token: 0x04003A53 RID: 14931
		private ApplicationIdentity previousAppId;
	}
}
