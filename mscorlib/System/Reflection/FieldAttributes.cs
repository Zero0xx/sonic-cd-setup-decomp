using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200030E RID: 782
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum FieldAttributes
	{
		// Token: 0x04000B4F RID: 2895
		FieldAccessMask = 7,
		// Token: 0x04000B50 RID: 2896
		PrivateScope = 0,
		// Token: 0x04000B51 RID: 2897
		Private = 1,
		// Token: 0x04000B52 RID: 2898
		FamANDAssem = 2,
		// Token: 0x04000B53 RID: 2899
		Assembly = 3,
		// Token: 0x04000B54 RID: 2900
		Family = 4,
		// Token: 0x04000B55 RID: 2901
		FamORAssem = 5,
		// Token: 0x04000B56 RID: 2902
		Public = 6,
		// Token: 0x04000B57 RID: 2903
		Static = 16,
		// Token: 0x04000B58 RID: 2904
		InitOnly = 32,
		// Token: 0x04000B59 RID: 2905
		Literal = 64,
		// Token: 0x04000B5A RID: 2906
		NotSerialized = 128,
		// Token: 0x04000B5B RID: 2907
		SpecialName = 512,
		// Token: 0x04000B5C RID: 2908
		PinvokeImpl = 8192,
		// Token: 0x04000B5D RID: 2909
		ReservedMask = 38144,
		// Token: 0x04000B5E RID: 2910
		RTSpecialName = 1024,
		// Token: 0x04000B5F RID: 2911
		HasFieldMarshal = 4096,
		// Token: 0x04000B60 RID: 2912
		HasDefault = 32768,
		// Token: 0x04000B61 RID: 2913
		HasFieldRVA = 256
	}
}
