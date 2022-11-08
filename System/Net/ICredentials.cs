using System;

namespace System.Net
{
	// Token: 0x02000397 RID: 919
	public interface ICredentials
	{
		// Token: 0x06001CB6 RID: 7350
		NetworkCredential GetCredential(Uri uri, string authType);
	}
}
