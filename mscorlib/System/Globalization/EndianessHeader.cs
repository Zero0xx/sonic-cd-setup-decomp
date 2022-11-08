using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x020003DA RID: 986
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct EndianessHeader
	{
		// Token: 0x04001329 RID: 4905
		internal uint leOffset;

		// Token: 0x0400132A RID: 4906
		internal uint beOffset;
	}
}
