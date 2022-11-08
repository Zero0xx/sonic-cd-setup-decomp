using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000617 RID: 1559
	[Flags]
	public enum NetworkInformationAccess
	{
		// Token: 0x04002DCE RID: 11726
		None = 0,
		// Token: 0x04002DCF RID: 11727
		Read = 1,
		// Token: 0x04002DD0 RID: 11728
		Ping = 4
	}
}
