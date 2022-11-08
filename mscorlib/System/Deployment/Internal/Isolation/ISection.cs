using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000188 RID: 392
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("285a8862-c84a-11d7-850f-005cd062464f")]
	[ComImport]
	internal interface ISection
	{
		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06001429 RID: 5161
		object _NewEnum { [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600142A RID: 5162
		uint Count { get; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x0600142B RID: 5163
		uint SectionID { get; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x0600142C RID: 5164
		string SectionName { [return: MarshalAs(UnmanagedType.LPWStr)] get; }
	}
}
