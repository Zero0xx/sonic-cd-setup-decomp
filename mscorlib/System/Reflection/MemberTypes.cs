using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200032C RID: 812
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum MemberTypes
	{
		// Token: 0x04000D3A RID: 3386
		Constructor = 1,
		// Token: 0x04000D3B RID: 3387
		Event = 2,
		// Token: 0x04000D3C RID: 3388
		Field = 4,
		// Token: 0x04000D3D RID: 3389
		Method = 8,
		// Token: 0x04000D3E RID: 3390
		Property = 16,
		// Token: 0x04000D3F RID: 3391
		TypeInfo = 32,
		// Token: 0x04000D40 RID: 3392
		Custom = 64,
		// Token: 0x04000D41 RID: 3393
		NestedType = 128,
		// Token: 0x04000D42 RID: 3394
		All = 191
	}
}
