using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005EB RID: 1515
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum MethodImplOptions
	{
		// Token: 0x04001CF0 RID: 7408
		Unmanaged = 4,
		// Token: 0x04001CF1 RID: 7409
		ForwardRef = 16,
		// Token: 0x04001CF2 RID: 7410
		PreserveSig = 128,
		// Token: 0x04001CF3 RID: 7411
		InternalCall = 4096,
		// Token: 0x04001CF4 RID: 7412
		Synchronized = 32,
		// Token: 0x04001CF5 RID: 7413
		NoInlining = 8,
		// Token: 0x04001CF6 RID: 7414
		NoOptimization = 64
	}
}
