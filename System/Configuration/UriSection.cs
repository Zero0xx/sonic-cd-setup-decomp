using System;

namespace System.Configuration
{
	// Token: 0x0200066F RID: 1647
	public sealed class UriSection : ConfigurationSection
	{
		// Token: 0x060032E3 RID: 13027 RVA: 0x000D7858 File Offset: 0x000D6858
		public UriSection()
		{
			this.properties.Add(this.idn);
			this.properties.Add(this.iriParsing);
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x060032E4 RID: 13028 RVA: 0x000D78D0 File Offset: 0x000D68D0
		[ConfigurationProperty("idn")]
		public IdnElement Idn
		{
			get
			{
				return (IdnElement)base[this.idn];
			}
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x060032E5 RID: 13029 RVA: 0x000D78E3 File Offset: 0x000D68E3
		[ConfigurationProperty("iriParsing")]
		public IriParsingElement IriParsing
		{
			get
			{
				return (IriParsingElement)base[this.iriParsing];
			}
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x060032E6 RID: 13030 RVA: 0x000D78F6 File Offset: 0x000D68F6
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04002F6D RID: 12141
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F6E RID: 12142
		private readonly ConfigurationProperty idn = new ConfigurationProperty("idn", typeof(IdnElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F6F RID: 12143
		private readonly ConfigurationProperty iriParsing = new ConfigurationProperty("iriParsing", typeof(IriParsingElement), null, ConfigurationPropertyOptions.None);
	}
}
