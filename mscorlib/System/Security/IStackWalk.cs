using System;
using System.Runtime.InteropServices;

namespace System.Security
{
	// Token: 0x02000625 RID: 1573
	[ComVisible(true)]
	public interface IStackWalk
	{
		// Token: 0x060038A4 RID: 14500
		void Assert();

		// Token: 0x060038A5 RID: 14501
		void Demand();

		// Token: 0x060038A6 RID: 14502
		void Deny();

		// Token: 0x060038A7 RID: 14503
		void PermitOnly();
	}
}
