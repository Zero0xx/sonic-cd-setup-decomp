using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003DB RID: 987
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct CultureTableHeader
	{
		// Token: 0x0400132B RID: 4907
		internal uint version;

		// Token: 0x0400132C RID: 4908
		internal ushort hash0;

		// Token: 0x0400132D RID: 4909
		internal ushort hash1;

		// Token: 0x0400132E RID: 4910
		internal ushort hash2;

		// Token: 0x0400132F RID: 4911
		internal ushort hash3;

		// Token: 0x04001330 RID: 4912
		internal ushort hash4;

		// Token: 0x04001331 RID: 4913
		internal ushort hash5;

		// Token: 0x04001332 RID: 4914
		internal ushort hash6;

		// Token: 0x04001333 RID: 4915
		internal ushort hash7;

		// Token: 0x04001334 RID: 4916
		internal ushort headerSize;

		// Token: 0x04001335 RID: 4917
		internal ushort numLcidItems;

		// Token: 0x04001336 RID: 4918
		internal ushort numCultureItems;

		// Token: 0x04001337 RID: 4919
		internal ushort sizeCultureItem;

		// Token: 0x04001338 RID: 4920
		internal uint offsetToCultureItemData;

		// Token: 0x04001339 RID: 4921
		internal ushort numCultureNames;

		// Token: 0x0400133A RID: 4922
		internal ushort numRegionNames;

		// Token: 0x0400133B RID: 4923
		internal uint cultureIDTableOffset;

		// Token: 0x0400133C RID: 4924
		internal uint cultureNameTableOffset;

		// Token: 0x0400133D RID: 4925
		internal uint regionNameTableOffset;

		// Token: 0x0400133E RID: 4926
		internal ushort numCalendarItems;

		// Token: 0x0400133F RID: 4927
		internal ushort sizeCalendarItem;

		// Token: 0x04001340 RID: 4928
		internal uint offsetToCalendarItemData;

		// Token: 0x04001341 RID: 4929
		internal uint offsetToDataPool;

		// Token: 0x04001342 RID: 4930
		internal ushort Unused_numIetfNames;

		// Token: 0x04001343 RID: 4931
		internal ushort Unused_Padding;

		// Token: 0x04001344 RID: 4932
		internal uint Unused_ietfNameTableOffset;
	}
}
