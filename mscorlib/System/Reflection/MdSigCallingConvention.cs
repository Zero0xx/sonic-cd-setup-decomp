using System;

namespace System.Reflection
{
	// Token: 0x02000318 RID: 792
	[Flags]
	[Serializable]
	internal enum MdSigCallingConvention : byte
	{
		// Token: 0x04000BA7 RID: 2983
		CallConvMask = 15,
		// Token: 0x04000BA8 RID: 2984
		Default = 0,
		// Token: 0x04000BA9 RID: 2985
		C = 1,
		// Token: 0x04000BAA RID: 2986
		StdCall = 2,
		// Token: 0x04000BAB RID: 2987
		ThisCall = 3,
		// Token: 0x04000BAC RID: 2988
		FastCall = 4,
		// Token: 0x04000BAD RID: 2989
		Vararg = 5,
		// Token: 0x04000BAE RID: 2990
		Field = 6,
		// Token: 0x04000BAF RID: 2991
		LoclaSig = 7,
		// Token: 0x04000BB0 RID: 2992
		Property = 8,
		// Token: 0x04000BB1 RID: 2993
		Unmgd = 9,
		// Token: 0x04000BB2 RID: 2994
		GenericInst = 10,
		// Token: 0x04000BB3 RID: 2995
		Generic = 16,
		// Token: 0x04000BB4 RID: 2996
		HasThis = 32,
		// Token: 0x04000BB5 RID: 2997
		ExplicitThis = 64
	}
}
