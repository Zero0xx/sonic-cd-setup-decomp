using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001EE RID: 494
	internal struct STORE_CATEGORY_INSTANCE
	{
		// Token: 0x0400085A RID: 2138
		public IDefinitionAppId DefinitionAppId_Application;

		// Token: 0x0400085B RID: 2139
		[MarshalAs(UnmanagedType.LPWStr)]
		public string XMLSnippet;
	}
}
