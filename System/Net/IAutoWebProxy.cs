using System;

namespace System.Net
{
	// Token: 0x020003AC RID: 940
	internal interface IAutoWebProxy : IWebProxy
	{
		// Token: 0x06001D8A RID: 7562
		ProxyChain GetProxies(Uri destination);
	}
}
