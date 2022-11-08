using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x0200063F RID: 1599
	public sealed class AuthenticationModuleElement : ConfigurationElement
	{
		// Token: 0x06003185 RID: 12677 RVA: 0x000D42C8 File Offset: 0x000D32C8
		public AuthenticationModuleElement()
		{
			this.properties.Add(this.type);
		}

		// Token: 0x06003186 RID: 12678 RVA: 0x000D4308 File Offset: 0x000D3308
		public AuthenticationModuleElement(string typeName) : this()
		{
			if (typeName != (string)this.type.DefaultValue)
			{
				this.Type = typeName;
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06003187 RID: 12679 RVA: 0x000D432F File Offset: 0x000D332F
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06003188 RID: 12680 RVA: 0x000D4337 File Offset: 0x000D3337
		// (set) Token: 0x06003189 RID: 12681 RVA: 0x000D434A File Offset: 0x000D334A
		[ConfigurationProperty("type", IsRequired = true, IsKey = true)]
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

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x0600318A RID: 12682 RVA: 0x000D4359 File Offset: 0x000D3359
		internal string Key
		{
			get
			{
				return this.Type;
			}
		}

		// Token: 0x04002E8C RID: 11916
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002E8D RID: 11917
		private readonly ConfigurationProperty type = new ConfigurationProperty("type", typeof(string), null, ConfigurationPropertyOptions.IsKey);
	}
}
