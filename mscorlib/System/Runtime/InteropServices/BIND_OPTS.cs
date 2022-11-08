using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000536 RID: 1334
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.BIND_OPTS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	public struct BIND_OPTS
	{
		// Token: 0x04001A46 RID: 6726
		public int cbStruct;

		// Token: 0x04001A47 RID: 6727
		public int grfFlags;

		// Token: 0x04001A48 RID: 6728
		public int grfMode;

		// Token: 0x04001A49 RID: 6729
		public int dwTickCountDeadline;
	}
}
