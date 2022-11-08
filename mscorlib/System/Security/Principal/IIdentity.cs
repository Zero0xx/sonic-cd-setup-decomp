using System;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x020004C4 RID: 1220
	[ComVisible(true)]
	public interface IIdentity
	{
		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x060030E1 RID: 12513
		string Name { get; }

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x060030E2 RID: 12514
		string AuthenticationType { get; }

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x060030E3 RID: 12515
		bool IsAuthenticated { get; }
	}
}
