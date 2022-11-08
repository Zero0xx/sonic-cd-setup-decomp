using System;

namespace System.Runtime.Remoting.Proxies
{
	// Token: 0x0200079F RID: 1951
	internal sealed class __TransparentProxy
	{
		// Token: 0x06004583 RID: 17795 RVA: 0x000EC818 File Offset: 0x000EB818
		private __TransparentProxy()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Constructor"));
		}

		// Token: 0x04002290 RID: 8848
		private RealProxy _rp;

		// Token: 0x04002291 RID: 8849
		private object _stubData;

		// Token: 0x04002292 RID: 8850
		private IntPtr _pMT;

		// Token: 0x04002293 RID: 8851
		private IntPtr _pInterfaceMT;

		// Token: 0x04002294 RID: 8852
		private IntPtr _stub;
	}
}
