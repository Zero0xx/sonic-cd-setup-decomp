using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F4 RID: 1268
	[ComVisible(true)]
	[Serializable]
	public enum UnmanagedType
	{
		// Token: 0x04001964 RID: 6500
		Bool = 2,
		// Token: 0x04001965 RID: 6501
		I1,
		// Token: 0x04001966 RID: 6502
		U1,
		// Token: 0x04001967 RID: 6503
		I2,
		// Token: 0x04001968 RID: 6504
		U2,
		// Token: 0x04001969 RID: 6505
		I4,
		// Token: 0x0400196A RID: 6506
		U4,
		// Token: 0x0400196B RID: 6507
		I8,
		// Token: 0x0400196C RID: 6508
		U8,
		// Token: 0x0400196D RID: 6509
		R4,
		// Token: 0x0400196E RID: 6510
		R8,
		// Token: 0x0400196F RID: 6511
		Currency = 15,
		// Token: 0x04001970 RID: 6512
		BStr = 19,
		// Token: 0x04001971 RID: 6513
		LPStr,
		// Token: 0x04001972 RID: 6514
		LPWStr,
		// Token: 0x04001973 RID: 6515
		LPTStr,
		// Token: 0x04001974 RID: 6516
		ByValTStr,
		// Token: 0x04001975 RID: 6517
		IUnknown = 25,
		// Token: 0x04001976 RID: 6518
		IDispatch,
		// Token: 0x04001977 RID: 6519
		Struct,
		// Token: 0x04001978 RID: 6520
		Interface,
		// Token: 0x04001979 RID: 6521
		SafeArray,
		// Token: 0x0400197A RID: 6522
		ByValArray,
		// Token: 0x0400197B RID: 6523
		SysInt,
		// Token: 0x0400197C RID: 6524
		SysUInt,
		// Token: 0x0400197D RID: 6525
		VBByRefStr = 34,
		// Token: 0x0400197E RID: 6526
		AnsiBStr,
		// Token: 0x0400197F RID: 6527
		TBStr,
		// Token: 0x04001980 RID: 6528
		VariantBool,
		// Token: 0x04001981 RID: 6529
		FunctionPtr,
		// Token: 0x04001982 RID: 6530
		AsAny = 40,
		// Token: 0x04001983 RID: 6531
		LPArray = 42,
		// Token: 0x04001984 RID: 6532
		LPStruct,
		// Token: 0x04001985 RID: 6533
		CustomMarshaler,
		// Token: 0x04001986 RID: 6534
		Error
	}
}
