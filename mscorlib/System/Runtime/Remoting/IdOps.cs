using System;

namespace System.Runtime.Remoting
{
	// Token: 0x020006FE RID: 1790
	internal struct IdOps
	{
		// Token: 0x06003FC0 RID: 16320 RVA: 0x000D9242 File Offset: 0x000D8242
		internal static bool bStrongIdentity(int flags)
		{
			return (flags & 2) != 0;
		}

		// Token: 0x04002038 RID: 8248
		internal const int None = 0;

		// Token: 0x04002039 RID: 8249
		internal const int GenerateURI = 1;

		// Token: 0x0400203A RID: 8250
		internal const int StrongIdentity = 2;
	}
}
