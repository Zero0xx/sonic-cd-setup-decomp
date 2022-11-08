using System;

namespace System.Reflection.Emit
{
	// Token: 0x0200081E RID: 2078
	internal class GenericFieldInfo
	{
		// Token: 0x060049CF RID: 18895 RVA: 0x00100E84 File Offset: 0x000FFE84
		internal GenericFieldInfo(RuntimeFieldHandle field, RuntimeTypeHandle context)
		{
			this.m_field = field;
			this.m_context = context;
		}

		// Token: 0x040025C7 RID: 9671
		internal RuntimeFieldHandle m_field;

		// Token: 0x040025C8 RID: 9672
		internal RuntimeTypeHandle m_context;
	}
}
