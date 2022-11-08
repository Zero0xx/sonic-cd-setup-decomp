using System;

namespace System.Net
{
	// Token: 0x02000374 RID: 884
	public interface ICredentialPolicy
	{
		// Token: 0x06001BB2 RID: 7090
		bool ShouldSendCredential(Uri challengeUri, WebRequest request, NetworkCredential credential, IAuthenticationModule authenticationModule);
	}
}
