using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C6 RID: 454
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("C31FF59E-CD25-47b8-9EF3-CF4433EB97CC")]
	[ComImport]
	internal interface IAssemblyReferenceDependentAssemblyEntry
	{
		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060014B1 RID: 5297
		AssemblyReferenceDependentAssemblyEntry AllData { get; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060014B2 RID: 5298
		string Group { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x060014B3 RID: 5299
		string Codebase { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060014B4 RID: 5300
		ulong Size { get; }

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060014B5 RID: 5301
		object HashValue { [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060014B6 RID: 5302
		uint HashAlgorithm { get; }

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060014B7 RID: 5303
		uint Flags { get; }

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060014B8 RID: 5304
		string ResourceFallbackCulture { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060014B9 RID: 5305
		string Description { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060014BA RID: 5306
		string SupportUrl { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060014BB RID: 5307
		ISection HashElements { get; }
	}
}
