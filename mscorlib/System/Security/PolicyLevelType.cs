using System;
using System.Runtime.InteropServices;

namespace System.Security
{
	// Token: 0x02000692 RID: 1682
	[ComVisible(true)]
	[Serializable]
	public enum PolicyLevelType
	{
		// Token: 0x04001F48 RID: 8008
		User,
		// Token: 0x04001F49 RID: 8009
		Machine,
		// Token: 0x04001F4A RID: 8010
		Enterprise,
		// Token: 0x04001F4B RID: 8011
		AppDomain
	}
}
