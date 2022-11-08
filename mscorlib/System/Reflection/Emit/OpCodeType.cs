using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200083F RID: 2111
	[ComVisible(true)]
	[Serializable]
	public enum OpCodeType
	{
		// Token: 0x04002788 RID: 10120
		[Obsolete("This API has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		Annotation,
		// Token: 0x04002789 RID: 10121
		Macro,
		// Token: 0x0400278A RID: 10122
		Nternal,
		// Token: 0x0400278B RID: 10123
		Objmodel,
		// Token: 0x0400278C RID: 10124
		Prefix,
		// Token: 0x0400278D RID: 10125
		Primitive
	}
}
