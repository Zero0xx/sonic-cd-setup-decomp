using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000603 RID: 1539
	internal struct MibIcmpStatsEx
	{
		// Token: 0x04002D8E RID: 11662
		internal uint dwMsgs;

		// Token: 0x04002D8F RID: 11663
		internal uint dwErrors;

		// Token: 0x04002D90 RID: 11664
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		internal uint[] rgdwTypeCount;
	}
}
