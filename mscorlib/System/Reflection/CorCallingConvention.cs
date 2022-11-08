using System;

namespace System.Reflection
{
	// Token: 0x02000314 RID: 788
	[Serializable]
	internal enum CorCallingConvention : byte
	{
		// Token: 0x04000B77 RID: 2935
		Default,
		// Token: 0x04000B78 RID: 2936
		Vararg = 5,
		// Token: 0x04000B79 RID: 2937
		Field,
		// Token: 0x04000B7A RID: 2938
		LocalSig,
		// Token: 0x04000B7B RID: 2939
		Property,
		// Token: 0x04000B7C RID: 2940
		Unmanaged,
		// Token: 0x04000B7D RID: 2941
		GenericInstance
	}
}
