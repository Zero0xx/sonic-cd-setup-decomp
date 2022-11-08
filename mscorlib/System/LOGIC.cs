using System;

namespace System
{
	// Token: 0x02000107 RID: 263
	internal static class LOGIC
	{
		// Token: 0x06000EDC RID: 3804 RVA: 0x0002C4E3 File Offset: 0x0002B4E3
		internal static bool IMPLIES(bool p, bool q)
		{
			return !p || q;
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0002C4EB File Offset: 0x0002B4EB
		internal static bool BIJECTION(bool p, bool q)
		{
			return LOGIC.IMPLIES(p, q) && LOGIC.IMPLIES(q, p);
		}
	}
}
