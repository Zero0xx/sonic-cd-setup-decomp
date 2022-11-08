using System;

namespace System.Net
{
	// Token: 0x02000398 RID: 920
	public interface ICredentialsByHost
	{
		// Token: 0x06001CB7 RID: 7351
		NetworkCredential GetCredential(string host, int port, string authenticationType);
	}
}
