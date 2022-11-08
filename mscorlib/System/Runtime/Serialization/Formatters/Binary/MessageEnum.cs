using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007CB RID: 1995
	[Flags]
	[Serializable]
	internal enum MessageEnum
	{
		// Token: 0x0400238D RID: 9101
		NoArgs = 1,
		// Token: 0x0400238E RID: 9102
		ArgsInline = 2,
		// Token: 0x0400238F RID: 9103
		ArgsIsArray = 4,
		// Token: 0x04002390 RID: 9104
		ArgsInArray = 8,
		// Token: 0x04002391 RID: 9105
		NoContext = 16,
		// Token: 0x04002392 RID: 9106
		ContextInline = 32,
		// Token: 0x04002393 RID: 9107
		ContextInArray = 64,
		// Token: 0x04002394 RID: 9108
		MethodSignatureInArray = 128,
		// Token: 0x04002395 RID: 9109
		PropertyInArray = 256,
		// Token: 0x04002396 RID: 9110
		NoReturnValue = 512,
		// Token: 0x04002397 RID: 9111
		ReturnValueVoid = 1024,
		// Token: 0x04002398 RID: 9112
		ReturnValueInline = 2048,
		// Token: 0x04002399 RID: 9113
		ReturnValueInArray = 4096,
		// Token: 0x0400239A RID: 9114
		ExceptionInArray = 8192,
		// Token: 0x0400239B RID: 9115
		GenericMethod = 32768
	}
}
