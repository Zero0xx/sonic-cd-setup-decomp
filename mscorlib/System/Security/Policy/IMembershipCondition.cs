using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x02000490 RID: 1168
	[ComVisible(true)]
	public interface IMembershipCondition : ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06002E60 RID: 11872
		bool Check(Evidence evidence);

		// Token: 0x06002E61 RID: 11873
		IMembershipCondition Copy();

		// Token: 0x06002E62 RID: 11874
		string ToString();

		// Token: 0x06002E63 RID: 11875
		bool Equals(object obj);
	}
}
