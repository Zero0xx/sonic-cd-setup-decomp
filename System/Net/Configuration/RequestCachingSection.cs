using System;
using System.Configuration;
using System.Net.Cache;
using System.Xml;

namespace System.Net.Configuration
{
	// Token: 0x0200065A RID: 1626
	public sealed class RequestCachingSection : ConfigurationSection
	{
		// Token: 0x06003231 RID: 12849 RVA: 0x000D5CA4 File Offset: 0x000D4CA4
		public RequestCachingSection()
		{
			this.properties.Add(this.disableAllCaching);
			this.properties.Add(this.defaultPolicyLevel);
			this.properties.Add(this.isPrivateCache);
			this.properties.Add(this.defaultHttpCachePolicy);
			this.properties.Add(this.defaultFtpCachePolicy);
			this.properties.Add(this.unspecifiedMaximumAge);
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06003232 RID: 12850 RVA: 0x000D5DF1 File Offset: 0x000D4DF1
		[ConfigurationProperty("defaultHttpCachePolicy")]
		public HttpCachePolicyElement DefaultHttpCachePolicy
		{
			get
			{
				return (HttpCachePolicyElement)base[this.defaultHttpCachePolicy];
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06003233 RID: 12851 RVA: 0x000D5E04 File Offset: 0x000D4E04
		[ConfigurationProperty("defaultFtpCachePolicy")]
		public FtpCachePolicyElement DefaultFtpCachePolicy
		{
			get
			{
				return (FtpCachePolicyElement)base[this.defaultFtpCachePolicy];
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06003234 RID: 12852 RVA: 0x000D5E17 File Offset: 0x000D4E17
		// (set) Token: 0x06003235 RID: 12853 RVA: 0x000D5E2A File Offset: 0x000D4E2A
		[ConfigurationProperty("defaultPolicyLevel", DefaultValue = RequestCacheLevel.BypassCache)]
		public RequestCacheLevel DefaultPolicyLevel
		{
			get
			{
				return (RequestCacheLevel)base[this.defaultPolicyLevel];
			}
			set
			{
				base[this.defaultPolicyLevel] = value;
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06003236 RID: 12854 RVA: 0x000D5E3E File Offset: 0x000D4E3E
		// (set) Token: 0x06003237 RID: 12855 RVA: 0x000D5E51 File Offset: 0x000D4E51
		[ConfigurationProperty("disableAllCaching", DefaultValue = false)]
		public bool DisableAllCaching
		{
			get
			{
				return (bool)base[this.disableAllCaching];
			}
			set
			{
				base[this.disableAllCaching] = value;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06003238 RID: 12856 RVA: 0x000D5E65 File Offset: 0x000D4E65
		// (set) Token: 0x06003239 RID: 12857 RVA: 0x000D5E78 File Offset: 0x000D4E78
		[ConfigurationProperty("isPrivateCache", DefaultValue = true)]
		public bool IsPrivateCache
		{
			get
			{
				return (bool)base[this.isPrivateCache];
			}
			set
			{
				base[this.isPrivateCache] = value;
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x0600323A RID: 12858 RVA: 0x000D5E8C File Offset: 0x000D4E8C
		// (set) Token: 0x0600323B RID: 12859 RVA: 0x000D5E9F File Offset: 0x000D4E9F
		[ConfigurationProperty("unspecifiedMaximumAge", DefaultValue = "1.00:00:00")]
		public TimeSpan UnspecifiedMaximumAge
		{
			get
			{
				return (TimeSpan)base[this.unspecifiedMaximumAge];
			}
			set
			{
				base[this.unspecifiedMaximumAge] = value;
			}
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x000D5EB4 File Offset: 0x000D4EB4
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			bool flag = this.DisableAllCaching;
			base.DeserializeElement(reader, serializeCollectionKey);
			if (flag)
			{
				this.DisableAllCaching = true;
			}
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x000D5EDC File Offset: 0x000D4EDC
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			try
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
			}
			catch (Exception inner)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_config_section_permission", new object[]
				{
					"requestCaching"
				}), inner);
			}
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x0600323E RID: 12862 RVA: 0x000D5F38 File Offset: 0x000D4F38
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F14 RID: 12052
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F15 RID: 12053
		private readonly ConfigurationProperty defaultHttpCachePolicy = new ConfigurationProperty("defaultHttpCachePolicy", typeof(HttpCachePolicyElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F16 RID: 12054
		private readonly ConfigurationProperty defaultFtpCachePolicy = new ConfigurationProperty("defaultFtpCachePolicy", typeof(FtpCachePolicyElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F17 RID: 12055
		private readonly ConfigurationProperty defaultPolicyLevel = new ConfigurationProperty("defaultPolicyLevel", typeof(RequestCacheLevel), RequestCacheLevel.BypassCache, ConfigurationPropertyOptions.None);

		// Token: 0x04002F18 RID: 12056
		private readonly ConfigurationProperty disableAllCaching = new ConfigurationProperty("disableAllCaching", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04002F19 RID: 12057
		private readonly ConfigurationProperty isPrivateCache = new ConfigurationProperty("isPrivateCache", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04002F1A RID: 12058
		private readonly ConfigurationProperty unspecifiedMaximumAge = new ConfigurationProperty("unspecifiedMaximumAge", typeof(TimeSpan), TimeSpan.FromDays(1.0), ConfigurationPropertyOptions.None);
	}
}
