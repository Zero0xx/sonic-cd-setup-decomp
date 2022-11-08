using System;

namespace System.Net.Configuration
{
	// Token: 0x02000663 RID: 1635
	internal sealed class SmtpNetworkElementInternal
	{
		// Token: 0x060032A1 RID: 12961 RVA: 0x000D6EF0 File Offset: 0x000D5EF0
		internal SmtpNetworkElementInternal(SmtpNetworkElement element)
		{
			this.host = element.Host;
			this.port = element.Port;
			this.clientDomain = element.ClientDomain;
			this.targetname = element.TargetName;
			if (element.DefaultCredentials)
			{
				this.credential = (NetworkCredential)CredentialCache.DefaultCredentials;
				return;
			}
			if (element.UserName != null && element.UserName.Length > 0)
			{
				this.credential = new NetworkCredential(element.UserName, element.Password);
			}
		}

		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x060032A2 RID: 12962 RVA: 0x000D6F79 File Offset: 0x000D5F79
		internal NetworkCredential Credential
		{
			get
			{
				return this.credential;
			}
		}

		// Token: 0x17000BDF RID: 3039
		// (get) Token: 0x060032A3 RID: 12963 RVA: 0x000D6F81 File Offset: 0x000D5F81
		internal string Host
		{
			get
			{
				return this.host;
			}
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x060032A4 RID: 12964 RVA: 0x000D6F89 File Offset: 0x000D5F89
		internal string ClientDomain
		{
			get
			{
				return this.clientDomain;
			}
		}

		// Token: 0x17000BE1 RID: 3041
		// (get) Token: 0x060032A5 RID: 12965 RVA: 0x000D6F91 File Offset: 0x000D5F91
		internal int Port
		{
			get
			{
				return this.port;
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x060032A6 RID: 12966 RVA: 0x000D6F99 File Offset: 0x000D5F99
		internal string TargetName
		{
			get
			{
				return this.targetname;
			}
		}

		// Token: 0x04002F56 RID: 12118
		private string targetname;

		// Token: 0x04002F57 RID: 12119
		private string host;

		// Token: 0x04002F58 RID: 12120
		private string clientDomain;

		// Token: 0x04002F59 RID: 12121
		private int port;

		// Token: 0x04002F5A RID: 12122
		private NetworkCredential credential;
	}
}
