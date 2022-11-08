using System;
using System.Configuration;
using System.Net.Cache;
using System.Xml;

namespace System.Net.Configuration
{
	// Token: 0x0200064E RID: 1614
	public sealed class HttpCachePolicyElement : ConfigurationElement
	{
		// Token: 0x060031F7 RID: 12791 RVA: 0x000D5580 File Offset: 0x000D4580
		public HttpCachePolicyElement()
		{
			this.properties.Add(this.maximumAge);
			this.properties.Add(this.maximumStale);
			this.properties.Add(this.minimumFresh);
			this.properties.Add(this.policyLevel);
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x060031F8 RID: 12792 RVA: 0x000D5672 File Offset: 0x000D4672
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x060031F9 RID: 12793 RVA: 0x000D567A File Offset: 0x000D467A
		// (set) Token: 0x060031FA RID: 12794 RVA: 0x000D568D File Offset: 0x000D468D
		[ConfigurationProperty("maximumAge", DefaultValue = "10675199.02:48:05.4775807")]
		public TimeSpan MaximumAge
		{
			get
			{
				return (TimeSpan)base[this.maximumAge];
			}
			set
			{
				base[this.maximumAge] = value;
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x060031FB RID: 12795 RVA: 0x000D56A1 File Offset: 0x000D46A1
		// (set) Token: 0x060031FC RID: 12796 RVA: 0x000D56B4 File Offset: 0x000D46B4
		[ConfigurationProperty("maximumStale", DefaultValue = "-10675199.02:48:05.4775808")]
		public TimeSpan MaximumStale
		{
			get
			{
				return (TimeSpan)base[this.maximumStale];
			}
			set
			{
				base[this.maximumStale] = value;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x060031FD RID: 12797 RVA: 0x000D56C8 File Offset: 0x000D46C8
		// (set) Token: 0x060031FE RID: 12798 RVA: 0x000D56DB File Offset: 0x000D46DB
		[ConfigurationProperty("minimumFresh", DefaultValue = "-10675199.02:48:05.4775808")]
		public TimeSpan MinimumFresh
		{
			get
			{
				return (TimeSpan)base[this.minimumFresh];
			}
			set
			{
				base[this.minimumFresh] = value;
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x060031FF RID: 12799 RVA: 0x000D56EF File Offset: 0x000D46EF
		// (set) Token: 0x06003200 RID: 12800 RVA: 0x000D5702 File Offset: 0x000D4702
		[ConfigurationProperty("policyLevel", IsRequired = true, DefaultValue = HttpRequestCacheLevel.Default)]
		public HttpRequestCacheLevel PolicyLevel
		{
			get
			{
				return (HttpRequestCacheLevel)base[this.policyLevel];
			}
			set
			{
				base[this.policyLevel] = value;
			}
		}

		// Token: 0x06003201 RID: 12801 RVA: 0x000D5716 File Offset: 0x000D4716
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			this.wasReadFromConfig = true;
			base.DeserializeElement(reader, serializeCollectionKey);
		}

		// Token: 0x06003202 RID: 12802 RVA: 0x000D5728 File Offset: 0x000D4728
		protected override void Reset(ConfigurationElement parentElement)
		{
			if (parentElement != null)
			{
				HttpCachePolicyElement httpCachePolicyElement = (HttpCachePolicyElement)parentElement;
				this.wasReadFromConfig = httpCachePolicyElement.wasReadFromConfig;
			}
			base.Reset(parentElement);
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06003203 RID: 12803 RVA: 0x000D5752 File Offset: 0x000D4752
		internal bool WasReadFromConfig
		{
			get
			{
				return this.wasReadFromConfig;
			}
		}

		// Token: 0x04002EF2 RID: 12018
		private bool wasReadFromConfig;

		// Token: 0x04002EF3 RID: 12019
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002EF4 RID: 12020
		private readonly ConfigurationProperty maximumAge = new ConfigurationProperty("maximumAge", typeof(TimeSpan), TimeSpan.MaxValue, ConfigurationPropertyOptions.None);

		// Token: 0x04002EF5 RID: 12021
		private readonly ConfigurationProperty maximumStale = new ConfigurationProperty("maximumStale", typeof(TimeSpan), TimeSpan.MinValue, ConfigurationPropertyOptions.None);

		// Token: 0x04002EF6 RID: 12022
		private readonly ConfigurationProperty minimumFresh = new ConfigurationProperty("minimumFresh", typeof(TimeSpan), TimeSpan.MinValue, ConfigurationPropertyOptions.None);

		// Token: 0x04002EF7 RID: 12023
		private readonly ConfigurationProperty policyLevel = new ConfigurationProperty("policyLevel", typeof(HttpRequestCacheLevel), HttpRequestCacheLevel.Default, ConfigurationPropertyOptions.None);
	}
}
