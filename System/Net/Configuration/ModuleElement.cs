using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000653 RID: 1619
	public sealed class ModuleElement : ConfigurationElement
	{
		// Token: 0x06003214 RID: 12820 RVA: 0x000D58E0 File Offset: 0x000D48E0
		public ModuleElement()
		{
			this.properties.Add(this.type);
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x06003215 RID: 12821 RVA: 0x000D5920 File Offset: 0x000D4920
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06003216 RID: 12822 RVA: 0x000D5928 File Offset: 0x000D4928
		// (set) Token: 0x06003217 RID: 12823 RVA: 0x000D593B File Offset: 0x000D493B
		[ConfigurationProperty("type")]
		public string Type
		{
			get
			{
				return (string)base[this.type];
			}
			set
			{
				base[this.type] = value;
			}
		}

		// Token: 0x04002EFE RID: 12030
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002EFF RID: 12031
		private readonly ConfigurationProperty type = new ConfigurationProperty("type", typeof(string), null, ConfigurationPropertyOptions.None);
	}
}
