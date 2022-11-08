using System;

namespace System.Net
{
	// Token: 0x02000507 RID: 1287
	internal class StaticProxy : ProxyChain
	{
		// Token: 0x060027F3 RID: 10227 RVA: 0x000A4C96 File Offset: 0x000A3C96
		internal StaticProxy(Uri destination, Uri proxy) : base(destination)
		{
			if (proxy == null)
			{
				throw new ArgumentNullException("proxy");
			}
			this.m_Proxy = proxy;
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x000A4CBA File Offset: 0x000A3CBA
		protected override bool GetNextProxy(out Uri proxy)
		{
			proxy = this.m_Proxy;
			if (proxy == null)
			{
				return false;
			}
			this.m_Proxy = null;
			return true;
		}

		// Token: 0x04002744 RID: 10052
		private Uri m_Proxy;
	}
}
