using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000544 RID: 1348
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.FILETIME instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	public struct FILETIME
	{
		// Token: 0x04001A4C RID: 6732
		public int dwLowDateTime;

		// Token: 0x04001A4D RID: 6733
		public int dwHighDateTime;
	}
}
