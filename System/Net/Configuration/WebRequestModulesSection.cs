using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200066D RID: 1645
	public sealed class WebRequestModulesSection : ConfigurationSection
	{
		// Token: 0x060032DA RID: 13018 RVA: 0x000D7589 File Offset: 0x000D6589
		public WebRequestModulesSection()
		{
			this.properties.Add(this.webRequestModules);
		}

		// Token: 0x060032DB RID: 13019 RVA: 0x000D75C8 File Offset: 0x000D65C8
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
					"webRequestModules"
				}), inner);
			}
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x000D7624 File Offset: 0x000D6624
		protected override void InitializeDefault()
		{
			this.WebRequestModules.Add(new WebRequestModuleElement("https:", typeof(HttpRequestCreator)));
			this.WebRequestModules.Add(new WebRequestModuleElement("http:", typeof(HttpRequestCreator)));
			this.WebRequestModules.Add(new WebRequestModuleElement("file:", typeof(FileWebRequestCreator)));
			this.WebRequestModules.Add(new WebRequestModuleElement("ftp:", typeof(FtpWebRequestCreator)));
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x060032DD RID: 13021 RVA: 0x000D76AD File Offset: 0x000D66AD
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x060032DE RID: 13022 RVA: 0x000D76B5 File Offset: 0x000D66B5
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public WebRequestModuleElementCollection WebRequestModules
		{
			get
			{
				return (WebRequestModuleElementCollection)base[this.webRequestModules];
			}
		}

		// Token: 0x04002F69 RID: 12137
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F6A RID: 12138
		private readonly ConfigurationProperty webRequestModules = new ConfigurationProperty(null, typeof(WebRequestModuleElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}
