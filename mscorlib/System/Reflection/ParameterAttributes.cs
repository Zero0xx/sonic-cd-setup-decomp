using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000339 RID: 825
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum ParameterAttributes
	{
		// Token: 0x04000DA2 RID: 3490
		None = 0,
		// Token: 0x04000DA3 RID: 3491
		In = 1,
		// Token: 0x04000DA4 RID: 3492
		Out = 2,
		// Token: 0x04000DA5 RID: 3493
		Lcid = 4,
		// Token: 0x04000DA6 RID: 3494
		Retval = 8,
		// Token: 0x04000DA7 RID: 3495
		Optional = 16,
		// Token: 0x04000DA8 RID: 3496
		ReservedMask = 61440,
		// Token: 0x04000DA9 RID: 3497
		HasDefault = 4096,
		// Token: 0x04000DAA RID: 3498
		HasFieldMarshal = 8192,
		// Token: 0x04000DAB RID: 3499
		Reserved3 = 16384,
		// Token: 0x04000DAC RID: 3500
		Reserved4 = 32768
	}
}
