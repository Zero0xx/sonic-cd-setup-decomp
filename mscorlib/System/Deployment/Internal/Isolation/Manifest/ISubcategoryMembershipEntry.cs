using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B7 RID: 439
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("5A7A54D7-5AD5-418e-AB7A-CF823A8D48D0")]
	[ComImport]
	internal interface ISubcategoryMembershipEntry
	{
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06001493 RID: 5267
		SubcategoryMembershipEntry AllData { get; }

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06001494 RID: 5268
		string Subcategory { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06001495 RID: 5269
		ISection CategoryMembershipData { get; }
	}
}
