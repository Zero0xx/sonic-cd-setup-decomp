using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200063A RID: 1594
	[ComVisible(true)]
	[Serializable]
	public enum SecurityAction
	{
		// Token: 0x04001DEF RID: 7663
		Demand = 2,
		// Token: 0x04001DF0 RID: 7664
		Assert,
		// Token: 0x04001DF1 RID: 7665
		Deny,
		// Token: 0x04001DF2 RID: 7666
		PermitOnly,
		// Token: 0x04001DF3 RID: 7667
		LinkDemand,
		// Token: 0x04001DF4 RID: 7668
		InheritanceDemand,
		// Token: 0x04001DF5 RID: 7669
		RequestMinimum,
		// Token: 0x04001DF6 RID: 7670
		RequestOptional,
		// Token: 0x04001DF7 RID: 7671
		RequestRefuse
	}
}
