using System;
using System.Net;

namespace Microsoft.Win32
{
	// Token: 0x02000379 RID: 889
	public class IntranetZoneCredentialPolicy : ICredentialPolicy
	{
		// Token: 0x06001BD8 RID: 7128 RVA: 0x00069353 File Offset: 0x00068353
		public IntranetZoneCredentialPolicy()
		{
			ExceptionHelper.ControlPolicyPermission.Demand();
			this._ManagerRef = (IInternetSecurityManager)new InternetSecurityManager();
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x00069378 File Offset: 0x00068378
		public virtual bool ShouldSendCredential(Uri challengeUri, WebRequest request, NetworkCredential credential, IAuthenticationModule authModule)
		{
			int num;
			this._ManagerRef.MapUrlToZone(challengeUri.AbsoluteUri, out num, 0);
			return num == 1;
		}

		// Token: 0x04001C80 RID: 7296
		private const int URLZONE_INTRANET = 1;

		// Token: 0x04001C81 RID: 7297
		private IInternetSecurityManager _ManagerRef;
	}
}
