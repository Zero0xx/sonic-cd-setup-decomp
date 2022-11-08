using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000649 RID: 1609
	public sealed class ConnectionManagementSection : ConfigurationSection
	{
		// Token: 0x060031D6 RID: 12758 RVA: 0x000D4B71 File Offset: 0x000D3B71
		public ConnectionManagementSection()
		{
			this.properties.Add(this.connectionManagement);
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x060031D7 RID: 12759 RVA: 0x000D4BAD File Offset: 0x000D3BAD
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public ConnectionManagementElementCollection ConnectionManagement
		{
			get
			{
				return (ConnectionManagementElementCollection)base[this.connectionManagement];
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x060031D8 RID: 12760 RVA: 0x000D4BC0 File Offset: 0x000D3BC0
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002EE1 RID: 12001
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002EE2 RID: 12002
		private readonly ConfigurationProperty connectionManagement = new ConfigurationProperty(null, typeof(ConnectionManagementElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}
