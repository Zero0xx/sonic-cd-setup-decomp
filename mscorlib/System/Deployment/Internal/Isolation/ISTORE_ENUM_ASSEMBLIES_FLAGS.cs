using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000210 RID: 528
	[Flags]
	internal enum ISTORE_ENUM_ASSEMBLIES_FLAGS
	{
		// Token: 0x04000882 RID: 2178
		ISTORE_ENUM_ASSEMBLIES_FLAG_LIMIT_TO_VISIBLE_ONLY = 1,
		// Token: 0x04000883 RID: 2179
		ISTORE_ENUM_ASSEMBLIES_FLAG_MATCH_SERVICING = 2,
		// Token: 0x04000884 RID: 2180
		ISTORE_ENUM_ASSEMBLIES_FLAG_FORCE_LIBRARY_SEMANTICS = 4
	}
}
