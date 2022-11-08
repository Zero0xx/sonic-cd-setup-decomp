using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200018D RID: 397
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("285a8860-c84a-11d7-850f-005cd062464f")]
	[ComImport]
	internal interface ICDF
	{
		// Token: 0x06001436 RID: 5174
		ISection GetRootSection(uint SectionId);

		// Token: 0x06001437 RID: 5175
		ISectionEntry GetRootSectionEntry(uint SectionId);

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06001438 RID: 5176
		object _NewEnum { [return: MarshalAs(UnmanagedType.Interface)] get; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06001439 RID: 5177
		uint Count { get; }

		// Token: 0x0600143A RID: 5178
		object GetItem(uint SectionId);
	}
}
