using System;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x0200037D RID: 893
	internal interface IWebProxyFinder : IDisposable
	{
		// Token: 0x06001BEA RID: 7146
		bool GetProxies(Uri destination, out IList<string> proxyList);

		// Token: 0x06001BEB RID: 7147
		void Abort();

		// Token: 0x06001BEC RID: 7148
		void Reset();

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001BED RID: 7149
		bool IsValid { get; }
	}
}
