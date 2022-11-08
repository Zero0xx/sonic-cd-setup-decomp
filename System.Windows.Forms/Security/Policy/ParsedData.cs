using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Policy
{
	// Token: 0x02000712 RID: 1810
	internal class ParsedData
	{
		// Token: 0x1700145A RID: 5210
		// (get) Token: 0x06006064 RID: 24676 RVA: 0x00160190 File Offset: 0x0015F190
		// (set) Token: 0x06006065 RID: 24677 RVA: 0x00160198 File Offset: 0x0015F198
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

		// Token: 0x1700145B RID: 5211
		// (get) Token: 0x06006066 RID: 24678 RVA: 0x001601A1 File Offset: 0x0015F1A1
		// (set) Token: 0x06006067 RID: 24679 RVA: 0x001601A9 File Offset: 0x0015F1A9
		public X509Certificate2 Certificate
		{
			get
			{
				return this.certificate;
			}
			set
			{
				this.certificate = value;
			}
		}

		// Token: 0x1700145C RID: 5212
		// (get) Token: 0x06006068 RID: 24680 RVA: 0x001601B2 File Offset: 0x0015F1B2
		// (set) Token: 0x06006069 RID: 24681 RVA: 0x001601BA File Offset: 0x0015F1BA
		public string AppName
		{
			get
			{
				return this.appName;
			}
			set
			{
				this.appName = value;
			}
		}

		// Token: 0x1700145D RID: 5213
		// (get) Token: 0x0600606A RID: 24682 RVA: 0x001601C3 File Offset: 0x0015F1C3
		// (set) Token: 0x0600606B RID: 24683 RVA: 0x001601CB File Offset: 0x0015F1CB
		public string AppPublisher
		{
			get
			{
				return this.appPublisher;
			}
			set
			{
				this.appPublisher = value;
			}
		}

		// Token: 0x1700145E RID: 5214
		// (get) Token: 0x0600606C RID: 24684 RVA: 0x001601D4 File Offset: 0x0015F1D4
		// (set) Token: 0x0600606D RID: 24685 RVA: 0x001601DC File Offset: 0x0015F1DC
		public string AuthenticodedPublisher
		{
			get
			{
				return this.authenticodedPublisher;
			}
			set
			{
				this.authenticodedPublisher = value;
			}
		}

		// Token: 0x1700145F RID: 5215
		// (get) Token: 0x0600606E RID: 24686 RVA: 0x001601E5 File Offset: 0x0015F1E5
		// (set) Token: 0x0600606F RID: 24687 RVA: 0x001601ED File Offset: 0x0015F1ED
		public bool UseManifestForTrust
		{
			get
			{
				return this.disallowTrustOverride;
			}
			set
			{
				this.disallowTrustOverride = value;
			}
		}

		// Token: 0x17001460 RID: 5216
		// (get) Token: 0x06006070 RID: 24688 RVA: 0x001601F6 File Offset: 0x0015F1F6
		// (set) Token: 0x06006071 RID: 24689 RVA: 0x001601FE File Offset: 0x0015F1FE
		public string SupportUrl
		{
			get
			{
				return this.supportUrl;
			}
			set
			{
				this.supportUrl = value;
			}
		}

		// Token: 0x04003A49 RID: 14921
		private bool requestsShellIntegration;

		// Token: 0x04003A4A RID: 14922
		private string appName;

		// Token: 0x04003A4B RID: 14923
		private string appPublisher;

		// Token: 0x04003A4C RID: 14924
		private string supportUrl;

		// Token: 0x04003A4D RID: 14925
		private string authenticodedPublisher;

		// Token: 0x04003A4E RID: 14926
		private bool disallowTrustOverride;

		// Token: 0x04003A4F RID: 14927
		private X509Certificate2 certificate;
	}
}
