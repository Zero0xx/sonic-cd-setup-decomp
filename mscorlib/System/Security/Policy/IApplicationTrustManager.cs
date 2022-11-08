using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x020004A7 RID: 1191
	[ComVisible(true)]
	public interface IApplicationTrustManager : ISecurityEncodable
	{
		// Token: 0x06002F35 RID: 12085
		ApplicationTrust DetermineApplicationTrust(ActivationContext activationContext, TrustManagerContext context);
	}
}
