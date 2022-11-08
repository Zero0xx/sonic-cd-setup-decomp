using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000330 RID: 816
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum PortableExecutableKinds
	{
		// Token: 0x04000D6E RID: 3438
		NotAPortableExecutableImage = 0,
		// Token: 0x04000D6F RID: 3439
		ILOnly = 1,
		// Token: 0x04000D70 RID: 3440
		Required32Bit = 2,
		// Token: 0x04000D71 RID: 3441
		PE32Plus = 4,
		// Token: 0x04000D72 RID: 3442
		Unmanaged32Bit = 8
	}
}
