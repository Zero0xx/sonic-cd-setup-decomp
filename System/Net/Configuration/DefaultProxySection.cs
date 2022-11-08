using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200064B RID: 1611
	public sealed class DefaultProxySection : ConfigurationSection
	{
		// Token: 0x060031DD RID: 12765 RVA: 0x000D4D00 File Offset: 0x000D3D00
		public DefaultProxySection()
		{
			this.properties.Add(this.bypasslist);
			this.properties.Add(this.module);
			this.properties.Add(this.proxy);
			this.properties.Add(this.enabled);
			this.properties.Add(this.useDefaultCredentials);
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x000D4E0C File Offset: 0x000D3E0C
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
					"defaultProxy"
				}), inner);
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x060031DF RID: 12767 RVA: 0x000D4E68 File Offset: 0x000D3E68
		[ConfigurationProperty("bypasslist")]
		public BypassElementCollection BypassList
		{
			get
			{
				return (BypassElementCollection)base[this.bypasslist];
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x060031E0 RID: 12768 RVA: 0x000D4E7B File Offset: 0x000D3E7B
		[ConfigurationProperty("module")]
		public ModuleElement Module
		{
			get
			{
				return (ModuleElement)base[this.module];
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x060031E1 RID: 12769 RVA: 0x000D4E8E File Offset: 0x000D3E8E
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x060031E2 RID: 12770 RVA: 0x000D4E96 File Offset: 0x000D3E96
		[ConfigurationProperty("proxy")]
		public ProxyElement Proxy
		{
			get
			{
				return (ProxyElement)base[this.proxy];
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x060031E3 RID: 12771 RVA: 0x000D4EA9 File Offset: 0x000D3EA9
		// (set) Token: 0x060031E4 RID: 12772 RVA: 0x000D4EBC File Offset: 0x000D3EBC
		[ConfigurationProperty("enabled", DefaultValue = true)]
		public bool Enabled
		{
			get
			{
				return (bool)base[this.enabled];
			}
			set
			{
				base[this.enabled] = value;
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x060031E5 RID: 12773 RVA: 0x000D4ED0 File Offset: 0x000D3ED0
		// (set) Token: 0x060031E6 RID: 12774 RVA: 0x000D4EE3 File Offset: 0x000D3EE3
		[ConfigurationProperty("useDefaultCredentials", DefaultValue = false)]
		public bool UseDefaultCredentials
		{
			get
			{
				return (bool)base[this.useDefaultCredentials];
			}
			set
			{
				base[this.useDefaultCredentials] = value;
			}
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x000D4EF8 File Offset: 0x000D3EF8
		protected override void Reset(ConfigurationElement parentElement)
		{
			DefaultProxySection defaultProxySection = new DefaultProxySection();
			defaultProxySection.InitializeDefault();
			base.Reset(defaultProxySection);
		}

		// Token: 0x04002EE5 RID: 12005
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002EE6 RID: 12006
		private readonly ConfigurationProperty bypasslist = new ConfigurationProperty("bypasslist", typeof(BypassElementCollection), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002EE7 RID: 12007
		private readonly ConfigurationProperty module = new ConfigurationProperty("module", typeof(ModuleElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002EE8 RID: 12008
		private readonly ConfigurationProperty proxy = new ConfigurationProperty("proxy", typeof(ProxyElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002EE9 RID: 12009
		private readonly ConfigurationProperty enabled = new ConfigurationProperty("enabled", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04002EEA RID: 12010
		private readonly ConfigurationProperty useDefaultCredentials = new ConfigurationProperty("useDefaultCredentials", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}
