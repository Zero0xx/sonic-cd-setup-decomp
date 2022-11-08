using System;

namespace System.Net.Mail
{
	// Token: 0x020006D9 RID: 1753
	internal class SmtpPooledStream : PooledStream
	{
		// Token: 0x0600360A RID: 13834 RVA: 0x000E6BF4 File Offset: 0x000E5BF4
		internal SmtpPooledStream(ConnectionPool connectionPool, TimeSpan lifetime, bool checkLifetime) : base(connectionPool, lifetime, checkLifetime)
		{
		}

		// Token: 0x0400314A RID: 12618
		internal bool previouslyUsed;

		// Token: 0x0400314B RID: 12619
		internal bool dsnEnabled;

		// Token: 0x0400314C RID: 12620
		internal ICredentialsByHost creds;
	}
}
