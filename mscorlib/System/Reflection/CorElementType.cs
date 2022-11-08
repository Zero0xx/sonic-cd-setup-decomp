using System;

namespace System.Reflection
{
	// Token: 0x02000315 RID: 789
	[Serializable]
	internal enum CorElementType : byte
	{
		// Token: 0x04000B7F RID: 2943
		End,
		// Token: 0x04000B80 RID: 2944
		Void,
		// Token: 0x04000B81 RID: 2945
		Boolean,
		// Token: 0x04000B82 RID: 2946
		Char,
		// Token: 0x04000B83 RID: 2947
		I1,
		// Token: 0x04000B84 RID: 2948
		U1,
		// Token: 0x04000B85 RID: 2949
		I2,
		// Token: 0x04000B86 RID: 2950
		U2,
		// Token: 0x04000B87 RID: 2951
		I4,
		// Token: 0x04000B88 RID: 2952
		U4,
		// Token: 0x04000B89 RID: 2953
		I8,
		// Token: 0x04000B8A RID: 2954
		U8,
		// Token: 0x04000B8B RID: 2955
		R4,
		// Token: 0x04000B8C RID: 2956
		R8,
		// Token: 0x04000B8D RID: 2957
		String,
		// Token: 0x04000B8E RID: 2958
		Ptr,
		// Token: 0x04000B8F RID: 2959
		ByRef,
		// Token: 0x04000B90 RID: 2960
		ValueType,
		// Token: 0x04000B91 RID: 2961
		Class,
		// Token: 0x04000B92 RID: 2962
		Array = 20,
		// Token: 0x04000B93 RID: 2963
		TypedByRef = 22,
		// Token: 0x04000B94 RID: 2964
		I = 24,
		// Token: 0x04000B95 RID: 2965
		U,
		// Token: 0x04000B96 RID: 2966
		FnPtr = 27,
		// Token: 0x04000B97 RID: 2967
		Object,
		// Token: 0x04000B98 RID: 2968
		SzArray,
		// Token: 0x04000B99 RID: 2969
		CModReqd = 31,
		// Token: 0x04000B9A RID: 2970
		CModOpt,
		// Token: 0x04000B9B RID: 2971
		Internal,
		// Token: 0x04000B9C RID: 2972
		Modifier = 64,
		// Token: 0x04000B9D RID: 2973
		Sentinel,
		// Token: 0x04000B9E RID: 2974
		Pinned = 69
	}
}
