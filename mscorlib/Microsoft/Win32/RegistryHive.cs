using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	// Token: 0x02000473 RID: 1139
	[ComVisible(true)]
	[Serializable]
	public enum RegistryHive
	{
		// Token: 0x0400176D RID: 5997
		ClassesRoot = -2147483648,
		// Token: 0x0400176E RID: 5998
		CurrentUser,
		// Token: 0x0400176F RID: 5999
		LocalMachine,
		// Token: 0x04001770 RID: 6000
		Users,
		// Token: 0x04001771 RID: 6001
		PerformanceData,
		// Token: 0x04001772 RID: 6002
		CurrentConfig,
		// Token: 0x04001773 RID: 6003
		DynData
	}
}
