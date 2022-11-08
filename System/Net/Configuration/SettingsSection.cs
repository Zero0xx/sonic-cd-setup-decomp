using System;
using System.Collections;
using System.Configuration;
using System.Net.Cache;

namespace System.Net.Configuration
{
	// Token: 0x0200065C RID: 1628
	public sealed class SettingsSection : ConfigurationSection
	{
		// Token: 0x0600324C RID: 12876 RVA: 0x000D6184 File Offset: 0x000D5184
		internal static void EnsureConfigLoaded()
		{
			try
			{
				AuthenticationManager.EnsureConfigLoaded();
				bool isCachingEnabled = RequestCacheManager.IsCachingEnabled;
				int defaultConnectionLimit = System.Net.ServicePointManager.DefaultConnectionLimit;
				bool expect100Continue = System.Net.ServicePointManager.Expect100Continue;
				ArrayList prefixList = WebRequest.PrefixList;
				IWebProxy internalDefaultWebProxy = WebRequest.InternalDefaultWebProxy;
				NetworkingPerfCounters.Initialize();
			}
			catch
			{
			}
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x000D61D0 File Offset: 0x000D51D0
		public SettingsSection()
		{
			this.properties.Add(this.httpWebRequest);
			this.properties.Add(this.ipv6);
			this.properties.Add(this.servicePointManager);
			this.properties.Add(this.socket);
			this.properties.Add(this.webProxyScript);
			this.properties.Add(this.performanceCounters);
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x0600324E RID: 12878 RVA: 0x000D62FC File Offset: 0x000D52FC
		[ConfigurationProperty("httpWebRequest")]
		public HttpWebRequestElement HttpWebRequest
		{
			get
			{
				return (HttpWebRequestElement)base[this.httpWebRequest];
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x0600324F RID: 12879 RVA: 0x000D630F File Offset: 0x000D530F
		[ConfigurationProperty("ipv6")]
		public Ipv6Element Ipv6
		{
			get
			{
				return (Ipv6Element)base[this.ipv6];
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06003250 RID: 12880 RVA: 0x000D6322 File Offset: 0x000D5322
		[ConfigurationProperty("servicePointManager")]
		public ServicePointManagerElement ServicePointManager
		{
			get
			{
				return (ServicePointManagerElement)base[this.servicePointManager];
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x06003251 RID: 12881 RVA: 0x000D6335 File Offset: 0x000D5335
		[ConfigurationProperty("socket")]
		public SocketElement Socket
		{
			get
			{
				return (SocketElement)base[this.socket];
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x06003252 RID: 12882 RVA: 0x000D6348 File Offset: 0x000D5348
		[ConfigurationProperty("webProxyScript")]
		public WebProxyScriptElement WebProxyScript
		{
			get
			{
				return (WebProxyScriptElement)base[this.webProxyScript];
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x06003253 RID: 12883 RVA: 0x000D635B File Offset: 0x000D535B
		[ConfigurationProperty("performanceCounters")]
		public PerformanceCountersElement PerformanceCounters
		{
			get
			{
				return (PerformanceCountersElement)base[this.performanceCounters];
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x06003254 RID: 12884 RVA: 0x000D636E File Offset: 0x000D536E
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F25 RID: 12069
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F26 RID: 12070
		private readonly ConfigurationProperty httpWebRequest = new ConfigurationProperty("httpWebRequest", typeof(HttpWebRequestElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F27 RID: 12071
		private readonly ConfigurationProperty ipv6 = new ConfigurationProperty("ipv6", typeof(Ipv6Element), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F28 RID: 12072
		private readonly ConfigurationProperty servicePointManager = new ConfigurationProperty("servicePointManager", typeof(ServicePointManagerElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F29 RID: 12073
		private readonly ConfigurationProperty socket = new ConfigurationProperty("socket", typeof(SocketElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F2A RID: 12074
		private readonly ConfigurationProperty webProxyScript = new ConfigurationProperty("webProxyScript", typeof(WebProxyScriptElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F2B RID: 12075
		private readonly ConfigurationProperty performanceCounters = new ConfigurationProperty("performanceCounters", typeof(PerformanceCountersElement), null, ConfigurationPropertyOptions.None);
	}
}
