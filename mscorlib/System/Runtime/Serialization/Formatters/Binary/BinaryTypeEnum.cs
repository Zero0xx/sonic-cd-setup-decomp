using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007BF RID: 1983
	[Serializable]
	internal enum BinaryTypeEnum
	{
		// Token: 0x04002335 RID: 9013
		Primitive,
		// Token: 0x04002336 RID: 9014
		String,
		// Token: 0x04002337 RID: 9015
		Object,
		// Token: 0x04002338 RID: 9016
		ObjectUrt,
		// Token: 0x04002339 RID: 9017
		ObjectUser,
		// Token: 0x0400233A RID: 9018
		ObjectArray,
		// Token: 0x0400233B RID: 9019
		StringArray,
		// Token: 0x0400233C RID: 9020
		PrimitiveArray
	}
}
