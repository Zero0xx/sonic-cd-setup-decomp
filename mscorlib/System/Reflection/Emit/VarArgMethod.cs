using System;

namespace System.Reflection.Emit
{
	// Token: 0x0200081F RID: 2079
	internal class VarArgMethod
	{
		// Token: 0x060049D0 RID: 18896 RVA: 0x00100E9A File Offset: 0x000FFE9A
		internal VarArgMethod(MethodInfo method, SignatureHelper signature)
		{
			this.m_method = method;
			this.m_signature = signature;
		}

		// Token: 0x040025C9 RID: 9673
		internal MethodInfo m_method;

		// Token: 0x040025CA RID: 9674
		internal SignatureHelper m_signature;
	}
}
