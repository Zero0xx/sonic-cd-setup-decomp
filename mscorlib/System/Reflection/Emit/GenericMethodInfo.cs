using System;

namespace System.Reflection.Emit
{
	// Token: 0x0200081D RID: 2077
	internal class GenericMethodInfo
	{
		// Token: 0x060049CE RID: 18894 RVA: 0x00100E6E File Offset: 0x000FFE6E
		internal GenericMethodInfo(RuntimeMethodHandle method, RuntimeTypeHandle context)
		{
			this.m_method = method;
			this.m_context = context;
		}

		// Token: 0x040025C5 RID: 9669
		internal RuntimeMethodHandle m_method;

		// Token: 0x040025C6 RID: 9670
		internal RuntimeTypeHandle m_context;
	}
}
