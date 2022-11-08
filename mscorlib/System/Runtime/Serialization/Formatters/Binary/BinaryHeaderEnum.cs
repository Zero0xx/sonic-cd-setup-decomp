using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007BE RID: 1982
	[Serializable]
	internal enum BinaryHeaderEnum
	{
		// Token: 0x0400231D RID: 8989
		SerializedStreamHeader,
		// Token: 0x0400231E RID: 8990
		Object,
		// Token: 0x0400231F RID: 8991
		ObjectWithMap,
		// Token: 0x04002320 RID: 8992
		ObjectWithMapAssemId,
		// Token: 0x04002321 RID: 8993
		ObjectWithMapTyped,
		// Token: 0x04002322 RID: 8994
		ObjectWithMapTypedAssemId,
		// Token: 0x04002323 RID: 8995
		ObjectString,
		// Token: 0x04002324 RID: 8996
		Array,
		// Token: 0x04002325 RID: 8997
		MemberPrimitiveTyped,
		// Token: 0x04002326 RID: 8998
		MemberReference,
		// Token: 0x04002327 RID: 8999
		ObjectNull,
		// Token: 0x04002328 RID: 9000
		MessageEnd,
		// Token: 0x04002329 RID: 9001
		Assembly,
		// Token: 0x0400232A RID: 9002
		ObjectNullMultiple256,
		// Token: 0x0400232B RID: 9003
		ObjectNullMultiple,
		// Token: 0x0400232C RID: 9004
		ArraySinglePrimitive,
		// Token: 0x0400232D RID: 9005
		ArraySingleObject,
		// Token: 0x0400232E RID: 9006
		ArraySingleString,
		// Token: 0x0400232F RID: 9007
		CrossAppDomainMap,
		// Token: 0x04002330 RID: 9008
		CrossAppDomainString,
		// Token: 0x04002331 RID: 9009
		CrossAppDomainAssembly,
		// Token: 0x04002332 RID: 9010
		MethodCall,
		// Token: 0x04002333 RID: 9011
		MethodReturn
	}
}
