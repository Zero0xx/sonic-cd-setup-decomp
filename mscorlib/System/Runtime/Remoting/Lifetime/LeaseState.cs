using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x0200070D RID: 1805
	[ComVisible(true)]
	[Serializable]
	public enum LeaseState
	{
		// Token: 0x04002062 RID: 8290
		Null,
		// Token: 0x04002063 RID: 8291
		Initial,
		// Token: 0x04002064 RID: 8292
		Active,
		// Token: 0x04002065 RID: 8293
		Renewing,
		// Token: 0x04002066 RID: 8294
		Expired
	}
}
