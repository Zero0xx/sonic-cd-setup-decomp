using System;

namespace System.Net
{
	// Token: 0x020004F4 RID: 1268
	[Flags]
	internal enum ContextFlags
	{
		// Token: 0x040026D1 RID: 9937
		Zero = 0,
		// Token: 0x040026D2 RID: 9938
		Delegate = 1,
		// Token: 0x040026D3 RID: 9939
		MutualAuth = 2,
		// Token: 0x040026D4 RID: 9940
		ReplayDetect = 4,
		// Token: 0x040026D5 RID: 9941
		SequenceDetect = 8,
		// Token: 0x040026D6 RID: 9942
		Confidentiality = 16,
		// Token: 0x040026D7 RID: 9943
		UseSessionKey = 32,
		// Token: 0x040026D8 RID: 9944
		AllocateMemory = 256,
		// Token: 0x040026D9 RID: 9945
		Connection = 2048,
		// Token: 0x040026DA RID: 9946
		InitExtendedError = 16384,
		// Token: 0x040026DB RID: 9947
		AcceptExtendedError = 32768,
		// Token: 0x040026DC RID: 9948
		InitStream = 32768,
		// Token: 0x040026DD RID: 9949
		AcceptStream = 65536,
		// Token: 0x040026DE RID: 9950
		InitIntegrity = 65536,
		// Token: 0x040026DF RID: 9951
		AcceptIntegrity = 131072,
		// Token: 0x040026E0 RID: 9952
		InitManualCredValidation = 524288,
		// Token: 0x040026E1 RID: 9953
		InitUseSuppliedCreds = 128,
		// Token: 0x040026E2 RID: 9954
		InitIdentify = 131072,
		// Token: 0x040026E3 RID: 9955
		AcceptIdentify = 524288,
		// Token: 0x040026E4 RID: 9956
		ProxyBindings = 67108864,
		// Token: 0x040026E5 RID: 9957
		AllowMissingBindings = 268435456
	}
}
