using System;

namespace System.Configuration
{
	// Token: 0x02000671 RID: 1649
	public sealed class IriParsingElement : ConfigurationElement
	{
		// Token: 0x060032EC RID: 13036 RVA: 0x000D79AC File Offset: 0x000D69AC
		public IriParsingElement()
		{
			this.properties.Add(this.enabled);
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x060032ED RID: 13037 RVA: 0x000D79FC File Offset: 0x000D69FC
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x060032EE RID: 13038 RVA: 0x000D7A04 File Offset: 0x000D6A04
		// (set) Token: 0x060032EF RID: 13039 RVA: 0x000D7A17 File Offset: 0x000D6A17
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

		// Token: 0x04002F73 RID: 12147
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F74 RID: 12148
		private readonly ConfigurationProperty enabled = new ConfigurationProperty("enabled", typeof(bool), false, ConfigurationPropertyOptions.None);
	}
}
