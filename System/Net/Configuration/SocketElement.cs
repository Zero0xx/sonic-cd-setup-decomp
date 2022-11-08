using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000666 RID: 1638
	public sealed class SocketElement : ConfigurationElement
	{
		// Token: 0x060032AD RID: 12973 RVA: 0x000D7028 File Offset: 0x000D6028
		public SocketElement()
		{
			this.properties.Add(this.alwaysUseCompletionPortsForAccept);
			this.properties.Add(this.alwaysUseCompletionPortsForConnect);
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x000D70AC File Offset: 0x000D60AC
		protected override void PostDeserialize()
		{
			if (base.EvaluationContext.IsMachineLevel)
			{
				return;
			}
			try
			{
				ExceptionHelper.UnrestrictedSocketPermission.Demand();
			}
			catch (Exception inner)
			{
				throw new ConfigurationErrorsException(SR.GetString("net_config_element_permission", new object[]
				{
					"socket"
				}), inner);
			}
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x060032AF RID: 12975 RVA: 0x000D7108 File Offset: 0x000D6108
		// (set) Token: 0x060032B0 RID: 12976 RVA: 0x000D711B File Offset: 0x000D611B
		[ConfigurationProperty("alwaysUseCompletionPortsForAccept", DefaultValue = false)]
		public bool AlwaysUseCompletionPortsForAccept
		{
			get
			{
				return (bool)base[this.alwaysUseCompletionPortsForAccept];
			}
			set
			{
				base[this.alwaysUseCompletionPortsForAccept] = value;
			}
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x060032B1 RID: 12977 RVA: 0x000D712F File Offset: 0x000D612F
		// (set) Token: 0x060032B2 RID: 12978 RVA: 0x000D7142 File Offset: 0x000D6142
		[ConfigurationProperty("alwaysUseCompletionPortsForConnect", DefaultValue = false)]
		public bool AlwaysUseCompletionPortsForConnect
		{
			get
			{
				return (bool)base[this.alwaysUseCompletionPortsForConnect];
			}
			set
			{
				base[this.alwaysUseCompletionPortsForConnect] = value;
			}
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x060032B3 RID: 12979 RVA: 0x000D7156 File Offset: 0x000D6156
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F5E RID: 12126
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F5F RID: 12127
		private readonly ConfigurationProperty alwaysUseCompletionPortsForConnect = new ConfigurationProperty("alwaysUseCompletionPortsForConnect", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04002F60 RID: 12128
		private readonly ConfigurationProperty alwaysUseCompletionPortsForAccept = new ConfigurationProperty("alwaysUseCompletionPortsForAccept", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}
