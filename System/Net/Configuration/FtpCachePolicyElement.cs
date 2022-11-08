using System;
using System.Configuration;
using System.Net.Cache;
using System.Xml;

namespace System.Net.Configuration
{
	// Token: 0x0200064F RID: 1615
	public sealed class FtpCachePolicyElement : ConfigurationElement
	{
		// Token: 0x06003204 RID: 12804 RVA: 0x000D575C File Offset: 0x000D475C
		public FtpCachePolicyElement()
		{
			this.properties.Add(this.policyLevel);
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06003205 RID: 12805 RVA: 0x000D57AC File Offset: 0x000D47AC
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06003206 RID: 12806 RVA: 0x000D57B4 File Offset: 0x000D47B4
		// (set) Token: 0x06003207 RID: 12807 RVA: 0x000D57C7 File Offset: 0x000D47C7
		[ConfigurationProperty("policyLevel", DefaultValue = RequestCacheLevel.Default)]
		public RequestCacheLevel PolicyLevel
		{
			get
			{
				return (RequestCacheLevel)base[this.policyLevel];
			}
			set
			{
				base[this.policyLevel] = value;
			}
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x000D57DB File Offset: 0x000D47DB
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			this.wasReadFromConfig = true;
			base.DeserializeElement(reader, serializeCollectionKey);
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x000D57EC File Offset: 0x000D47EC
		protected override void Reset(ConfigurationElement parentElement)
		{
			if (parentElement != null)
			{
				FtpCachePolicyElement ftpCachePolicyElement = (FtpCachePolicyElement)parentElement;
				this.wasReadFromConfig = ftpCachePolicyElement.wasReadFromConfig;
			}
			base.Reset(parentElement);
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x0600320A RID: 12810 RVA: 0x000D5816 File Offset: 0x000D4816
		internal bool WasReadFromConfig
		{
			get
			{
				return this.wasReadFromConfig;
			}
		}

		// Token: 0x04002EF8 RID: 12024
		private bool wasReadFromConfig;

		// Token: 0x04002EF9 RID: 12025
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002EFA RID: 12026
		private readonly ConfigurationProperty policyLevel = new ConfigurationProperty("policyLevel", typeof(RequestCacheLevel), RequestCacheLevel.Default, ConfigurationPropertyOptions.None);
	}
}
