using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000654 RID: 1620
	public sealed class NetSectionGroup : ConfigurationSectionGroup
	{
		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06003219 RID: 12825 RVA: 0x000D5952 File Offset: 0x000D4952
		[ConfigurationProperty("authenticationModules")]
		public AuthenticationModulesSection AuthenticationModules
		{
			get
			{
				return (AuthenticationModulesSection)base.Sections["authenticationModules"];
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x0600321A RID: 12826 RVA: 0x000D5969 File Offset: 0x000D4969
		[ConfigurationProperty("connectionManagement")]
		public ConnectionManagementSection ConnectionManagement
		{
			get
			{
				return (ConnectionManagementSection)base.Sections["connectionManagement"];
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x0600321B RID: 12827 RVA: 0x000D5980 File Offset: 0x000D4980
		[ConfigurationProperty("defaultProxy")]
		public DefaultProxySection DefaultProxy
		{
			get
			{
				return (DefaultProxySection)base.Sections["defaultProxy"];
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x0600321C RID: 12828 RVA: 0x000D5997 File Offset: 0x000D4997
		public MailSettingsSectionGroup MailSettings
		{
			get
			{
				return (MailSettingsSectionGroup)base.SectionGroups["mailSettings"];
			}
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x000D59AE File Offset: 0x000D49AE
		public static NetSectionGroup GetSectionGroup(Configuration config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			return config.GetSectionGroup("system.net") as NetSectionGroup;
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x0600321E RID: 12830 RVA: 0x000D59CE File Offset: 0x000D49CE
		[ConfigurationProperty("requestCaching")]
		public RequestCachingSection RequestCaching
		{
			get
			{
				return (RequestCachingSection)base.Sections["requestCaching"];
			}
		}

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x0600321F RID: 12831 RVA: 0x000D59E5 File Offset: 0x000D49E5
		[ConfigurationProperty("settings")]
		public SettingsSection Settings
		{
			get
			{
				return (SettingsSection)base.Sections["settings"];
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x06003220 RID: 12832 RVA: 0x000D59FC File Offset: 0x000D49FC
		[ConfigurationProperty("webRequestModules")]
		public WebRequestModulesSection WebRequestModules
		{
			get
			{
				return (WebRequestModulesSection)base.Sections["webRequestModules"];
			}
		}
	}
}
