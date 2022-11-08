using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C3 RID: 451
	[Guid("1E0422A1-F0D2-44ae-914B-8A2DECCFD22B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICLRSurrogateEntry
	{
		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060014A9 RID: 5289
		CLRSurrogateEntry AllData { get; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060014AA RID: 5290
		Guid Clsid { get; }

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060014AB RID: 5291
		string RuntimeVersion { [return: MarshalAs(UnmanagedType.LPWStr)] get; }

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060014AC RID: 5292
		string ClassName { [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
