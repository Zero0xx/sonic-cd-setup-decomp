using System;

namespace System.Net
{
	// Token: 0x020004D7 RID: 1239
	[Serializable]
	internal sealed class EmptyWebProxy : IAutoWebProxy, IWebProxy
	{
		// Token: 0x06002689 RID: 9865 RVA: 0x0009DB82 File Offset: 0x0009CB82
		public Uri GetProxy(Uri uri)
		{
			return uri;
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x0009DB85 File Offset: 0x0009CB85
		public bool IsBypassed(Uri uri)
		{
			return true;
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x0600268B RID: 9867 RVA: 0x0009DB88 File Offset: 0x0009CB88
		// (set) Token: 0x0600268C RID: 9868 RVA: 0x0009DB90 File Offset: 0x0009CB90
		public ICredentials Credentials
		{
			get
			{
				return this.m_credentials;
			}
			set
			{
				this.m_credentials = value;
			}
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x0009DB99 File Offset: 0x0009CB99
		ProxyChain IAutoWebProxy.GetProxies(Uri destination)
		{
			return new DirectProxy(destination);
		}

		// Token: 0x04002628 RID: 9768
		[NonSerialized]
		private ICredentials m_credentials;
	}
}
