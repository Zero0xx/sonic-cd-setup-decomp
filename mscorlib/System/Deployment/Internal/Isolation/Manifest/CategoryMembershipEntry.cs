using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B8 RID: 440
	[StructLayout(LayoutKind.Sequential)]
	internal class CategoryMembershipEntry
	{
		// Token: 0x0400079B RID: 1947
		public IDefinitionIdentity Identity;

		// Token: 0x0400079C RID: 1948
		public ISection SubcategoryMembership;
	}
}
