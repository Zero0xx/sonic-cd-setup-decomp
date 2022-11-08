using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007C0 RID: 1984
	[Serializable]
	internal enum BinaryArrayTypeEnum
	{
		// Token: 0x0400233E RID: 9022
		Single,
		// Token: 0x0400233F RID: 9023
		Jagged,
		// Token: 0x04002340 RID: 9024
		Rectangular,
		// Token: 0x04002341 RID: 9025
		SingleOffset,
		// Token: 0x04002342 RID: 9026
		JaggedOffset,
		// Token: 0x04002343 RID: 9027
		RectangularOffset
	}
}
