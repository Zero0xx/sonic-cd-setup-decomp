using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000643 RID: 1603
	public sealed class BypassElement : ConfigurationElement
	{
		// Token: 0x060031A1 RID: 12705 RVA: 0x000D475C File Offset: 0x000D375C
		public BypassElement()
		{
			this.properties.Add(this.address);
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x000D479C File Offset: 0x000D379C
		public BypassElement(string address) : this()
		{
			this.Address = address;
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x060031A3 RID: 12707 RVA: 0x000D47AB File Offset: 0x000D37AB
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x060031A4 RID: 12708 RVA: 0x000D47B3 File Offset: 0x000D37B3
		// (set) Token: 0x060031A5 RID: 12709 RVA: 0x000D47C6 File Offset: 0x000D37C6
		[ConfigurationProperty("address", IsRequired = true, IsKey = true)]
		public string Address
		{
			get
			{
				return (string)base[this.address];
			}
			set
			{
				base[this.address] = value;
			}
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x060031A6 RID: 12710 RVA: 0x000D47D5 File Offset: 0x000D37D5
		internal string Key
		{
			get
			{
				return this.Address;
			}
		}

		// Token: 0x04002E92 RID: 11922
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002E93 RID: 11923
		private readonly ConfigurationProperty address = new ConfigurationProperty("address", typeof(string), null, ConfigurationPropertyOptions.IsKey);
	}
}
