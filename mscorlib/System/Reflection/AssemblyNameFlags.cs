using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002F3 RID: 755
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum AssemblyNameFlags
	{
		// Token: 0x04000AEA RID: 2794
		None = 0,
		// Token: 0x04000AEB RID: 2795
		PublicKey = 1,
		// Token: 0x04000AEC RID: 2796
		EnableJITcompileOptimizer = 16384,
		// Token: 0x04000AED RID: 2797
		EnableJITcompileTracking = 32768,
		// Token: 0x04000AEE RID: 2798
		Retargetable = 256
	}
}
