using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x0200037D RID: 893
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum StreamingContextStates
	{
		// Token: 0x04000EB2 RID: 3762
		CrossProcess = 1,
		// Token: 0x04000EB3 RID: 3763
		CrossMachine = 2,
		// Token: 0x04000EB4 RID: 3764
		File = 4,
		// Token: 0x04000EB5 RID: 3765
		Persistence = 8,
		// Token: 0x04000EB6 RID: 3766
		Remoting = 16,
		// Token: 0x04000EB7 RID: 3767
		Other = 32,
		// Token: 0x04000EB8 RID: 3768
		Clone = 64,
		// Token: 0x04000EB9 RID: 3769
		CrossAppDomain = 128,
		// Token: 0x04000EBA RID: 3770
		All = 255
	}
}
