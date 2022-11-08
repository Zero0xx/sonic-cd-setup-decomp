using System;

namespace System.Diagnostics
{
	// Token: 0x02000754 RID: 1876
	[Flags]
	public enum EventLogPermissionAccess
	{
		// Token: 0x040032B3 RID: 12979
		None = 0,
		// Token: 0x040032B4 RID: 12980
		Write = 16,
		// Token: 0x040032B5 RID: 12981
		Administer = 48,
		// Token: 0x040032B6 RID: 12982
		[Obsolete("This member has been deprecated.  Please use System.Diagnostics.EventLogPermissionAccess.Administer instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Browse = 2,
		// Token: 0x040032B7 RID: 12983
		[Obsolete("This member has been deprecated.  Please use System.Diagnostics.EventLogPermissionAccess.Write instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Instrument = 6,
		// Token: 0x040032B8 RID: 12984
		[Obsolete("This member has been deprecated.  Please use System.Diagnostics.EventLogPermissionAccess.Administer instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		Audit = 10
	}
}
