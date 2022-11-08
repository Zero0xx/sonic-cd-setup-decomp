using System;

namespace System.Net
{
	// Token: 0x02000505 RID: 1285
	internal class ProxyScriptChain : ProxyChain
	{
		// Token: 0x060027EE RID: 10222 RVA: 0x000A4BC2 File Offset: 0x000A3BC2
		internal ProxyScriptChain(WebProxy proxy, Uri destination) : base(destination)
		{
			this.m_Proxy = proxy;
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x000A4BD4 File Offset: 0x000A3BD4
		protected override bool GetNextProxy(out Uri proxy)
		{
			if (this.m_CurrentIndex < 0)
			{
				proxy = null;
				return false;
			}
			if (this.m_CurrentIndex == 0)
			{
				this.m_ScriptProxies = this.m_Proxy.GetProxiesAuto(base.Destination, ref this.m_SyncStatus);
			}
			if (this.m_ScriptProxies == null || this.m_CurrentIndex >= this.m_ScriptProxies.Length)
			{
				proxy = this.m_Proxy.GetProxyAutoFailover(base.Destination);
				this.m_CurrentIndex = -1;
				return true;
			}
			proxy = this.m_ScriptProxies[this.m_CurrentIndex++];
			return true;
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x000A4C63 File Offset: 0x000A3C63
		internal override void Abort()
		{
			this.m_Proxy.AbortGetProxiesAuto(ref this.m_SyncStatus);
		}

		// Token: 0x0400273F RID: 10047
		private WebProxy m_Proxy;

		// Token: 0x04002740 RID: 10048
		private Uri[] m_ScriptProxies;

		// Token: 0x04002741 RID: 10049
		private int m_CurrentIndex;

		// Token: 0x04002742 RID: 10050
		private int m_SyncStatus;
	}
}
