using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007C3 RID: 1987
	[Serializable]
	internal enum InternalParseTypeE
	{
		// Token: 0x0400234C RID: 9036
		Empty,
		// Token: 0x0400234D RID: 9037
		SerializedStreamHeader,
		// Token: 0x0400234E RID: 9038
		Object,
		// Token: 0x0400234F RID: 9039
		Member,
		// Token: 0x04002350 RID: 9040
		ObjectEnd,
		// Token: 0x04002351 RID: 9041
		MemberEnd,
		// Token: 0x04002352 RID: 9042
		Headers,
		// Token: 0x04002353 RID: 9043
		HeadersEnd,
		// Token: 0x04002354 RID: 9044
		SerializedStreamHeaderEnd,
		// Token: 0x04002355 RID: 9045
		Envelope,
		// Token: 0x04002356 RID: 9046
		EnvelopeEnd,
		// Token: 0x04002357 RID: 9047
		Body,
		// Token: 0x04002358 RID: 9048
		BodyEnd
	}
}
