using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200018B RID: 395
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("285a8861-c84a-11d7-850f-005cd062464f")]
	[ComImport]
	internal interface ISectionEntry
	{
		// Token: 0x06001430 RID: 5168
		object GetField(uint fieldId);

		// Token: 0x06001431 RID: 5169
		string GetFieldName(uint fieldId);
	}
}
