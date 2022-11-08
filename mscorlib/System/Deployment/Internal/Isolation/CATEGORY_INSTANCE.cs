using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001F1 RID: 497
	internal struct CATEGORY_INSTANCE
	{
		// Token: 0x0400085E RID: 2142
		public IDefinitionAppId DefinitionAppId_Application;

		// Token: 0x0400085F RID: 2143
		[MarshalAs(UnmanagedType.LPWStr)]
		public string XMLSnippet;
	}
}
