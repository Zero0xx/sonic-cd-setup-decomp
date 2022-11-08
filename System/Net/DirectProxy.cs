using System;

namespace System.Net
{
	// Token: 0x02000506 RID: 1286
	internal class DirectProxy : ProxyChain
	{
		// Token: 0x060027F1 RID: 10225 RVA: 0x000A4C76 File Offset: 0x000A3C76
		internal DirectProxy(Uri destination) : base(destination)
		{
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000A4C7F File Offset: 0x000A3C7F
		protected override bool GetNextProxy(out Uri proxy)
		{
			proxy = null;
			if (this.m_ProxyRetrieved)
			{
				return false;
			}
			this.m_ProxyRetrieved = true;
			return true;
		}

		// Token: 0x04002743 RID: 10051
		private bool m_ProxyRetrieved;
	}
}
