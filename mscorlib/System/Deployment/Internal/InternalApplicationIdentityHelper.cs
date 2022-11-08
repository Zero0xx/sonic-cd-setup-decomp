using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal
{
	// Token: 0x0200006A RID: 106
	[ComVisible(false)]
	public static class InternalApplicationIdentityHelper
	{
		// Token: 0x06000635 RID: 1589 RVA: 0x00015622 File Offset: 0x00014622
		public static object GetInternalAppId(ApplicationIdentity id)
		{
			return id.Identity;
		}
	}
}
