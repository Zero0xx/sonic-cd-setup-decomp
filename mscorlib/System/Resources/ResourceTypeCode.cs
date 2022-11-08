using System;

namespace System.Resources
{
	// Token: 0x0200043B RID: 1083
	[Serializable]
	internal enum ResourceTypeCode
	{
		// Token: 0x04001595 RID: 5525
		Null,
		// Token: 0x04001596 RID: 5526
		String,
		// Token: 0x04001597 RID: 5527
		Boolean,
		// Token: 0x04001598 RID: 5528
		Char,
		// Token: 0x04001599 RID: 5529
		Byte,
		// Token: 0x0400159A RID: 5530
		SByte,
		// Token: 0x0400159B RID: 5531
		Int16,
		// Token: 0x0400159C RID: 5532
		UInt16,
		// Token: 0x0400159D RID: 5533
		Int32,
		// Token: 0x0400159E RID: 5534
		UInt32,
		// Token: 0x0400159F RID: 5535
		Int64,
		// Token: 0x040015A0 RID: 5536
		UInt64,
		// Token: 0x040015A1 RID: 5537
		Single,
		// Token: 0x040015A2 RID: 5538
		Double,
		// Token: 0x040015A3 RID: 5539
		Decimal,
		// Token: 0x040015A4 RID: 5540
		DateTime,
		// Token: 0x040015A5 RID: 5541
		TimeSpan,
		// Token: 0x040015A6 RID: 5542
		LastPrimitive = 16,
		// Token: 0x040015A7 RID: 5543
		ByteArray = 32,
		// Token: 0x040015A8 RID: 5544
		Stream,
		// Token: 0x040015A9 RID: 5545
		StartOfUserTypes = 64
	}
}
