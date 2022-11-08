using System;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005FC RID: 1532
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct MibIfRow
	{
		// Token: 0x04002D37 RID: 11575
		internal const int MAX_INTERFACE_NAME_LEN = 256;

		// Token: 0x04002D38 RID: 11576
		internal const int MAXLEN_IFDESCR = 256;

		// Token: 0x04002D39 RID: 11577
		internal const int MAXLEN_PHYSADDR = 8;

		// Token: 0x04002D3A RID: 11578
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		internal string wszName;

		// Token: 0x04002D3B RID: 11579
		internal uint dwIndex;

		// Token: 0x04002D3C RID: 11580
		internal uint dwType;

		// Token: 0x04002D3D RID: 11581
		internal uint dwMtu;

		// Token: 0x04002D3E RID: 11582
		internal uint dwSpeed;

		// Token: 0x04002D3F RID: 11583
		internal uint dwPhysAddrLen;

		// Token: 0x04002D40 RID: 11584
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		internal byte[] bPhysAddr;

		// Token: 0x04002D41 RID: 11585
		internal uint dwAdminStatus;

		// Token: 0x04002D42 RID: 11586
		internal OldOperationalStatus operStatus;

		// Token: 0x04002D43 RID: 11587
		internal uint dwLastChange;

		// Token: 0x04002D44 RID: 11588
		internal uint dwInOctets;

		// Token: 0x04002D45 RID: 11589
		internal uint dwInUcastPkts;

		// Token: 0x04002D46 RID: 11590
		internal uint dwInNUcastPkts;

		// Token: 0x04002D47 RID: 11591
		internal uint dwInDiscards;

		// Token: 0x04002D48 RID: 11592
		internal uint dwInErrors;

		// Token: 0x04002D49 RID: 11593
		internal uint dwInUnknownProtos;

		// Token: 0x04002D4A RID: 11594
		internal uint dwOutOctets;

		// Token: 0x04002D4B RID: 11595
		internal uint dwOutUcastPkts;

		// Token: 0x04002D4C RID: 11596
		internal uint dwOutNUcastPkts;

		// Token: 0x04002D4D RID: 11597
		internal uint dwOutDiscards;

		// Token: 0x04002D4E RID: 11598
		internal uint dwOutErrors;

		// Token: 0x04002D4F RID: 11599
		internal uint dwOutQLen;

		// Token: 0x04002D50 RID: 11600
		internal uint dwDescrLen;

		// Token: 0x04002D51 RID: 11601
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		internal byte[] bDescr;
	}
}
