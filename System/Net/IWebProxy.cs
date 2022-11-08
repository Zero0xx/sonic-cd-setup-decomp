using System;

namespace System.Net
{
	// Token: 0x020003AB RID: 939
	public interface IWebProxy
	{
		// Token: 0x06001D86 RID: 7558
		Uri GetProxy(Uri destination);

		// Token: 0x06001D87 RID: 7559
		bool IsBypassed(Uri host);

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001D88 RID: 7560
		// (set) Token: 0x06001D89 RID: 7561
		ICredentials Credentials { get; set; }
	}
}
