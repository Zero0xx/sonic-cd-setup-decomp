using System;
using System.Runtime.InteropServices;

namespace System.Security
{
	// Token: 0x02000672 RID: 1650
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum HostSecurityManagerOptions
	{
		// Token: 0x04001EB8 RID: 7864
		None = 0,
		// Token: 0x04001EB9 RID: 7865
		HostAppDomainEvidence = 1,
		// Token: 0x04001EBA RID: 7866
		HostPolicyLevel = 2,
		// Token: 0x04001EBB RID: 7867
		HostAssemblyEvidence = 4,
		// Token: 0x04001EBC RID: 7868
		HostDetermineApplicationTrust = 8,
		// Token: 0x04001EBD RID: 7869
		HostResolvePolicy = 16,
		// Token: 0x04001EBE RID: 7870
		AllFlags = 31
	}
}
