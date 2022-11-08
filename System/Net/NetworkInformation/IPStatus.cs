using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005E0 RID: 1504
	public enum IPStatus
	{
		// Token: 0x04002C98 RID: 11416
		Success,
		// Token: 0x04002C99 RID: 11417
		DestinationNetworkUnreachable = 11002,
		// Token: 0x04002C9A RID: 11418
		DestinationHostUnreachable,
		// Token: 0x04002C9B RID: 11419
		DestinationProtocolUnreachable,
		// Token: 0x04002C9C RID: 11420
		DestinationPortUnreachable,
		// Token: 0x04002C9D RID: 11421
		DestinationProhibited = 11004,
		// Token: 0x04002C9E RID: 11422
		NoResources = 11006,
		// Token: 0x04002C9F RID: 11423
		BadOption,
		// Token: 0x04002CA0 RID: 11424
		HardwareError,
		// Token: 0x04002CA1 RID: 11425
		PacketTooBig,
		// Token: 0x04002CA2 RID: 11426
		TimedOut,
		// Token: 0x04002CA3 RID: 11427
		BadRoute = 11012,
		// Token: 0x04002CA4 RID: 11428
		TtlExpired,
		// Token: 0x04002CA5 RID: 11429
		TtlReassemblyTimeExceeded,
		// Token: 0x04002CA6 RID: 11430
		ParameterProblem,
		// Token: 0x04002CA7 RID: 11431
		SourceQuench,
		// Token: 0x04002CA8 RID: 11432
		BadDestination = 11018,
		// Token: 0x04002CA9 RID: 11433
		DestinationUnreachable = 11040,
		// Token: 0x04002CAA RID: 11434
		TimeExceeded,
		// Token: 0x04002CAB RID: 11435
		BadHeader,
		// Token: 0x04002CAC RID: 11436
		UnrecognizedNextHeader,
		// Token: 0x04002CAD RID: 11437
		IcmpError,
		// Token: 0x04002CAE RID: 11438
		DestinationScopeMismatch,
		// Token: 0x04002CAF RID: 11439
		Unknown = -1
	}
}
