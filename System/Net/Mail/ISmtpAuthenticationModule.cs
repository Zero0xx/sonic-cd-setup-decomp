using System;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net.Mail
{
	// Token: 0x02000694 RID: 1684
	internal interface ISmtpAuthenticationModule
	{
		// Token: 0x060033F4 RID: 13300
		Authorization Authenticate(string challenge, NetworkCredential credentials, object sessionCookie, string spn, ChannelBinding channelBindingToken);

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x060033F5 RID: 13301
		string AuthenticationType { get; }

		// Token: 0x060033F6 RID: 13302
		void CloseContext(object sessionCookie);
	}
}
