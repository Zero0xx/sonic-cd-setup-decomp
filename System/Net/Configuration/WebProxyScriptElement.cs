using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000668 RID: 1640
	public sealed class WebProxyScriptElement : ConfigurationElement
	{
		// Token: 0x060032B7 RID: 12983 RVA: 0x000D71C8 File Offset: 0x000D61C8
		public WebProxyScriptElement()
		{
			this.properties.Add(this.downloadTimeout);
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x000D723C File Offset: 0x000D623C
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
				throw new ConfigurationErrorsException(SR.GetString("net_config_element_permission", new object[]
				{
					"webProxyScript"
				}), inner);
			}
		}

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x060032B9 RID: 12985 RVA: 0x000D7298 File Offset: 0x000D6298
		// (set) Token: 0x060032BA RID: 12986 RVA: 0x000D72AB File Offset: 0x000D62AB
		[ConfigurationProperty("downloadTimeout", DefaultValue = "00:01:00")]
		public TimeSpan DownloadTimeout
		{
			get
			{
				return (TimeSpan)base[this.downloadTimeout];
			}
			set
			{
				base[this.downloadTimeout] = value;
			}
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x060032BB RID: 12987 RVA: 0x000D72BF File Offset: 0x000D62BF
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F62 RID: 12130
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F63 RID: 12131
		private readonly ConfigurationProperty downloadTimeout = new ConfigurationProperty("downloadTimeout", typeof(TimeSpan), TimeSpan.FromMinutes(1.0), null, new TimeSpanValidator(new TimeSpan(0, 0, 0), TimeSpan.MaxValue, false), ConfigurationPropertyOptions.None);
	}
}
