using System;

namespace System.Globalization
{
	// Token: 0x020003B4 RID: 948
	internal struct InternalCodePageDataItem
	{
		// Token: 0x04001122 RID: 4386
		internal int codePage;

		// Token: 0x04001123 RID: 4387
		internal int uiFamilyCodePage;

		// Token: 0x04001124 RID: 4388
		internal unsafe char* webName;

		// Token: 0x04001125 RID: 4389
		internal unsafe char* headerName;

		// Token: 0x04001126 RID: 4390
		internal unsafe char* bodyName;

		// Token: 0x04001127 RID: 4391
		internal uint flags;
	}
}
