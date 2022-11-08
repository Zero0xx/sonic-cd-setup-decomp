using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000650 RID: 1616
	public sealed class Ipv6Element : ConfigurationElement
	{
		// Token: 0x0600320B RID: 12811 RVA: 0x000D5820 File Offset: 0x000D4820
		public Ipv6Element()
		{
			this.properties.Add(this.enabled);
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x0600320C RID: 12812 RVA: 0x000D5870 File Offset: 0x000D4870
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x0600320D RID: 12813 RVA: 0x000D5878 File Offset: 0x000D4878
		// (set) Token: 0x0600320E RID: 12814 RVA: 0x000D588B File Offset: 0x000D488B
		[ConfigurationProperty("enabled", DefaultValue = false)]
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

		// Token: 0x04002EFB RID: 12027
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002EFC RID: 12028
		private readonly ConfigurationProperty enabled = new ConfigurationProperty("enabled", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}
