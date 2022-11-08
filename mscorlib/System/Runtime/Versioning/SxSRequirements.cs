using System;

namespace System.Runtime.Versioning
{
	// Token: 0x0200091D RID: 2333
	[Flags]
	internal enum SxSRequirements
	{
		// Token: 0x04002C0E RID: 11278
		None = 0,
		// Token: 0x04002C0F RID: 11279
		AppDomainID = 1,
		// Token: 0x04002C10 RID: 11280
		ProcessID = 2,
		// Token: 0x04002C11 RID: 11281
		AssemblyName = 4,
		// Token: 0x04002C12 RID: 11282
		TypeName = 8
	}
}
