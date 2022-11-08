using System;

namespace System.Diagnostics
{
	// Token: 0x0200076D RID: 1901
	[Flags]
	public enum PerformanceCounterPermissionAccess
	{
		// Token: 0x0400333F RID: 13119
		[Obsolete("This member has been deprecated.  Use System.Diagnostics.PerformanceCounter.PerformanceCounterPermissionAccess.Read instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Browse = 1,
		// Token: 0x04003340 RID: 13120
		[Obsolete("This member has been deprecated.  Use System.Diagnostics.PerformanceCounter.PerformanceCounterPermissionAccess.Write instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Instrument = 3,
		// Token: 0x04003341 RID: 13121
		None = 0,
		// Token: 0x04003342 RID: 13122
		Read = 1,
		// Token: 0x04003343 RID: 13123
		Write = 2,
		// Token: 0x04003344 RID: 13124
		Administer = 7
	}
}
