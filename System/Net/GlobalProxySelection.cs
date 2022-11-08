using System;

namespace System.Net
{
	// Token: 0x020003C3 RID: 963
	[Obsolete("This class has been deprecated. Please use WebRequest.DefaultWebProxy instead to access and set the global default proxy. Use 'null' instead of GetEmptyWebProxy. http://go.microsoft.com/fwlink/?linkid=14202")]
	public class GlobalProxySelection
	{
		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001E52 RID: 7762 RVA: 0x000743CC File Offset: 0x000733CC
		// (set) Token: 0x06001E53 RID: 7763 RVA: 0x000743FA File Offset: 0x000733FA
		public static IWebProxy Select
		{
			get
			{
				IWebProxy defaultWebProxy = WebRequest.DefaultWebProxy;
				if (defaultWebProxy == null)
				{
					return GlobalProxySelection.GetEmptyWebProxy();
				}
				WebRequest.WebProxyWrapper webProxyWrapper = defaultWebProxy as WebRequest.WebProxyWrapper;
				if (webProxyWrapper != null)
				{
					return webProxyWrapper.WebProxy;
				}
				return defaultWebProxy;
			}
			set
			{
				WebRequest.DefaultWebProxy = value;
			}
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x00074402 File Offset: 0x00073402
		public static IWebProxy GetEmptyWebProxy()
		{
			return new EmptyWebProxy();
		}
	}
}
