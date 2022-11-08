using System;

namespace System.Net
{
	// Token: 0x0200039C RID: 924
	internal class SystemNetworkCredential : NetworkCredential
	{
		// Token: 0x06001CDE RID: 7390 RVA: 0x0006E11A File Offset: 0x0006D11A
		private SystemNetworkCredential() : base(string.Empty, string.Empty, string.Empty)
		{
		}

		// Token: 0x04001D48 RID: 7496
		internal static readonly SystemNetworkCredential defaultCredential = new SystemNetworkCredential();
	}
}
