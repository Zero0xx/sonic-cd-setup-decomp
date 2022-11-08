using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001BA RID: 442
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("97FDCA77-B6F2-4718-A1EB-29D0AECE9C03")]
	[ComImport]
	internal interface ICategoryMembershipEntry
	{
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06001497 RID: 5271
		CategoryMembershipEntry AllData { get; }

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06001498 RID: 5272
		IDefinitionIdentity Identity { get; }

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06001499 RID: 5273
		ISection SubcategoryMembership { get; }
	}
}
