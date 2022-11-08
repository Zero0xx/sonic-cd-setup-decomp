using System;
using System.Runtime.InteropServices;

namespace System.Security
{
	// Token: 0x02000624 RID: 1572
	[ComVisible(true)]
	public interface IPermission : ISecurityEncodable
	{
		// Token: 0x0600389F RID: 14495
		IPermission Copy();

		// Token: 0x060038A0 RID: 14496
		IPermission Intersect(IPermission target);

		// Token: 0x060038A1 RID: 14497
		IPermission Union(IPermission target);

		// Token: 0x060038A2 RID: 14498
		bool IsSubsetOf(IPermission target);

		// Token: 0x060038A3 RID: 14499
		void Demand();
	}
}
