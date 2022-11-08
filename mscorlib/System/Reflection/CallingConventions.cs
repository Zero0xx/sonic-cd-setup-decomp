using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002F7 RID: 759
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum CallingConventions
	{
		// Token: 0x04000B0B RID: 2827
		Standard = 1,
		// Token: 0x04000B0C RID: 2828
		VarArgs = 2,
		// Token: 0x04000B0D RID: 2829
		Any = 3,
		// Token: 0x04000B0E RID: 2830
		HasThis = 32,
		// Token: 0x04000B0F RID: 2831
		ExplicitThis = 64
	}
}
