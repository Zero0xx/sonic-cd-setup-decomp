using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B1 RID: 433
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0C66F299-E08E-48c5-9264-7CCBEB4D5CBB")]
	[ComImport]
	internal interface IFileAssociationEntry
	{
		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06001487 RID: 5255
		FileAssociationEntry AllData { get; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06001488 RID: 5256
		string Extension { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06001489 RID: 5257
		string Description { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x0600148A RID: 5258
		string ProgID { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x0600148B RID: 5259
		string DefaultIcon { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600148C RID: 5260
		string Parameter { [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
