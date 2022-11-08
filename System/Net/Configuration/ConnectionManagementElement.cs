using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000647 RID: 1607
	public sealed class ConnectionManagementElement : ConfigurationElement
	{
		// Token: 0x060031C1 RID: 12737 RVA: 0x000D49C4 File Offset: 0x000D39C4
		public ConnectionManagementElement()
		{
			this.properties.Add(this.address);
			this.properties.Add(this.maxconnection);
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x000D4A41 File Offset: 0x000D3A41
		public ConnectionManagementElement(string address, int maxConnection) : this()
		{
			this.Address = address;
			this.MaxConnection = maxConnection;
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x060031C3 RID: 12739 RVA: 0x000D4A57 File Offset: 0x000D3A57
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x060031C4 RID: 12740 RVA: 0x000D4A5F File Offset: 0x000D3A5F
		// (set) Token: 0x060031C5 RID: 12741 RVA: 0x000D4A72 File Offset: 0x000D3A72
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

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x060031C6 RID: 12742 RVA: 0x000D4A81 File Offset: 0x000D3A81
		// (set) Token: 0x060031C7 RID: 12743 RVA: 0x000D4A94 File Offset: 0x000D3A94
		[ConfigurationProperty("maxconnection", IsRequired = true, DefaultValue = 1)]
		public int MaxConnection
		{
			get
			{
				return (int)base[this.maxconnection];
			}
			set
			{
				base[this.maxconnection] = value;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x060031C8 RID: 12744 RVA: 0x000D4AA8 File Offset: 0x000D3AA8
		internal string Key
		{
			get
			{
				return this.Address;
			}
		}

		// Token: 0x04002EDE RID: 11998
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002EDF RID: 11999
		private readonly ConfigurationProperty address = new ConfigurationProperty("address", typeof(string), null, ConfigurationPropertyOptions.IsKey);

		// Token: 0x04002EE0 RID: 12000
		private readonly ConfigurationProperty maxconnection = new ConfigurationProperty("maxconnection", typeof(int), 1, ConfigurationPropertyOptions.None);
	}
}
