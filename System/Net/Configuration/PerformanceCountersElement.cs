using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000655 RID: 1621
	public sealed class PerformanceCountersElement : ConfigurationElement
	{
		// Token: 0x06003221 RID: 12833 RVA: 0x000D5A14 File Offset: 0x000D4A14
		public PerformanceCountersElement()
		{
			this.properties.Add(this.enabled);
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x06003222 RID: 12834 RVA: 0x000D5A64 File Offset: 0x000D4A64
		// (set) Token: 0x06003223 RID: 12835 RVA: 0x000D5A77 File Offset: 0x000D4A77
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

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x06003224 RID: 12836 RVA: 0x000D5A8B File Offset: 0x000D4A8B
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F00 RID: 12032
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F01 RID: 12033
		private readonly ConfigurationProperty enabled = new ConfigurationProperty("enabled", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}
