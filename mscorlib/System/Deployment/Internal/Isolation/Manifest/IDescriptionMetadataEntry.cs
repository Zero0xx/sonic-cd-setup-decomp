using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001DB RID: 475
	[Guid("CB73147E-5FC2-4c31-B4E6-58D13DBE1A08")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IDescriptionMetadataEntry
	{
		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x060014DA RID: 5338
		DescriptionMetadataEntry AllData { get; }

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x060014DB RID: 5339
		string Publisher { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x060014DC RID: 5340
		string Product { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060014DD RID: 5341
		string SupportUrl { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060014DE RID: 5342
		string IconFile { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060014DF RID: 5343
		string ErrorReportUrl { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060014E0 RID: 5344
		string SuiteName { [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
