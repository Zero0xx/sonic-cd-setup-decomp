using System;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace System.Security
{
	// Token: 0x0200048F RID: 1167
	[ComVisible(true)]
	public interface ISecurityPolicyEncodable
	{
		// Token: 0x06002E5E RID: 11870
		SecurityElement ToXml(PolicyLevel level);

		// Token: 0x06002E5F RID: 11871
		void FromXml(SecurityElement e, PolicyLevel level);
	}
}
