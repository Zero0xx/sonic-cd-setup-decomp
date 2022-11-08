using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x02000392 RID: 914
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum CompareOptions
	{
		// Token: 0x04000F55 RID: 3925
		None = 0,
		// Token: 0x04000F56 RID: 3926
		IgnoreCase = 1,
		// Token: 0x04000F57 RID: 3927
		IgnoreNonSpace = 2,
		// Token: 0x04000F58 RID: 3928
		IgnoreSymbols = 4,
		// Token: 0x04000F59 RID: 3929
		IgnoreKanaType = 8,
		// Token: 0x04000F5A RID: 3930
		IgnoreWidth = 16,
		// Token: 0x04000F5B RID: 3931
		OrdinalIgnoreCase = 268435456,
		// Token: 0x04000F5C RID: 3932
		StringSort = 536870912,
		// Token: 0x04000F5D RID: 3933
		Ordinal = 1073741824
	}
}
