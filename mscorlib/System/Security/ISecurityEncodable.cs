using System;
using System.Runtime.InteropServices;

namespace System.Security
{
	// Token: 0x0200048E RID: 1166
	[ComVisible(true)]
	public interface ISecurityEncodable
	{
		// Token: 0x06002E5C RID: 11868
		SecurityElement ToXml();

		// Token: 0x06002E5D RID: 11869
		void FromXml(SecurityElement e);
	}
}
