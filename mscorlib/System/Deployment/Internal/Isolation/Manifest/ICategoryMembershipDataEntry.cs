using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B4 RID: 436
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("DA0C3B27-6B6B-4b80-A8F8-6CE14F4BC0A4")]
	[ComImport]
	internal interface ICategoryMembershipDataEntry
	{
		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600148E RID: 5262
		CategoryMembershipDataEntry AllData { get; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x0600148F RID: 5263
		uint index { get; }

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06001490 RID: 5264
		string Xml { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06001491 RID: 5265
		string Description { [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
