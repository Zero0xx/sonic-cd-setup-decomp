using System;

namespace System.Security.Util
{
	// Token: 0x02000483 RID: 1155
	[Flags]
	[Serializable]
	internal enum QuickCacheEntryType
	{
		// Token: 0x0400179C RID: 6044
		FullTrustZoneMyComputer = 16777216,
		// Token: 0x0400179D RID: 6045
		FullTrustZoneIntranet = 33554432,
		// Token: 0x0400179E RID: 6046
		FullTrustZoneInternet = 67108864,
		// Token: 0x0400179F RID: 6047
		FullTrustZoneTrusted = 134217728,
		// Token: 0x040017A0 RID: 6048
		FullTrustZoneUntrusted = 268435456,
		// Token: 0x040017A1 RID: 6049
		FullTrustAll = 536870912
	}
}
