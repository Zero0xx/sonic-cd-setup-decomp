using System;

namespace System.Net
{
	// Token: 0x020003E6 RID: 998
	public interface IAuthenticationModule
	{
		// Token: 0x06002065 RID: 8293
		Authorization Authenticate(string challenge, WebRequest request, ICredentials credentials);

		// Token: 0x06002066 RID: 8294
		Authorization PreAuthenticate(WebRequest request, ICredentials credentials);

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06002067 RID: 8295
		bool CanPreAuthenticate { get; }

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002068 RID: 8296
		string AuthenticationType { get; }
	}
}
