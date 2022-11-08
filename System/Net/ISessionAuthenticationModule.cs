using System;

namespace System.Net
{
	// Token: 0x020004D2 RID: 1234
	internal interface ISessionAuthenticationModule : IAuthenticationModule
	{
		// Token: 0x06002660 RID: 9824
		bool Update(string challenge, WebRequest webRequest);

		// Token: 0x06002661 RID: 9825
		void ClearSession(WebRequest webRequest);

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06002662 RID: 9826
		bool CanUseDefaultCredentials { get; }
	}
}
